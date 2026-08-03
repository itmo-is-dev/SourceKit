using Microsoft.CodeAnalysis.Testing;
using SourceKit.Analyzers.MemberAccessibility;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;

namespace SourceKit.Analyzers.Tests.MemberAccessibility;

public class FieldCannotBePublicCodeFixTests : CodeFixTestBase<FieldCannotBePublicAnalyzer, FieldCannotBePublicCodeFixProvider>
{
    [Fact]
    public async Task ShouldMakePropertyPublic_WhenDiagnosticReported()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.MemberAccessibility.Samples/PublicFieldTestCase.cs");

        SourceFile fixedSourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.MemberAccessibility.Samples/PublicFieldTestCase.Fixed.cs");

        string fixedContent = fixedSourceFile.Content.Replace("PublicFieldTestCaseFixed", "PublicFieldTestCase");

        fixedSourceFile = sourceFile with { Content = fixedContent };

        DiagnosticResult diagnostic = Diagnostic(FieldCannotBePublicAnalyzer.Descriptor)
            .WithLocation(sourceFile.FilePath, 5, 20)
            .WithArguments("object?", "_publicField");

        await CodeFixTest
            .WithSource(sourceFile)
            .WithFixedSource(fixedSourceFile)
            .WithExpectedDiagnostic(diagnostic)
            .Build()
            .RunAsync();
    }
}
