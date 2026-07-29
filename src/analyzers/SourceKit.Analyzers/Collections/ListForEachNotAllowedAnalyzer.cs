using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

namespace SourceKit.Analyzers.Collections;

[Generator]
public class ListForEachNotAllowedAnalyzer : IIncrementalGenerator
{
    private const string ForEachMethodName = "ForEach";
    private const string ListMetadataName = "System.Collections.Generic.List`1";

    public const string DiagnosticId = "SK1501";
    public const string Title = nameof(ListForEachNotAllowedAnalyzer);
    public const string Format = """Using ForEach method is not allowed""";

    public static readonly DiagnosticDescriptor Descriptor = new(
        DiagnosticId,
        Title,
        Format,
        "Design",
        DiagnosticSeverity.Warning,
        true);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValueProvider<IncrementalResult<INamedTypeSymbol>> listTypeProvider = context.CompilationProvider
            .Select(static compilation => compilation.GetTypeByMetadataName(ListMetadataName) is { } listType ? IncrementalResult.Success(listType) : IncrementalResult.Skip);

        IncrementalValuesProvider<InvocationExpressionSyntax> syntaxProvider = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => node is InvocationExpressionSyntax
                {
                    Expression: MemberAccessExpressionSyntax
                    {
                        Name.Identifier.Text: ForEachMethodName,
                    },
                },
                static (context, _) => ((InvocationExpressionSyntax)context.Node, context.SemanticModel))
            .CombineAndUnwrap(listTypeProvider, context)
            .Select(static (syntax, semanticModel, listType) =>
            {
                if (semanticModel.GetOperation(syntax) is not IInvocationOperation operation
                    || operation.Instance?.Type is not INamedTypeSymbol instanceType
                    || instanceType.ConstructedFrom.Equals(listType, SymbolEqualityComparer.Default) is false)
                {
                    return IncrementalResult.Skip;
                }

                return IncrementalResult.Success(syntax);
            })
            .Unwrap(context);

        context.RegisterSourceOutput(
            syntaxProvider,
            static (context, node) => context.ReportDiagnostic(Diagnostic.Create(Descriptor, node.GetLocation())));
    }
}
