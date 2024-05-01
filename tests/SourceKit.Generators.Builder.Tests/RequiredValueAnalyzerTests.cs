using Microsoft.CodeAnalysis.Testing;
using SourceKit.Generators.Builder.Analyzers;
using SourceKit.Generators.Builder.Annotations;
using SourceKit.Generators.Builder.Generators;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;
using Xunit;

namespace SourceKit.Generators.Builder.Tests;

public class RequiredValueAnalyzerTests : AnalyzerTestBase<RequiredValueAnalyzer>
{
    [Fact]
    public async Task ShouldProduceError_WhenRequiredValueNotDefined()
    {
        SourceFile usageFile = await SourceFile.LoadAsync(
            "SourceKit.Generators.Builder.Samples/RequiredValue/UninitializedRequiredValueSample.cs");

        DiagnosticResult diagnostic = AnalyzerVerifier
            .Diagnostic(RequiredValueAnalyzer.Descriptor)
            .WithLocation(usageFile.Name, 10, 21)
            .WithArguments("Value");

        await AnalyzerTest
            .WithSource(usageFile)
            .WithAdditionalReference(typeof(GenerateBuilderAttribute).Assembly)
            .WithExpectedDiagnostic(diagnostic)
            .WithSourceGenerator(new BuilderSourceGenerator())
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task ShouldProduceNoError_WhenRequiredValueDefined()
    {
        SourceFile usageFile = await SourceFile.LoadAsync(
            "SourceKit.Generators.Builder.Samples/RequiredValue/InitializedRequiredValueSample.cs");

        await AnalyzerTest
            .WithSource(usageFile)
            .WithAdditionalReference(typeof(GenerateBuilderAttribute).Assembly)
            .WithSourceGenerator(new BuilderSourceGenerator())
            .Build()
            .RunAsync();
    }
}