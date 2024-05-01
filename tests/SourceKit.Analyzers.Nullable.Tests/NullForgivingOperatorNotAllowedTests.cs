using SourceKit.Analyzers.Nullable.Analyzers;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;
using Xunit;

namespace SourceKit.Analyzers.Nullable.Tests;

public class NullForgivingOperatorNotAllowedTests : AnalyzerTestBase<NullForgivingOperatorNotAllowedAnalyzer>
{
    [Fact]
    public async Task ShouldReportDiagnostic_AllVariationsOfNullSuppression()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Nullable.Samples/NullForgivingOperatorNotAllowed/JokeExample.cs");

        await AnalyzerTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(
                AnalyzerVerifier
                    .Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.Name, 34, 36, 34, 65))
            .WithExpectedDiagnostic(
                AnalyzerVerifier
                    .Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.Name, 36, 35, 36, 68))
            .WithExpectedDiagnostic(
                AnalyzerVerifier
                    .Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.Name, 38, 47, 38, 97))
            .WithExpectedDiagnostic(
                AnalyzerVerifier
                    .Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.Name, 39, 45, 39, 95))
            .WithExpectedDiagnostic(
                AnalyzerVerifier
                    .Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.Name, 40, 46, 40, 75))
            .WithExpectedDiagnostic(
                AnalyzerVerifier
                    .Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.Name, 42, 55, 42, 105))
            .WithExpectedDiagnostic(
                AnalyzerVerifier
                    .Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.Name, 43, 52, 43, 85))
            .WithExpectedDiagnostic(
                AnalyzerVerifier
                    .Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.Name, 46, 13, 46, 63))
            .WithExpectedDiagnostic(
                AnalyzerVerifier
                    .Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.Name, 48, 13, 48, 67))
            .Build()
            .RunAsync();
    }
}