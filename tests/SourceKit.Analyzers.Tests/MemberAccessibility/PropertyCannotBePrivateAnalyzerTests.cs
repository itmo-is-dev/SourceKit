using Microsoft.CodeAnalysis.Testing;
using SourceKit.Analyzers.MemberAccessibility;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;

namespace SourceKit.Analyzers.Tests.MemberAccessibility;

public class PropertyCannotBePrivateAnalyzerTests : GeneratorTestBase<PropertyCannotBePrivateAnalyzer>
{
    [Fact]
    public async Task ShouldReportDiagnostic_WhenPropertyIsPrivate()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.MemberAccessibility.Samples/PrivatePropertyCase.cs");

        DiagnosticResult diagnostic = Diagnostic(PropertyCannotBePrivateAnalyzer.Descriptor)
            .WithLocation(sourceFile.FilePath, 5, 5)
            .WithArguments("object?", "PrivateProperty");

        await GeneratorTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(diagnostic)
            .Build()
            .RunAsync();
    }
}
