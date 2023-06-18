using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SourceKit.Analyzers.Enumerable.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class CannotLinqChainAfterTerminalOperationAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "SK1301";
    public const string Title = nameof(CannotLinqChainAfterTerminalOperationAnalyzer);

    public const string Format = """Cannot chain LINQ methods after terminal operation {0}""";

    public static readonly DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(
        DiagnosticId,
        Title,
        Format,
        "Performance",
        DiagnosticSeverity.Error,
        true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(Descriptor);

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);

        context.RegisterSyntaxNodeAction(c =>
        {
            var semanticModel = c.SemanticModel;
            if (c.Node is not MemberAccessExpressionSyntax node || !IsTerminationMethod(node, semanticModel))
                return;

            var linqNode = node.Ancestors().OfType<MemberAccessExpressionSyntax>()
                .FirstOrDefault(expressionSyntax => IsLinqEnumerable(expressionSyntax, semanticModel));

            if (linqNode == null) return;
            
            var terminationNodeToken= node.GetLastToken();
            c.ReportDiagnostic(Diagnostic.Create(Descriptor, terminationNodeToken.GetLocation()));
        }, SyntaxKind.SimpleMemberAccessExpression);
    }

    private bool IsLinqEnumerable(IMethodSymbol symbol, SemanticModel model)
    {
        var comparer = SymbolEqualityComparer.Default;
        return comparer.Equals(symbol.ContainingType, model.Compilation.GetTypeByMetadataName(typeof(System.Linq.Enumerable).FullName));
    }
    
    private bool IsLinqEnumerable(MemberAccessExpressionSyntax syntax, SemanticModel model)
    {
        var symbol = GetSymbol(syntax, model) ?? throw new InvalidOperationException();
        return IsLinqEnumerable(symbol, model);
    }
    
    private bool ReturnsIEnumerable(IMethodSymbol symbol, SemanticModel model)
    {
        var ienumerableType = model.Compilation.GetTypeByMetadataName("System.Collections.Generic.IEnumerable`1");
        var comparer = SymbolEqualityComparer.Default;
        return comparer.Equals(symbol.ReturnType.OriginalDefinition,ienumerableType);
    }

    private bool IsTerminationMethod(MemberAccessExpressionSyntax syntax, SemanticModel model)
    {
        IMethodSymbol symbol = GetSymbol(syntax, model) ?? throw new InvalidOperationException();
        return IsLinqEnumerable(symbol, model) && !ReturnsIEnumerable(symbol, model);
    }
    
    private IMethodSymbol? GetSymbol(MemberAccessExpressionSyntax syntax, SemanticModel model) 
        => model.GetSymbolInfo(syntax).Symbol as IMethodSymbol;
}