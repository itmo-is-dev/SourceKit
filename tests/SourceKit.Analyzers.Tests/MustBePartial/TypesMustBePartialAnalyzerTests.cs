using Microsoft.CodeAnalysis.Testing;
using SourceKit.Analyzers.Annotations.MustBePartial;
using SourceKit.Analyzers.MustBePartial;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;

namespace SourceKit.Analyzers.Tests.MustBePartial;

public class TypesMustBePartialAnalyzerTests : GeneratorTestBase<TypeMustBePartialAnalyzer>
{
    [Fact]
    public async Task ShouldReportDiagnostic_WhenTypeIsNotPartial()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.MustBePartial.Samples/NonPartialDerivative.cs");

        SourceFile interfaceSource = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.MustBePartial.Samples/IPartialBase.cs");

        DiagnosticResult diagnostic = Diagnostic(TypeMustBePartialAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 3, 14)
            .WithArguments("NonPartialDerivative");

        await GeneratorTest
            .WithSource(sourceFile)
            .WithSource(interfaceSource)
            .WithAdditionalReference(typeof(DerivativesMustBePartialAttribute).Assembly)
            .WithExpectedDiagnostic(diagnostic)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task ShouldReportNoDiagnostic_WhenTypeIsPartial()
    {
        await GeneratorTest
            .WithSource(await SourceFile.LoadAsync("SourceKit.Analyzers.MustBePartial.Samples/PartialDerivative.cs"))
            .WithSource(await SourceFile.LoadAsync("SourceKit.Analyzers.MustBePartial.Samples/IPartialBase.cs"))
            .WithAdditionalReference(typeof(DerivativesMustBePartialAttribute).Assembly)
            .Build()
            .RunAsync();
    }
}