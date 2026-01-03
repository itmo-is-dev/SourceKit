using Microsoft.CodeAnalysis.Testing;
using SourceKit.Analyzers.MemberAccessibility.Analyzers;
using SourceKit.Analyzers.MemberAccessibility.CodeFixes;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;
using Xunit;

namespace SourceKit.Analyzers.MemberAccessibility.Tests.CodeFixes;

public class FieldCannotHaveMultipleVariablesCodeFixTests : CodeFixTestBase<FieldCannotHaveMultipleVariablesAnalyzer, FieldCannotHaveMultipleVariablesCodeFixProvider>
{
    [Fact]
    public async Task ShouldMakePropertyPublic_WhenDiagnosticReported()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.MemberAccessibility.Samples/MultipleFieldsCase.cs");

        SourceFile fixedSourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.MemberAccessibility.Samples/MultipleFieldsCase.Fixed.cs");

        string fixedContent = fixedSourceFile.Content.Replace("MultipleFieldsCaseFixed", "MultipleFieldsCase");

        fixedSourceFile = sourceFile with { Content = fixedContent };

        DiagnosticResult diagnostic = AnalyzerVerifier
            .Diagnostic(FieldCannotHaveMultipleVariablesAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 5, 5);

        await CodeFixTest
            .WithSource(sourceFile)
            .WithFixedSource(fixedSourceFile)
            .WithExpectedDiagnostic(diagnostic)
            .Build()
            .RunAsync();
    }
}
