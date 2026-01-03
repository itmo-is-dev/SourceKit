using Microsoft.CodeAnalysis.Testing;
using SourceKit.Analyzers.MemberAccessibility.Analyzers;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;
using Xunit;

namespace SourceKit.Analyzers.MemberAccessibility.Tests.Analyzers;

public class FieldCannotHaveMultipleVariablesAnalyzerTests : AnalyzerTestBase<FieldCannotHaveMultipleVariablesAnalyzer>
{
    [Fact]
    public async Task ShouldReportDiagnostic_WhenMultipleFields()
    {
        SourceFile sourceFile = await SourceFile
            .LoadAsync("SourceKit.Analyzers.MemberAccessibility.Samples/MultipleFieldsCase.cs");

        DiagnosticResult diagnostic = AnalyzerVerifier
            .Diagnostic(FieldCannotHaveMultipleVariablesAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 5, 5);

        await AnalyzerTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(diagnostic)
            .Build()
            .RunAsync();
    }
}
