using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using SourceKit.Analyzers.Collections.Analyzers;
using SourceKit.Tests.Tools;
using AnalyzerVerifier = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<
    SourceKit.Analyzers.MustBePartial.Analyzers.TypeMustBePartialAnalyzer>;
using Xunit;

namespace SourceKit.Tests.Analyzers;

public class CollectionsTests
{
    [Fact]
    public async Task DictionaryKeyType_ShouldReportDiagnostic_WhenTypeIsNotImplementEquatable()
    {
        var sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Sample/Analyzers/Collections/Dictionary/KeyTypeDoesNotImplementEquatable.cs");

        var diagnostic = AnalyzerVerifier.Diagnostic(DictionaryKeyTypeMustImplementEquatableAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 7, 12)
            .WithArguments("KeyTypeDoesNotImplementEquatable");

        var test = new CSharpAnalyzerTest<DictionaryKeyTypeMustImplementEquatableAnalyzer, XUnitVerifier>
        {
            TestState =
            {
                Sources =
                {
                    sourceFile,
                },
            },
            ExpectedDiagnostics = { diagnostic },
        };

        await test.RunAsync();
    }

    [Fact]
    public async Task DictionaryCustomKeyType_ShouldReportDiagnostic_WhenTypeImplementsOtherEquatable()
    {
        var sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Sample/Analyzers/Collections/Dictionary/CustomKeyTypeImplementsOtherEquatable.cs");

        var diagnostic = AnalyzerVerifier.Diagnostic(DictionaryKeyTypeMustImplementEquatableAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 18, 23)
            .WithArguments("CustomKeyTypeImplementsOtherEquatable");

        var test = new CSharpAnalyzerTest<DictionaryKeyTypeMustImplementEquatableAnalyzer, XUnitVerifier>
        {
            TestState =
            {
                Sources =
                {
                    sourceFile,
                },
            },
            ExpectedDiagnostics = { diagnostic },
        };

        await test.RunAsync();
    }

    [Fact]
    public async Task DictionaryKeyType_ShouldNotReportDiagnostic_WhenTypeImplementsEquatable()
    {
        var test = new CSharpAnalyzerTest<DictionaryKeyTypeMustImplementEquatableAnalyzer, XUnitVerifier>
        {
            TestState =
            {
                Sources =
                {
                    await SourceFile.LoadAsync(
                        "SourceKit.Sample/Analyzers/Collections/Dictionary/KeyTypeImplementsEquatable.cs"),
                },
            },
        };

        await test.RunAsync();
    }

    [Fact]
    public async Task DictionaryCustomKeyType_ShouldNotReportDiagnostic_WhenTypeImplementsEquatable()
    {
        var sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Sample/Analyzers/Collections/Dictionary/CustomKeyTypeImplementsEquatable.cs");

        var test = new CSharpAnalyzerTest<DictionaryKeyTypeMustImplementEquatableAnalyzer, XUnitVerifier>
        {
            TestState =
            {
                Sources =
                {
                    sourceFile,
                    await SourceFile.LoadAsync("SourceKit.Sample/Analyzers/Collections/Dictionary/EquatableType.cs"),
                },
            },
        };

        await test.RunAsync();
    }

    [Fact]
    public async Task DictionaryKeyType_ShouldNotReportDiagnostic_WhenTypeIsEmpty()
    {
        var test = new CSharpAnalyzerTest<DictionaryKeyTypeMustImplementEquatableAnalyzer, XUnitVerifier>
        {
            TestState =
            {
                Sources =
                {
                    await SourceFile.LoadAsync("SourceKit.Sample/Analyzers/Collections/Dictionary/EmptyKeyType.cs"),
                },
            },
        };

        await test.RunAsync();
    }

    [Fact]
    public async Task DictionaryInheritedCustomKeyType_ShouldNotReportDiagnostic_WhenTypeImplementsEquatable()
    {
        var sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Sample/Analyzers/Collections/Dictionary/InheritedCustomKeyTypeImplementsEquatable.cs");

        var test = new CSharpAnalyzerTest<DictionaryKeyTypeMustImplementEquatableAnalyzer, XUnitVerifier>
        {
            TestState =
            {
                Sources =
                {
                    sourceFile,
                },
            },
        };

        await test.RunAsync();
    }

    [Fact]
    public async Task DictionaryInheritedCustomKeyType_ShouldNotReportDiagnostic_WhenKeyTypeIsEnum()
    {
        var sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Sample/Analyzers/Collections/Dictionary/EnumKeyDictionary.cs");

        var test = new CSharpAnalyzerTest<DictionaryKeyTypeMustImplementEquatableAnalyzer, XUnitVerifier>
        {
            TestState =
            {
                Sources =
                {
                    sourceFile,
                },
            },
        };

        await test.RunAsync();
    }
}