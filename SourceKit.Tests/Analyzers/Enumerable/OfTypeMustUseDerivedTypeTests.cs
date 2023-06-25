using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using SourceKit.Analyzers.Enumerable.Analyzers;
using SourceKit.Tests.Tools;
using Xunit;
using AnalyzerVerifier = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<
    SourceKit.Analyzers.Enumerable.Analyzers.OfTypeMustUseDerivedTypeAnalyzer>;

namespace SourceKit.Tests.Analyzers.Enumerable;

public class OfTypeMustUseDerivedTypeTests
{
    [Fact]
    public async Task OfTypeMustUseDerivedType_ShouldReportDiagnostic_WhenUsedWithNotDerivedType()
    {
        var sourceFile = await SourceFile.LoadAsync("SourceKit.Sample/Analyzers/Enumerable/OfTypeSample.cs");

        var diagnostic = AnalyzerVerifier.Diagnostic(OfTypeMustUseDerivedTypeAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 10, 16)
            .WithArguments(nameof(Int32), nameof(String));

        var test = new CSharpAnalyzerTest<OfTypeMustUseDerivedTypeAnalyzer, XUnitVerifier>
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
}