using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using SourceKit.Generators.Builder.Annotations;
using SourceKit.Generators.Builder.Generators;
using SourceKit.Tests.Tools;
using Xunit;
using Verifier = Microsoft.CodeAnalysis.CSharp.Testing.CSharpSourceGeneratorVerifier<
        SourceKit.Generators.Builder.Generators.BuilderSourceGenerator,
        Microsoft.CodeAnalysis.Testing.Verifiers.XUnitVerifier>;

namespace SourceKit.Tests.Generators;

public class BuilderTests
{
    [Fact]
    public async Task A()
    {
        var test = new CSharpSourceGeneratorTest<BuilderSourceGenerator, XUnitVerifier>
        {
            TestState =
            {
                Sources =
                {
                    await SourceFile.LoadAsync("SourceKit.Sample/Generators/SomeQuery.cs"),
                },
                AdditionalReferences = { typeof(GenerateBuilderAttribute).Assembly },
            }
        };

        await test.RunAsync();
    }
}