using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;

namespace SourceKit.Tests.Common.Tools;

public sealed class SourceGeneratorCodeFixTest<TGenerator, TCodeFixProvider>
    : CSharpCodeFixTest<StubDiagnosticAnalyzer, TCodeFixProvider, DefaultVerifier>
    where TGenerator : IIncrementalGenerator, new()
    where TCodeFixProvider : CodeFixProvider, new()
{
    protected override IEnumerable<Type> GetSourceGenerators()
    {
        yield return typeof(TGenerator);
    }
}
