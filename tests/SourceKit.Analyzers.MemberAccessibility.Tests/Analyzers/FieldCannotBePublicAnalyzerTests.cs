using Microsoft.CodeAnalysis.Testing;
using SourceKit.Analyzers.MemberAccessibility.Analyzers;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;
using Xunit;

namespace SourceKit.Analyzers.MemberAccessibility.Tests.Analyzers;

public class FieldCannotBePublicAnalyzerTests : AnalyzerTestBase<FieldCannotBePublicAnalyzer>
{
    [Fact]
    public async Task ShouldReportDiagnostic_WhenFieldIsPublic()
    {
        SourceFile sourceFile = await SourceFile
            .LoadAsync("SourceKit.Analyzers.MemberAccessibility.Samples/PublicFieldTestCase.cs");

        DiagnosticResult diagnostic = AnalyzerVerifier
            .Diagnostic(FieldCannotBePublicAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 5, 19)
            .WithArguments("object", "_publicField");

        await AnalyzerTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(diagnostic)
            .Build()
            .RunAsync();
    }
}
