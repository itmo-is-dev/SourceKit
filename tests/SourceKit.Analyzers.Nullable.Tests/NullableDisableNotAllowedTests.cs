using Microsoft.CodeAnalysis.Testing;
using SourceKit.Analyzers.Nullable.Analyzers;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;
using Xunit;

namespace SourceKit.Analyzers.Nullable.Tests;

public class NullableDisableNotAllowedTests : AnalyzerTestBase<NullableDisableNotAllowedAnalyzer>
{
    [Fact]
    public async Task ShouldReportDiagnostic_WhenNullableDisableIsUsed()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Nullable.Samples/NullableDisableNotAllowed/NullableDisable.cs");

        DiagnosticResult diagnostic = AnalyzerVerifier
            .Diagnostic(NullableDisableNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 1, 1);

        await AnalyzerTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(diagnostic)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task ShouldReportNoDiagnostic_WhenNullableEnableIsUsed()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Nullable.Samples/NullableDisableNotAllowed/NullableEnable.cs");

        await AnalyzerTest
            .WithSource(sourceFile)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task NullableDisableNotAllowed_ShouldReport2Diagnostics_WhenNullableDisableIsUsedTwice()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Nullable.Samples/NullableDisableNotAllowed/MultipleNestedNullableDisable.cs");

        DiagnosticResult diagnostic1 = AnalyzerVerifier
            .Diagnostic(NullableDisableNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 1, 1);

        DiagnosticResult diagnostic2 = AnalyzerVerifier
            .Diagnostic(NullableDisableNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 18, 21);

        await AnalyzerTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(diagnostic1)
            .WithExpectedDiagnostic(diagnostic2)
            .Build()
            .RunAsync();
    }
}