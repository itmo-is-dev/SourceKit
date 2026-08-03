using Microsoft.CodeAnalysis.Testing;
using SourceKit.Analyzers.MemberAccessibility;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;

namespace SourceKit.Analyzers.Tests.MemberAccessibility;

public class FieldCannotHaveMultipleVariablesCodeFixTests :
    CodeFixTestBase<FieldCannotHaveMultipleVariablesAnalyzer, FieldCannotHaveMultipleVariablesCodeFixProvider>
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

        DiagnosticResult diagnostic = Diagnostic(FieldCannotHaveMultipleVariablesAnalyzer.Descriptor)
            .WithLocation(sourceFile.FilePath, 5, 5);

        await CodeFixTest
            .WithSource(sourceFile)
            .WithFixedSource(fixedSourceFile)
            .WithExpectedDiagnostic(diagnostic)
            .Build()
            .RunAsync();
    }
}
