using Microsoft.CodeAnalysis.Testing;
using SourceKit.Analyzers.MemberAccessibility;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;

namespace SourceKit.Analyzers.Tests.MemberAccessibility;

public class FieldCannotBePublicAnalyzerTests : GeneratorTestBase<FieldCannotBePublicAnalyzer>
{
    [Fact]
    public async Task ShouldReportDiagnostic_WhenFieldIsPublic()
    {
        SourceFile sourceFile = await SourceFile
            .LoadAsync("SourceKit.Analyzers.MemberAccessibility.Samples/PublicFieldTestCase.cs");

        DiagnosticResult diagnostic = Diagnostic(FieldCannotBePublicAnalyzer.Descriptor)
            .WithLocation(sourceFile.FilePath, 5, 20)
            .WithArguments("object?", "_publicField");

        await GeneratorTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(diagnostic)
            .Build()
            .RunAsync();
    }
}
