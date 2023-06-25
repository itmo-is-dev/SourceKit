using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SourceKit.Analyzers.Enumerable.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class DoNotEnumerateCollectionInLinqChain : DiagnosticAnalyzer
{
    public const string DiagnosticId = "SK1301";
    public const string Title = nameof(DoNotEnumerateCollectionInLinqChain);

    public const string Format = """Cannot chain LINQ methods after terminal operation {0}""";
    
        
    internal static HashSet<string> TerminationMethods = new HashSet<string>()
    {
        nameof(System.Linq.Enumerable.ToArray),
        nameof(System.Linq.Enumerable.ToList),
        nameof(System.Linq.Enumerable.ToDictionary),
        nameof(System.Linq.Enumerable.ToLookup),
    };

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

            var hasLinqAncestor = node.Ancestors()
                .OfType<MemberAccessExpressionSyntax>()
                .Any(expressionSyntax => IsLinqEnumerable(expressionSyntax, semanticModel));

            if (!hasLinqAncestor) return;

            var token = node.GetLastToken();
            c.ReportDiagnostic(Diagnostic.Create(Descriptor, token.GetLocation(), node.Name));
        }, SyntaxKind.SimpleMemberAccessExpression);
    }

    private static bool IsTerminationMethod(MemberAccessExpressionSyntax syntax, SemanticModel? model)
    {
        return TerminationMethods.Contains(syntax.GetLastToken().ToString()) && IsLinqEnumerable(syntax, model);
    }
    
    private static bool IsLinqEnumerable(MemberAccessExpressionSyntax syntax, SemanticModel? model)
    {
        try
        {
            var symbol = GetSymbol(syntax, model) ?? throw new InvalidOperationException();
            return IsLinqEnumerable(symbol, model);
        } catch (InvalidOperationException e)
        {
            return false;
        }
    }
    
    
    
    private static bool IsLinqEnumerable(IMethodSymbol? symbol, SemanticModel? model)
    {
        var comparer = SymbolEqualityComparer.Default;
        return comparer.Equals(symbol?.ContainingType, model?.Compilation.GetTypeByMetadataName(typeof(System.Linq.Enumerable).FullName));
    }

    private static IMethodSymbol? GetSymbol(MemberAccessExpressionSyntax syntax, SemanticModel? model) 
        => model?.GetSymbolInfo(syntax).Symbol as IMethodSymbol;
}