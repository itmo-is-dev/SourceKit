using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SourceKit.Extensions;

namespace SourceKit.Analyzers.Enumerable.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class DoNotEnumerateCollectionInLinqChainAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "SK1301";
    public const string Title = nameof(DoNotEnumerateCollectionInLinqChainAnalyzer);

    public const string Format = """Cannot chain LINQ methods after terminal operation {0}""";

    private static readonly HashSet<string> TerminationMethods = new()
    {
        // ToHashSet is missing in standard 2.0
        "ToHashSet",
        nameof(System.Linq.Enumerable.ToLookup),
        nameof(System.Linq.Enumerable.ToArray),
        nameof(System.Linq.Enumerable.ToDictionary),
        nameof(System.Linq.Enumerable.ToList),
        nameof(ImmutableArray.ToImmutableArray),
        nameof(ImmutableDictionary.ToImmutableDictionary),
        nameof(ImmutableList.ToImmutableList),
        nameof(ImmutableHashSet.ToImmutableHashSet),
        nameof(ImmutableSortedDictionary.ToImmutableSortedDictionary),
        nameof(ImmutableSortedSet.ToImmutableSortedSet),
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
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.RegisterSyntaxNodeAction(RegisterDiagnostic, SyntaxKind.InvocationExpression);
    }

    private static void RegisterDiagnostic(SyntaxNodeAnalysisContext context)
    {
        SemanticModel semanticModel = context.SemanticModel;

        if (context.Node is not InvocationExpressionSyntax node || IsTerminationMethod(node, semanticModel) is false)
            return;

        bool hasLinqAncestor = node.Ancestors()
            .OfType<MemberAccessExpressionSyntax>()
            .Any(expressionSyntax => IsLinqEnumerable(expressionSyntax, semanticModel));

        if (hasLinqAncestor is false)
            return;

        ExpressionSyntax terminalOperationExpression = node.Expression;
        SyntaxToken terminalOperationWithoutParamsToken = terminalOperationExpression.GetLastToken();

        context.ReportDiagnostic(
            Diagnostic.Create(
                Descriptor,
                terminalOperationWithoutParamsToken.GetLocation(),
                terminalOperationWithoutParamsToken));
    }

    private static bool IsLinqEnumerable(ExpressionSyntax syntax, SemanticModel model)
    {
        IMethodSymbol? symbol = GetSymbol(syntax, model);
        return IsLinqEnumerable(symbol, model);
    }

    private static bool IsLinqEnumerable(ISymbol? symbol, SemanticModel model)
    {
        Type linqEnumerableType = typeof(System.Linq.Enumerable);
        INamedTypeSymbol linqSymbol = model.Compilation.GetTypeSymbol(linqEnumerableType);
        var comparer = SymbolEqualityComparer.Default;

        return comparer.Equals(symbol?.ContainingType, linqSymbol);
    }

    private static bool IsTerminationMethod(InvocationExpressionSyntax syntax, SemanticModel model)
    {
        ExpressionSyntax expression = syntax.Expression;
        SyntaxToken method = expression.GetLastToken();

        return TerminationMethods.Contains(method.ToString()) && IsLinqEnumerable(syntax, model);
    }

    private static IMethodSymbol? GetSymbol(ExpressionSyntax syntax, SemanticModel model)
        => model.GetSymbolInfo(syntax).Symbol as IMethodSymbol;
}