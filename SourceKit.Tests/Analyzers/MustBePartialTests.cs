using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using SourceKit.Analyzers.MustBePartial.Analyzers;
using Xunit;
using Verify = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<
    SourceKit.Analyzers.MustBePartial.Analyzers.DerivativesMustBePartialAnalyzer>;

namespace SourceKit.Tests.Analyzers;

public class MustBePartialTests
{
    [Fact]
    public async Task A()
    {
        var subject = await File.ReadAllTextAsync("MustBePartial/NotPartialDerivative.cs");

        var test = new CSharpAnalyzerTest<DerivativesMustBePartialAnalyzer, XUnitVerifier>
        {
            TestState =
            {
                Sources = { subject },
                AdditionalFiles = { await LoadFileAsync("MustBePartial/IPartialBase.cs") },
                ExpectedDiagnostics = { Verify.Diagnostic(DerivativesMustBePartialAnalyzer.Descriptor) },
            },
        };

        await test.RunAsync();
    }

    private static async Task<(string name, string content)> LoadFileAsync(string path)
    {
        var name = Path.ChangeExtension(Path.GetFileName(path), null);
        var content = await File.ReadAllTextAsync(path);

        return (name, content);
    }
}