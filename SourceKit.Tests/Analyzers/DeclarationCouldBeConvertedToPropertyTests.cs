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
    public async Task PublicDeclarationCouldNotBeConvertedToProperty_ShouldNotReportDiagnostic()
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
    public async Task DeclarationCouldNotBeConvertedToProperty_ShouldNotReportDiagnostic()
    {
        var sourceFile =
            await SourceFile.LoadAsync(
                "SourceKit.Sample/Analyzers/DeclarationCouldBeConvertedToProperty/PrivateFieldWithoutMethods.cs");

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
    public async Task DeclarationsCouldBeConvertedToProperties_ShouldReportAllDiagnostics_WhenEveryPublicField()
    {
        var sourceFile =
            await SourceFile.LoadAsync(
                "SourceKit.Sample/Analyzers/DeclarationCouldBeConvertedToProperty/ManyPublicFields.cs");

        var diagnostic1 = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 7, 25)
            .WithMessage("Variable first could be converted to property.");
        var diagnostic2 = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 7, 32)
            .WithMessage("Variable second could be converted to property.");

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

        var diagnostic = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 5, 20)
            .WithLocation(sourceFile.Name, 7, 19)
            .WithLocation(sourceFile.Name, 12, 17)
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
    public async Task DeclarationsCouldBeConvertedToProperties_ShouldReportAllDiagnostics_WhenEveryField()
    {
        var sourceFile =
            await SourceFile.LoadAsync(
                "SourceKit.Sample/Analyzers/DeclarationCouldBeConvertedToProperty/UltimateTest.cs");

        var diagnostic1 = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 7, 31)
            .WithMessage("Variable a could be converted to property.");
        var diagnostic2 = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 7, 34)
            .WithMessage("Variable b could be converted to property.");
        var diagnostic3 = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 8, 26)
            .WithLocation(sourceFile.Name, 11, 28)
            .WithMessage("Variable c could be converted to property.");
        var diagnostic4 = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 9, 20)
            .WithLocation(sourceFile.Name, 16, 22)
            .WithLocation(sourceFile.Name, 21, 20)
            .WithMessage("Variable d could be converted to property.");
        var diagnostic5 = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 26, 17)
            .WithLocation(sourceFile.Name, 28, 19)
            .WithLocation(sourceFile.Name, 33, 18)
            .WithMessage("Variable e could be converted to property.");

        var test = new CSharpAnalyzerTest<DeclarationCouldBeConvertedToPropertyAnalyzer, XUnitVerifier>
        {
            TestState =
            {
                Sources =
                {
                    sourceFile,
                },
            },
            ExpectedDiagnostics = { diagnostic1, diagnostic2, diagnostic3, diagnostic4, diagnostic5 },
        };

        await test.RunAsync();
    }
}