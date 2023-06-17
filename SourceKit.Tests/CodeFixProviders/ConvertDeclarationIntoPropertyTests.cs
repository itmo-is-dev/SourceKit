using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using SourceKit.Analyzers.Properties.Analyzers;
using SourceKit.Analyzers.Properties.CodeFixes;
using SourceKit.Tests.Tools;
using AnalyzerVerifier = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<
    SourceKit.Analyzers.Properties.Analyzers.DeclarationCouldBeConvertedToPropertyAnalyzer>;
using Xunit;

namespace SourceKit.Tests.CodeFixProviders;

public class ConvertDeclarationIntoPropertyTests
{
    [Fact]
    public async Task ConvertDeclarationIntoProperty_ShouldGenerateCorrectProperty()
    {
        var sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Sample/Analyzers/DeclarationCouldBeConvertedToProperty/OnePublicField.cs");
        var fixedFile = await SourceFile.LoadAsync(
            "CodeFixProviders/TxtSamples/OnePublicField.txt");

        var diagnostic = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 5, 19)
            .WithMessage("Variable field could be converted to property.");

        var test = new CSharpCodeFixTest<DeclarationCouldBeConvertedToPropertyAnalyzer,
            ConvertDeclarationIntoPropertyCodeFixProvider,
            XUnitVerifier>
        {
            TestState =
            {
                Sources = { sourceFile },
                ExpectedDiagnostics = { diagnostic },
            },
            FixedState =
            {
                Sources = { fixedFile }
            }
        };

        await test.RunAsync();
    }

    [Fact]
    public async Task ConvertDeclarationsIntoProperty_ShouldGenerateCorrectProperties()
    {
        var sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Sample/Analyzers/DeclarationCouldBeConvertedToProperty/ManyPublicFields.cs");
        var fixedFile = await SourceFile.LoadAsync(
            "CodeFixProviders/TxtSamples/ManyPublicFields.txt");

        var diagnostic1 = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 5, 19)
            .WithMessage("Variable first could be converted to property.");
        var diagnostic2 = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 5, 26)
            .WithMessage("Variable second could be converted to property.");
        var diagnostic3 = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 6, 19)
            .WithMessage("Variable third could be converted to property.");
        var diagnostic4 = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 6, 26)
            .WithMessage("Variable fourth could be converted to property.");
        var diagnostic5 = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 8, 19)
            .WithMessage("Variable fifth could be converted to property.");

        var test = new CSharpCodeFixTest<DeclarationCouldBeConvertedToPropertyAnalyzer,
            ConvertDeclarationIntoPropertyCodeFixProvider,
            XUnitVerifier>
        {
            TestState =
            {
                Sources = { sourceFile },
                ExpectedDiagnostics = { diagnostic1, diagnostic2, diagnostic3, diagnostic4, diagnostic5 },
            },
            FixedState =
            {
                Sources = { fixedFile }
            }
        };

        await test.RunAsync();
    }
}