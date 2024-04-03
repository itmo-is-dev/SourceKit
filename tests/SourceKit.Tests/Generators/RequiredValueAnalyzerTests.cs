using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using SourceKit.Generators.Builder.Analyzers;
using SourceKit.Generators.Builder.Annotations;
using SourceKit.Tests.Tools;
using Xunit;
using AnalyzerVerifier = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<
    SourceKit.Generators.Builder.Analyzers.RequiredValueAnalyzer>;

namespace SourceKit.Tests.Generators;

public class RequiredValueAnalyzerTests
{
    [Fact]
    public async Task A()
    {
        var usageFile = await SourceFile.LoadAsync("SourceKit.Sample/Generators/ArrayQueryUsage.cs");

        var diagnostic = AnalyzerVerifier.Diagnostic(RequiredValueAnalyzer.Descriptor)
            .WithLocation(usageFile.Name, 12, 21)
            .WithArguments("Value");

        var test = new CSharpAnalyzerTest<RequiredValueAnalyzer, XUnitVerifier>
        {
            TestState =
            {
                Sources = { usageFile },
                AdditionalReferences =
                {
                    typeof(GenerateBuilderAttribute).Assembly,
                },
                ReferenceAssemblies = ReferenceAssemblies.Net.Net60,
            },
            ExpectedDiagnostics = { diagnostic },
        };

        await test.RunAsync();
    }
}