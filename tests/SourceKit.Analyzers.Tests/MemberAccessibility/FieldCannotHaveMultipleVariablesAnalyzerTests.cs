using Microsoft.CodeAnalysis.Testing;
using SourceKit.Analyzers.MemberAccessibility;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;

namespace SourceKit.Analyzers.Tests.MemberAccessibility;

public class FieldCannotHaveMultipleVariablesAnalyzerTests : GeneratorTestBase<FieldCannotHaveMultipleVariablesAnalyzer>
{
    [Fact]
    public async Task ShouldReportDiagnostic_WhenMultipleFields()
    {
        SourceFile sourceFile = await SourceFile
            .LoadAsync("SourceKit.Analyzers.MemberAccessibility.Samples/MultipleFieldsCase.cs");

        DiagnosticResult diagnostic = Diagnostic(FieldCannotHaveMultipleVariablesAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 5, 5);

        await GeneratorTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(diagnostic)
            .Build()
            .RunAsync();
    }
}
