using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using SourceKit.Extensions;

namespace SourceKit.Analyzers.Enumerable;

[Generator]
public class DoNotEnumerateCollectionInLinqChainAnalyzer : IIncrementalGenerator
{
    public const string DiagnosticId = "SK1301";
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

    public static readonly DiagnosticDescriptor Descriptor = new(
        DiagnosticId,
        "Do not materialize the collection in between LINQ chain",
        Format,
        "Performance",
        DiagnosticSeverity.Error,
        true);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValueProvider<INamedTypeSymbol> enumerableTypeProvider = context.CompilationProvider
            .Select(static compilation => compilation.GetTypeSymbol(typeof(System.Linq.Enumerable)));

        var syntaxProvider = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) =>
                    node is InvocationExpressionSyntax
                    {
                        Expression: MemberAccessExpressionSyntax
                        {
                            Expression: InvocationExpressionSyntax
                            {
                                Expression: MemberAccessExpressionSyntax terminalMemberAccess,
                            },
                        },
                    }
                    && TerminationMethods.Contains(terminalMemberAccess.Name.Identifier.Text),
                static (context, _) => ((InvocationExpressionSyntax)context.Node, context.SemanticModel))
            .Combine(enumerableTypeProvider)
            .WithComparer(static (invocation, _, _) => invocation)
            .Select(static (invocation, semanticModel, enumerableType) =>
            {
                if (semanticModel.GetOperation(invocation) is not IInvocationOperation operation
                    || operation.TargetMethod.ContainingType.Equals(enumerableType, SymbolEqualityComparer.Default) is false)
                {
                    return IncrementalResult.Skip;
                }

                return IncrementalResult.Success(invocation);
            })
            .Unwrap(context);

        context.RegisterSourceOutput(
            syntaxProvider,
            static (context, node) =>
            {
                var latterMemberAccess = (MemberAccessExpressionSyntax)node!.Expression;
                var terminalInvocation = (InvocationExpressionSyntax)latterMemberAccess.Expression;
                var terminalMemberAccess = (MemberAccessExpressionSyntax)terminalInvocation.Expression;

                context.ReportDiagnostic(Diagnostic.Create(
                    Descriptor,
                    terminalMemberAccess.Name.GetLocation(),
                    terminalMemberAccess.Name));
            });
    }
}
