using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using SourceKit.Analyzers.Nullable.Analyzers;
using SourceKit.Tests.Tools;
using Xunit;
using AnalyzerVerifier = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<
    SourceKit.Analyzers.Nullable.Analyzers.NullableDisableNotAllowedAnalyzer>;
namespace SourceKit.Tests.Analyzers;

public class NullableDisableNotAllowedTests
{

    [Fact]
    public async Task NullableDisableNotAllowed_ShouldReportDiagnostic_WhenNullableDisableIsUsed()
    {
        var sourceFile = await SourceFile.LoadAsync("SourceKit.Sample/Analyzers/Nullable/NullableDisable.cs");

        var diagnostic = AnalyzerVerifier.Diagnostic(NullableDisableNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 1, 1);

        var test = new CSharpAnalyzerTest<NullableDisableNotAllowedAnalyzer, XUnitVerifier>
        {
            TestState =
            {
                Sources =
                {
                    sourceFile,
                },
            },
            ExpectedDiagnostics = { diagnostic },
        };

        await test.RunAsync();
    }

    [Fact]
    public async Task OtherNullableKeywordsAllowed_ShouldReportNoDiagnostic_WhenNullableEnableIsUsed()
    {
        var sourceFile = await SourceFile.LoadAsync("SourceKit.Sample/Analyzers/Nullable/NullableEnable.cs");

        var test = new CSharpAnalyzerTest<NullableDisableNotAllowedAnalyzer, XUnitVerifier>
        {
            TestState =
            {
                Sources =
                {
                    sourceFile,
                },
            },
            ExpectedDiagnostics = { },
        };

        await test.RunAsync();
    }
    [Fact]
    public async Task NullableDisableNotAllowed_ShouldReport2Diagnostics_WhenNullableDisableIsUsedTwice()
    {
        var sourceFile = await SourceFile.LoadAsync("SourceKit.Sample/Analyzers/Nullable/MultipleNestedNullableDisable.cs");

        var diagnostic1 = AnalyzerVerifier.Diagnostic(NullableDisableNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 1, 1);
        var diagnostic2 = AnalyzerVerifier.Diagnostic(NullableDisableNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 18, 21);

        var test = new CSharpAnalyzerTest<NullableDisableNotAllowedAnalyzer, XUnitVerifier>
        {
            TestState =
            {
                Sources =
                {
                    sourceFile,
                },

            },
            ExpectedDiagnostics = { diagnostic1, diagnostic2 },
        };

        await test.RunAsync();
    }
}