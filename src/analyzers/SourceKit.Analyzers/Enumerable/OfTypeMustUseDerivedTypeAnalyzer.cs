using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using SourceKit.Extensions;

namespace SourceKit.Analyzers.Enumerable;

[Generator]
public class OfTypeMustUseDerivedTypeAnalyzer : IIncrementalGenerator
{
    public const string DiagnosticId = "SK1300";
    public const string Format = """Type {0} is not derived from type {1}""";

    public static readonly DiagnosticDescriptor Descriptor = new(
        DiagnosticId,
        "OfType must be called only for subtypes",
        Format,
        "Usage",
        DiagnosticSeverity.Error,
        true);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var syntaxProvider = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => node is InvocationExpressionSyntax,
                static (context, _) =>
                {
                    if (context.SemanticModel.GetOperation(context.Node) is not IInvocationOperation operation)
                        return IncrementalResult.Skip;

                    if (operation.TargetMethod.Name is not "OfType")
                        return IncrementalResult.Skip;

                    var enumerableStaticSymbol = context.SemanticModel.Compilation.GetTypeSymbol(typeof(System.Linq.Enumerable));
                    var containingType = operation.TargetMethod.ContainingType;

                    if (containingType.Equals(enumerableStaticSymbol, SymbolEqualityComparer.Default) is false)
                        return IncrementalResult.Skip;

                    var argument = operation.Arguments.Single();

                    var value = argument.Value is IConversionOperation conversion
                        ? conversion.Operand
                        : argument.Value;

                    if (value.Type is not INamedTypeSymbol namedSourceType
                        || operation.Type is not INamedTypeSymbol namedReturnType)
                    {
                        return IncrementalResult.Skip;
                    }

                    var sourceElementType = namedSourceType.TypeArguments.SingleOrDefault();
                    var returnElementType = namedReturnType.TypeArguments.Single();

                    if (sourceElementType is null)
                        return IncrementalResult.Skip;

                    if (returnElementType.IsAssignableTo(sourceElementType))
                        return IncrementalResult.Skip;

                    if (sourceElementType is ITypeParameterSymbol parameterSymbol &&
                        parameterSymbol.ConstraintTypes.Any(returnElementType.IsAssignableTo))
                    {
                        return IncrementalResult.Skip;
                    }

                    return IncrementalResult.Success(((InvocationExpressionSyntax)context.Node, returnElementType, sourceElementType));
                })
            .Unwrap(context);

        context.RegisterSourceOutput(
            syntaxProvider,
            static (context, node, returnElementType, sourceElementType) =>
            {
                var diagnostic = Diagnostic.Create(
                    Descriptor,
                    node.GetLocation(),
                    returnElementType.Name,
                    sourceElementType.Name);

                context.ReportDiagnostic(diagnostic);
            });
    }
}
