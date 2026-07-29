using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;

namespace SourceKit.Analyzers.MustBePartial;

[Generator]
public class TypeMustBePartialAnalyzer : IIncrementalGenerator
{
    public const string DiagnosticId = "SK1000";
    public const string Title = "Type must be partial";

    public const string Format = """Type "{0}" must be partial""";

    public static readonly DiagnosticDescriptor Descriptor = new(
        DiagnosticId,
        Title,
        Format,
        "Usage",
        DiagnosticSeverity.Error,
        true);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var derivatives = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => node is TypeDeclarationSyntax,
                static INamedTypeSymbol? (context, _) =>
                {
                    var derivativesMustBePartialAttributeType = context.SemanticModel.Compilation
                        .GetTypeByMetadataName(MustBePartialConstants.DerivativesMustBePartialAttributeFullyQualifiedName);

                    if (derivativesMustBePartialAttributeType is null)
                        return null;

                    var typeSymbol = (INamedTypeSymbol)context.SemanticModel.GetDeclaredSymbol(context.Node)!;

                    IEnumerable<INamedTypeSymbol> baseTypesAndInterfaces = typeSymbol.GetBaseTypesAndInterfaces();

                    var mustBePartial = baseTypesAndInterfaces.Any(x => x.HasAttribute(derivativesMustBePartialAttributeType));
                    var isPartial = typeSymbol.IsPartial();

                    if ((mustBePartial, isPartial) is not (true, false))
                        return null;

                    return typeSymbol;
                })
            .Where(x => x is not null);

        context.RegisterSourceOutput(
            derivatives,
            static (context, typeSymbol) =>
            {
                var location = typeSymbol!.GetSignatureLocations().Single();
                var diagnostic = Diagnostic.Create(Descriptor, location, typeSymbol!.Name);

                context.ReportDiagnostic(diagnostic);
            });

        var annotated = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => node is TypeDeclarationSyntax,
                static INamedTypeSymbol? (context, _) =>
                {
                    var annotatedMustBePartialAttributeType = context.SemanticModel.Compilation
                        .GetTypeByMetadataName(MustBePartialConstants.AnnotatedMustBePartialAttributeFullyQualifiedName);

                    if (annotatedMustBePartialAttributeType is null)
                        return null;

                    var typeSymbol = (INamedTypeSymbol)context.SemanticModel.GetDeclaredSymbol(context.Node)!;

                    var attributes = typeSymbol.GetAttributes();

                    var mustBePartial = attributes.Any(x => x.AttributeClass?.HasAttribute(annotatedMustBePartialAttributeType) is true);
                    var isPartial = typeSymbol.IsPartial();

                    if ((mustBePartial, isPartial) is not (true, false))
                        return null;

                    return typeSymbol;
                })
            .Where(x => x is not null);

        context.RegisterSourceOutput(
            annotated,
            static (context, typeSymbol) =>
            {
                var location = typeSymbol!.GetSignatureLocations().Single();
                var diagnostic = Diagnostic.Create(Descriptor, location, typeSymbol!.Name);

                context.ReportDiagnostic(diagnostic);
            });
    }
}
