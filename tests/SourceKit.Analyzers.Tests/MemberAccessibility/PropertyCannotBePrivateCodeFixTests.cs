using Microsoft.CodeAnalysis.Testing;
using SourceKit.Analyzers.MemberAccessibility;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;

namespace SourceKit.Analyzers.Tests.MemberAccessibility;

public class PropertyCannotBePrivateCodeFixTests : CodeFixTestBase<PropertyCannotBePrivateAnalyzer, PropertyCannotBePrivateCodeFixProvider>
{
    [Fact]
    public async Task ShouldMakePropertyPublic_WhenDiagnosticReported()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.MemberAccessibility.Samples/PrivatePropertyCase.cs");

        SourceFile fixedSourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.MemberAccessibility.Samples/PrivatePropertyCase.Fixed.cs");

        string fixedContent = fixedSourceFile.Content.Replace("PrivatePropertyCaseFixed", "PrivatePropertyCase");

        fixedSourceFile = sourceFile with { Content = fixedContent };

        DiagnosticResult diagnostic = Diagnostic(PropertyCannotBePrivateAnalyzer.Descriptor)
            .WithLocation(sourceFile.FilePath, 5, 5)
            .WithArguments("object?", "PrivateProperty");

        await CodeFixTest
            .WithSource(sourceFile)
            .WithFixedSource(fixedSourceFile)
            .WithExpectedDiagnostic(diagnostic)
            .Build()
            .RunAsync();
    }
}
