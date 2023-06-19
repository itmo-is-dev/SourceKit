using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using SourceKit.Analyzers.Properties.Analyzers;
using SourceKit.Tests.Tools;
using AnalyzerVerifier = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<
    SourceKit.Analyzers.Properties.Analyzers.DeclarationCouldBeConvertedToPropertyAnalyzer>;
using Xunit;

namespace SourceKit.Tests.Analyzers;

public class DeclarationCouldBeConvertedToPropertyTests
{
    [Fact]
    public async Task DeclarationCouldBeConvertedToProperty_ShouldNotReportDiagnostic()
    {
        var sourceFile =
            await SourceFile.LoadAsync(
                "SourceKit.Sample/Analyzers/DeclarationCouldBeConvertedToProperty/PublicProperty.cs");

        var test = new CSharpAnalyzerTest<DeclarationCouldBeConvertedToPropertyAnalyzer, XUnitVerifier>
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
    public async Task DeclarationCouldBeConvertedToProperty_ShouldReportDiagnostic_WhenPublicField()
    {
        var sourceFile =
            await SourceFile.LoadAsync(
                "SourceKit.Sample/Analyzers/DeclarationCouldBeConvertedToProperty/OnePublicField.cs");

        var diagnostic = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 5, 19)
            .WithMessage("Variable field could be converted to property.");

        var test = new CSharpAnalyzerTest<DeclarationCouldBeConvertedToPropertyAnalyzer, XUnitVerifier>
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
    public async Task DeclarationCouldBeConvertedToProperty_ShouldReportAllDiagnostics_WhenEveryPublicField()
    {
        var sourceFile =
            await SourceFile.LoadAsync(
                "SourceKit.Sample/Analyzers/DeclarationCouldBeConvertedToProperty/ManyPublicFields.cs");

        var diagnostic1 = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 5, 19)
            .WithMessage("Variable first could be converted to property.");
        var diagnostic2 = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 5, 26);

        var test = new CSharpAnalyzerTest<DeclarationCouldBeConvertedToPropertyAnalyzer, XUnitVerifier>
        {
            TestState =
            {
                Sources =
                {
                    sourceFile,
                },
            },
            ExpectedDiagnostics = { diagnostic1, diagnostic2 },
        };

        await test.RunAsync();
    }
    
    [Fact]
    public async Task DeclarationCouldBeConvertedToProperty_ShouldReportDiagnostic_WhenField()
    {
        var sourceFile =
            await SourceFile.LoadAsync(
                "SourceKit.Sample/Analyzers/DeclarationCouldBeConvertedToProperty/OneField.cs");

        var diagnostic1 = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 5, 19)
            .WithMessage("Variable first could be converted to property.");
        var diagnostic2 = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 5, 26);

        var test = new CSharpAnalyzerTest<DeclarationCouldBeConvertedToPropertyAnalyzer, XUnitVerifier>
        {
            TestState =
            {
                Sources =
                {
                    sourceFile,
                },
            },
            ExpectedDiagnostics = { diagnostic1, diagnostic2 },
        };

        await test.RunAsync();
    }
}