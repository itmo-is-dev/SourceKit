using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SourceKit.Analyzers.Nullable.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class NullableDisableNotAllowedAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "SK1201";
    public const string Title = nameof(NullableDisableNotAllowedAnalyzer);

    public const string Format = """#nullable disable is not allowed""";

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
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
    }
}