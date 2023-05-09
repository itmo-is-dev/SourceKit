using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SourceKit.Analyzers.Collections.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ListForEachNotAllowedAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "SK1501";
    public const string Title = nameof(DictionaryKeyTypeMustImplementEquatableAnalyzer);

    public const string Format = """Using ForEach method is not allowed""";

    public static readonly DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(
        DiagnosticId,
        Title,
        Format,
        "Design",
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