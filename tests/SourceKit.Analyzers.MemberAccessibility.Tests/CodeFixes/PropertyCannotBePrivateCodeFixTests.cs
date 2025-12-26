using Microsoft.CodeAnalysis.Testing;
using SourceKit.Analyzers.MemberAccessibility.Analyzers;
using SourceKit.Analyzers.MemberAccessibility.CodeFixes;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;
using Xunit;

namespace SourceKit.Analyzers.MemberAccessibility.Tests.CodeFixes;

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

        DiagnosticResult diagnostic = AnalyzerVerifier
            .Diagnostic(PropertyCannotBePrivateAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 5, 5)
            .WithArguments("object", "PrivateProperty");

        await CodeFixTest
            .WithSource(sourceFile)
            .WithFixedSource(fixedSourceFile)
            .WithExpectedDiagnostic(diagnostic)
            .Build()
            .RunAsync();
    }
}
