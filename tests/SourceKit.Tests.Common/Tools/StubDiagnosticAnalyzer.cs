using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SourceKit.Tests.Common.Tools;

#pragma warning disable RS1001
public sealed class StubDiagnosticAnalyzer : DiagnosticAnalyzer
{
    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();

        context.ConfigureGeneratedCodeAnalysis(
            GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
    }

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [];
}
