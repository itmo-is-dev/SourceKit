using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using SourceKit.Analyzers.MustBePartial.Tools;
using SourceKit.Extensions;

namespace SourceKit.Analyzers.MustBePartial.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public class DerivativesMustBePartialAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "SK1000";
    public const string Title = nameof(DerivativesMustBePartialAnalyzer);

    public const string Format = """Type "{0}" must be partial""";

    public static readonly DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(
        DiagnosticId,
        Title,
        Format,
        "Class definition",
        DiagnosticSeverity.Error,
        true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(Descriptor);

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(
            GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);

        context.RegisterCompilationStartAction(compilationContext =>
        {
            var derivativesMustBePartialAttributeType = compilationContext.Compilation
                .GetTypeByMetadataName(Constants.DerivativesMustBePartialAttributeFullyQualifiedName);

            if (derivativesMustBePartialAttributeType is null)
                return;

            compilationContext.RegisterSymbolAction(
                x => AnalyzeTypeSymbol(x, derivativesMustBePartialAttributeType),
                SymbolKind.NamedType);
        });
    }

    private static void AnalyzeTypeSymbol(SymbolAnalysisContext context, INamedTypeSymbol attributeSymbol)
    {
        context.CancellationToken.ThrowIfCancellationRequested();

        var typeSymbol = (INamedTypeSymbol)context.Symbol;

        IEnumerable<INamedTypeSymbol> baseTypesAndInterfaces = typeSymbol.GetBaseTypesAndInterfaces();

        var mustBePartial = baseTypesAndInterfaces.Any(x => x.HasAttribute(attributeSymbol));
        var isPartial = typeSymbol.IsPartial();

        if ((mustBePartial, isPartial) is not (true, false))
            return;

        var location = typeSymbol.GetSignatureLocations().Single();
        var diagnostic = Diagnostic.Create(Descriptor, location, typeSymbol.Name);

        context.ReportDiagnostic(diagnostic);
    }
}