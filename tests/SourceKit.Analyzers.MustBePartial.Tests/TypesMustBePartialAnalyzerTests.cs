using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using SourceKit.Analyzers.MustBePartial.Analyzers;
using SourceKit.Analyzers.MustBePartial.Annotations;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;
using Xunit;

namespace SourceKit.Analyzers.MustBePartial.Tests;

public class TypesMustBePartialAnalyzerTests : AnalyzerTestBase<TypeMustBePartialAnalyzer>
{
    [Fact]
    public async Task ShouldReportDiagnostic_WhenTypeIsNotPartial()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.MustBePartial.Samples/NonPartialDerivative.cs");

        SourceFile interfaceSource = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.MustBePartial.Samples/IPartialBase.cs");

        DiagnosticResult diagnostic = AnalyzerVerifier
            .Diagnostic(TypeMustBePartialAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 3, 14)
            .WithArguments("NonPartialDerivative");

        await AnalyzerTest
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
        await AnalyzerTest
            .WithSource(await SourceFile.LoadAsync("SourceKit.Analyzers.MustBePartial.Samples/PartialDerivative.cs"))
            .WithSource(await SourceFile.LoadAsync("SourceKit.Analyzers.MustBePartial.Samples/IPartialBase.cs"))
            .WithAdditionalReference(typeof(DerivativesMustBePartialAttribute).Assembly)
            .Build()
            .RunAsync();
    }
}