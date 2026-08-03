using Microsoft.CodeAnalysis.Testing;
using SourceKit.Analyzers.Nullable;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;

namespace SourceKit.Analyzers.Tests.Nullable;

public class NullableDisableNotAllowedTests : GeneratorTestBase<NullableDisableNotAllowedAnalyzer>
{
    [Fact]
    public async Task ShouldReportDiagnostic_WhenNullableDisableIsUsed()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Nullable.Samples/NullableDisableNotAllowed/NullableDisable.cs");

        DiagnosticResult diagnostic = Diagnostic(NullableDisableNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.FilePath, 1, 1);

        await GeneratorTest
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

        await GeneratorTest
            .WithSource(sourceFile)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task NullableDisableNotAllowed_ShouldReport2Diagnostics_WhenNullableDisableIsUsedTwice()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Nullable.Samples/NullableDisableNotAllowed/MultipleNestedNullableDisable.cs");

        DiagnosticResult diagnostic1 = Diagnostic(NullableDisableNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.FilePath, 1, 1);

        DiagnosticResult diagnostic2 = Diagnostic(NullableDisableNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.FilePath, 18, 21);

        await GeneratorTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(diagnostic1)
            .WithExpectedDiagnostic(diagnostic2)
            .Build()
            .RunAsync();
    }
}