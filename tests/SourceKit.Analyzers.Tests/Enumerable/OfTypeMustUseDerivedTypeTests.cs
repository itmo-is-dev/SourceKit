using Microsoft.CodeAnalysis.Testing;
using SourceKit.Analyzers.Enumerable;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;

namespace SourceKit.Analyzers.Tests.Enumerable;

public class OfTypeMustUseDerivedTypeTests : GeneratorTestBase<OfTypeMustUseDerivedTypeAnalyzer>
{
    [Fact]
    public async Task ShouldReportDiagnostic_WhenUsedWithNotDerivedType()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Enumerable.Samples/OfTypeMustUseDerivedType/OfTypeSample.cs");

        DiagnosticResult diagnostic = Diagnostic(OfTypeMustUseDerivedTypeAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 10, 16)
            .WithArguments(nameof(Int32), nameof(String));

        await GeneratorTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(diagnostic)
            .Build()
            .RunAsync();
    }
}