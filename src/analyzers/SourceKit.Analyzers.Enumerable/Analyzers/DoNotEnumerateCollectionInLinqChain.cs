using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SourceKit.Extensions;

namespace SourceKit.Analyzers.Enumerable.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class DoNotEnumerateCollectionInLinqChain : DiagnosticAnalyzer
{
    public const string DiagnosticId = "SK1301";
    public const string Title = nameof(DoNotEnumerateCollectionInLinqChain);

    public const string Format = """Cannot chain LINQ methods after terminal operation {0}""";


    private static readonly HashSet<string> TerminationMethods = new HashSet<string>()
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
        context.RegisterSyntaxNodeAction(RegisterDiagnostic, SyntaxKind.SimpleMemberAccessExpression);
    }

    private static void RegisterDiagnostic(SyntaxNodeAnalysisContext context)
    {
        SemanticModel semanticModel = context.SemanticModel;
        
        if (context.Node is not MemberAccessExpressionSyntax node || IsTerminationMethod(node, semanticModel) is false)
            return;

        bool hasLinqAncestor = node.Ancestors()
            .OfType<MemberAccessExpressionSyntax>()
            .Any(expressionSyntax => IsLinqEnumerable(expressionSyntax, semanticModel));

        if (hasLinqAncestor is false) return;

        SyntaxToken terminalOperationWithoutParamsToken = node.GetLastToken();
        context.ReportDiagnostic(Diagnostic.Create(Descriptor, terminalOperationWithoutParamsToken.GetLocation(), node.Name));
    }

    private static bool IsLinqEnumerable(ExpressionSyntax syntax, SemanticModel model)
    {
        IMethodSymbol? symbol = GetSymbol(syntax, model);
        return IsLinqEnumerable(symbol, model);
    }
    
    private static bool IsLinqEnumerable(ISymbol? symbol, SemanticModel model)
    {
        INamedTypeSymbol linqSymbol = model.Compilation.GetTypeSymbol(typeof(System.Linq.Enumerable));
        var comparer = SymbolEqualityComparer.Default;
        
        return comparer.Equals(symbol?.ContainingType, linqSymbol);
    }
    
    private static bool IsTerminationMethod(ExpressionSyntax syntax, SemanticModel model)
        => TerminationMethods.Contains(syntax.GetLastToken().ToString()) && IsLinqEnumerable(syntax, model);

    private static IMethodSymbol? GetSymbol(ExpressionSyntax syntax, SemanticModel model) 
        => model.GetSymbolInfo(syntax).Symbol as IMethodSymbol;
}