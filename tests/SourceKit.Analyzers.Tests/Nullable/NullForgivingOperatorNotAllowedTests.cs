using SourceKit.Analyzers.Nullable;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;

namespace SourceKit.Analyzers.Tests.Nullable;

public class NullForgivingOperatorNotAllowedTests : GeneratorTestBase<NullForgivingOperatorNotAllowedAnalyzer>
{
    [Fact]
    public async Task ShouldReportDiagnostic_AllVariationsOfNullSuppression()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Nullable.Samples/NullForgivingOperatorNotAllowed/JokeExample.cs");

        await GeneratorTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(
                Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.FilePath, 34, 36, 34, 65))
            .WithExpectedDiagnostic(
                Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.FilePath, 36, 35, 36, 68))
            .WithExpectedDiagnostic(
                Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.FilePath, 38, 47, 38, 97))
            .WithExpectedDiagnostic(
                Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.FilePath, 39, 45, 39, 95))
            .WithExpectedDiagnostic(
                Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.FilePath, 40, 46, 40, 75))
            .WithExpectedDiagnostic(
                Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.FilePath, 42, 55, 42, 105))
            .WithExpectedDiagnostic(
                Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.FilePath, 43, 52, 43, 85))
            .WithExpectedDiagnostic(
                Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.FilePath, 46, 13, 46, 63))
            .WithExpectedDiagnostic(
                Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.FilePath, 48, 13, 48, 67))
            .Build()
            .RunAsync();
    }
}
