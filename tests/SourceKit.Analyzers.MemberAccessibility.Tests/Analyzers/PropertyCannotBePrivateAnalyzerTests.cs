using Microsoft.CodeAnalysis.Testing;
using SourceKit.Analyzers.MemberAccessibility.Analyzers;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;
using Xunit;

namespace SourceKit.Analyzers.MemberAccessibility.Tests.Analyzers;

public class PropertyCannotBePrivateAnalyzerTests : AnalyzerTestBase<PropertyCannotBePrivateAnalyzer>
{
    [Fact]
    public async Task ShouldReportDiagnostic_WhenPropertyIsPrivate()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.MemberAccessibility.Samples/PrivatePropertyCase.cs");

        DiagnosticResult diagnostic = AnalyzerVerifier
            .Diagnostic(PropertyCannotBePrivateAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 5, 5)
            .WithArguments("object", "PrivateProperty");

        await AnalyzerTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(diagnostic)
            .Build()
            .RunAsync();
    }
}
