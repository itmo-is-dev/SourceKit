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


    private static readonly HashSet<string> TerminationMethods = new()
    {
        // ToHashSet is missing in standard 2.0
        "ToHashSet",
        nameof(System.Linq.Enumerable.ToLookup),
        nameof(System.Linq.Enumerable.ToArray),
        nameof(System.Linq.Enumerable.ToDictionary),
        nameof(System.Linq.Enumerable.ToList),
        nameof(System.Collections.Immutable.ImmutableArray.ToImmutableArray),
        nameof(System.Collections.Immutable.ImmutableDictionary.ToImmutableDictionary),
        nameof(System.Collections.Immutable.ImmutableList.ToImmutableList),
        nameof(System.Collections.Immutable.ImmutableHashSet.ToImmutableHashSet),
        nameof(System.Collections.Immutable.ImmutableSortedDictionary.ToImmutableSortedDictionary),
        nameof(System.Collections.Immutable.ImmutableSortedSet.ToImmutableSortedSet),
    };

    public static readonly DiagnosticDescriptor Descriptor = new(
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

        if (hasLinqAncestor is false) return;

        ExpressionSyntax terminalOperationExpression = node.Expression;
        SyntaxToken terminalOperationWithoutParamsToken = terminalOperationExpression.GetLastToken();
        
        context.ReportDiagnostic(Diagnostic.Create(Descriptor, terminalOperationWithoutParamsToken.GetLocation(), terminalOperationWithoutParamsToken));
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