using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using SourceKit.Analyzers.Nullable.Analyzers;
using SourceKit.Tests.Tools;
using Xunit;
using AnalyzerVerifier = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<
    SourceKit.Analyzers.Nullable.Analyzers.NullForgivingOperatorNotAllowedAnalyzer>;
using CodeFixTest = Microsoft.CodeAnalysis.CSharp.Testing.CSharpCodeFixTest<
    SourceKit.Analyzers.Nullable.Analyzers.NullForgivingOperatorNotAllowedAnalyzer,
    SourceKit.Analyzers.Nullable.CodeFixes.PropagateNullableCorrectlyCodeFixProvider,
    Microsoft.CodeAnalysis.Testing.Verifiers.XUnitVerifier>;

namespace SourceKit.Tests.Analyzers;

public class NullForgivingOperatorNotAllowedTests
{
    [Fact]
    public async Task NullForgivingOperatorNotAllowed_ShouldReportDiagnostic_AllVariationsOfNullSuppression()
    {
        var sourceFile = await SourceFile.LoadAsync("SourceKit.Sample/Analyzers/NullForgivingOperatorNotAllowed/JokeExample.cs");

        var test = new CSharpAnalyzerTest<NullForgivingOperatorNotAllowedAnalyzer, XUnitVerifier>
        {
            TestState =
            {
                Sources =
                {
                    sourceFile
                }
            },
            ExpectedDiagnostics = {
                AnalyzerVerifier.Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.Name, 34, 36, 34, 65),
                AnalyzerVerifier.Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.Name, 36, 35, 36, 68),
                AnalyzerVerifier.Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.Name, 38, 47, 38, 97),
                AnalyzerVerifier.Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.Name, 39, 45, 39, 95),
                AnalyzerVerifier.Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.Name, 40, 46, 40, 75),
                AnalyzerVerifier.Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.Name, 42, 55, 42, 105),
                AnalyzerVerifier.Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.Name, 43, 52, 43, 85),
                AnalyzerVerifier.Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.Name, 46, 13, 46, 63),
                AnalyzerVerifier.Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.Name, 48, 13, 48, 67),
            } ,
        };

        await test.RunAsync();
    }
}