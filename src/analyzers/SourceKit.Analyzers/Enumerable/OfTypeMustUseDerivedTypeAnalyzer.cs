using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using SourceKit.Extensions;

namespace SourceKit.Analyzers.Enumerable;

[Generator]
public class OfTypeMustUseDerivedTypeAnalyzer : IIncrementalGenerator
{
    public const string DiagnosticId = "SK1300";
    public const string Title = nameof(OfTypeMustUseDerivedTypeAnalyzer);

    public const string Format = """Type {0} is not derived from type {1}""";

    public static readonly DiagnosticDescriptor Descriptor = new(
        DiagnosticId,
        Title,
        Format,
        "Usage",
        DiagnosticSeverity.Error,
        true);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var syntaxProvider = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => node is InvocationExpressionSyntax,
                static (InvocationExpressionSyntax, ITypeSymbol, ITypeSymbol)? (context, _) =>
                {
                    var operation = (IInvocationOperation)context.SemanticModel.GetOperation(context.Node)!;

                    if (operation.TargetMethod.Name is not "OfType")
                        return null;

                    var enumerableStaticSymbol = context.SemanticModel.Compilation.GetTypeSymbol(typeof(System.Linq.Enumerable));
                    var containingType = operation.TargetMethod.ContainingType;

                    if (containingType.Equals(enumerableStaticSymbol, SymbolEqualityComparer.Default) is false)
                        return null;

                    var argument = operation.Arguments.Single();

                    var value = argument.Value is IConversionOperation conversion
                        ? conversion.Operand
                        : argument.Value;

                    if (value.Type is not INamedTypeSymbol namedSourceType
                        || operation.Type is not INamedTypeSymbol namedReturnType)
                    {
                        return null;
                    }

                    var sourceElementType = namedSourceType.TypeArguments.SingleOrDefault();
                    var returnElementType = namedReturnType.TypeArguments.Single();

                    if (sourceElementType is null)
                        return null;

                    if (returnElementType.IsAssignableTo(sourceElementType))
                        return null;

                    if (sourceElementType is ITypeParameterSymbol parameterSymbol &&
                        parameterSymbol.ConstraintTypes.Any(c => returnElementType.IsAssignableTo(c)))
                    {
                        return null;
                    }

                    return ((InvocationExpressionSyntax)context.Node, returnElementType, sourceElementType);
                })
            .Where(node => node is not null);

        context.RegisterSourceOutput(
            syntaxProvider,
            static (context, tuple) =>
            {
                var (node, returnElementType, sourceElementType) = tuple!.Value;

                var diagnostic = Diagnostic.Create(
                    Descriptor,
                    node.GetLocation(),
                    returnElementType.Name,
                    sourceElementType.Name);

                context.ReportDiagnostic(diagnostic);
            });
    }
}
