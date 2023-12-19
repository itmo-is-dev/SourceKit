using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using SourceKit.Analyzers.MustBePartial.Tools;
using SourceKit.Extensions;

namespace SourceKit.Analyzers.MustBePartial.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class TypeMustBePartialAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "SK1000";
    public const string Title = "Type must be partial";

    public const string Format = """Type "{0}" must be partial""";

    public static readonly DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(
        DiagnosticId,
        Title,
        Format,
        "Usage",
        DiagnosticSeverity.Error,
        true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(Descriptor);

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

        context.RegisterCompilationStartAction(compilationContext =>
        {
            var derivativesMustBePartialAttributeType = compilationContext.Compilation
                .GetTypeByMetadataName(Constants.DerivativesMustBePartialAttributeFullyQualifiedName);

            if (derivativesMustBePartialAttributeType is not null)
            {
                compilationContext.RegisterSymbolAction(
                    x => AnalyzeDerivatives(x, derivativesMustBePartialAttributeType),
                    SymbolKind.NamedType);
            }

            var annotatedMustBePartialAttributeType = compilationContext.Compilation
                .GetTypeByMetadataName(Constants.AnnotatedMustBePartialAttributeFullyQualifiedName);

            if (annotatedMustBePartialAttributeType is not null)
            {
                compilationContext.RegisterSymbolAction(
                    x => AnalyzeAnnotated(x, annotatedMustBePartialAttributeType),
                    SymbolKind.NamedType);
            }
        });
    }

    private static void AnalyzeDerivatives(SymbolAnalysisContext context, INamedTypeSymbol attribute)
    {
        context.CancellationToken.ThrowIfCancellationRequested();

        var typeSymbol = (INamedTypeSymbol)context.Symbol;

        IEnumerable<INamedTypeSymbol> baseTypesAndInterfaces = typeSymbol.GetBaseTypesAndInterfaces();

        var mustBePartial = baseTypesAndInterfaces.Any(x => x.HasAttribute(attribute));
        var isPartial = typeSymbol.IsPartial();

        if ((mustBePartial, isPartial) is not (true, false))
            return;

        var location = typeSymbol.GetSignatureLocations().Single();
        var diagnostic = Diagnostic.Create(Descriptor, location, typeSymbol.Name);

        context.ReportDiagnostic(diagnostic);
    }

    private void AnalyzeAnnotated(SymbolAnalysisContext context, INamedTypeSymbol attribute)
    {
        context.CancellationToken.ThrowIfCancellationRequested();

        var typeSymbol = (INamedTypeSymbol)context.Symbol;
        var attributes = typeSymbol.GetAttributes();

        var mustBePartial = attributes.Any(x => x.AttributeClass?.HasAttribute(attribute) is true);
        var isPartial = typeSymbol.IsPartial();

        if ((mustBePartial, isPartial) is not (true, false))
            return;

        var location = typeSymbol.GetSignatureLocations().Single();
        var diagnostic = Diagnostic.Create(Descriptor, location, typeSymbol.Name);

        context.ReportDiagnostic(diagnostic);
    }
}