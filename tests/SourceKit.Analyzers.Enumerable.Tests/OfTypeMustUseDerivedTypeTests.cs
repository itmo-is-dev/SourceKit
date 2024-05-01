using Microsoft.CodeAnalysis.Testing;
using SourceKit.Analyzers.Enumerable.Analyzers;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;
using Xunit;

namespace SourceKit.Analyzers.Enumerable.Tests;

public class OfTypeMustUseDerivedTypeTests : AnalyzerTestBase<OfTypeMustUseDerivedTypeAnalyzer>
{
    [Fact]
    public async Task ShouldReportDiagnostic_WhenUsedWithNotDerivedType()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Enumerable.Samples/OfTypeMustUseDerivedType/OfTypeSample.cs");

        DiagnosticResult diagnostic = AnalyzerVerifier
            .Diagnostic(OfTypeMustUseDerivedTypeAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 10, 16)
            .WithArguments(nameof(Int32), nameof(String));

        await AnalyzerTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(diagnostic)
            .Build()
            .RunAsync();
    }
}