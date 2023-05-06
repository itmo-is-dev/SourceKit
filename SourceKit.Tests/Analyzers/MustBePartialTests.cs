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
        var subject = File.ReadAllText("MustBePartial/NotPartialDerivative.cs");

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

    private static Task<(string name, string content)> LoadFileAsync(string path)
    {
        var name = Path.ChangeExtension(Path.GetFileName(path), null);
        var content = File.ReadAllText(path);

        return Task.FromResult((name, content));
    }
}