using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SourceKit.Analyzers.Enumerable.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class CannotLinqChainAfterTerminalOperationAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "SK1301";
    public const string Title = nameof(CannotLinqChainAfterTerminalOperationAnalyzer);

    public const string Format = """Cannot chain LINQ methods after terminal operation {0}""";

    public static readonly DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(
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
    }
}