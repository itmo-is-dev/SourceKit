using Microsoft.CodeAnalysis.Testing;
using SourceKit.Analyzers.Collections.Analyzers;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;
using Xunit;

namespace SourceKit.Analyzers.Collections.Tests;

public class CollectionsTests : AnalyzerTestBase<DictionaryKeyTypeMustImplementEquatableAnalyzer>
{
    [Fact]
    public async Task DictionaryKeyType_ShouldReportDiagnostic_WhenTypeIsNotImplementEquatable()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Collections.Samples/Dictionary/KeyTypeDoesNotImplementEquatable.cs");

        DiagnosticResult diagnostic = AnalyzerVerifier
            .Diagnostic(DictionaryKeyTypeMustImplementEquatableAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 7, 12)
            .WithArguments("KeyTypeDoesNotImplementEquatable");

        await AnalyzerTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(diagnostic)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task DictionaryCustomKeyType_ShouldReportDiagnostic_WhenTypeImplementsOtherEquatable()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Collections.Samples/Dictionary/CustomKeyTypeImplementsOtherEquatable.cs");

        DiagnosticResult diagnostic = AnalyzerVerifier
            .Diagnostic(DictionaryKeyTypeMustImplementEquatableAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 18, 23)
            .WithArguments("CustomKeyTypeImplementsOtherEquatable");

        await AnalyzerTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(diagnostic)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task DictionaryKeyType_ShouldNotReportDiagnostic_WhenTypeImplementsEquatable()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Collections.Samples/Dictionary/KeyTypeImplementsEquatable.cs");

        await AnalyzerTest
            .WithSource(sourceFile)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task DictionaryCustomKeyType_ShouldNotReportDiagnostic_WhenTypeImplementsEquatable()
    {
        SourceFile customKeySourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Collections.Samples/Dictionary/CustomKeyTypeImplementsEquatable.cs");

        SourceFile equatableSourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Collections.Samples/Dictionary/EquatableType.cs");

        await AnalyzerTest
            .WithSource(customKeySourceFile)
            .WithSource(equatableSourceFile)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task DictionaryKeyType_ShouldNotReportDiagnostic_WhenTypeIsEmpty()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Collections.Samples/Dictionary/EmptyKeyType.cs");

        await AnalyzerTest
            .WithSource(sourceFile)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task DictionaryInheritedCustomKeyType_ShouldNotReportDiagnostic_WhenTypeImplementsEquatable()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Collections.Samples/Dictionary/InheritedCustomKeyTypeImplementsEquatable.cs");

        await AnalyzerTest
            .WithSource(sourceFile)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task DictionaryInheritedCustomKeyType_ShouldNotReportDiagnostic_WhenKeyTypeIsEnum()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Collections.Samples/Dictionary/EnumKeyDictionary.cs");

        await AnalyzerTest
            .WithSource(sourceFile)
            .Build()
            .RunAsync();
    }
}