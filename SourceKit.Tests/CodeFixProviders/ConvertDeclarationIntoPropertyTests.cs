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
    public async Task ConvertPublicDeclarationIntoProperty_ShouldGenerateCorrectProperty()
    {
        var sourceFile = await SourceFile.LoadAsync(OnePublicFieldFilePath);
        var fixedFile = new SourceFile(FixedOnePublicFieldFileName, OnePublicFieldFile);

        var diagnostic = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 5, 19)
            .WithLocation(sourceFile.Name, 5, 19)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "field"));

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
    public async Task ConvertPublicDeclarationsIntoProperties_ShouldGenerateCorrectProperties()
    {
        var sourceFile = await SourceFile.LoadAsync(ManyPublicFieldsFilePath);
        var fixedFile = new SourceFile(FixedManyPublicFieldsFileName, FixedManyPublicFieldsFile);

        var diagnostic1 = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 7, 25)
            .WithLocation(sourceFile.Name, 7, 25)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "first"));
        var diagnostic2 = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 7, 32)
            .WithLocation(sourceFile.Name, 7, 32)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "second"));

        var test = new CSharpCodeFixTest<DeclarationCouldBeConvertedToPropertyAnalyzer,
            ConvertDeclarationIntoPropertyCodeFixProvider,
            XUnitVerifier>
        {
            TestState =
            {
                Sources = { sourceFile },
                ExpectedDiagnostics = { diagnostic1, diagnostic2 },
            },
            FixedState =
            {
                Sources = { fixedFile },
            },
            NumberOfFixAllInDocumentIterations = 2,
            NumberOfFixAllInProjectIterations = 2,
            NumberOfFixAllIterations = 2
        };

        await test.RunAsync();
    }

    [Fact]
    public async Task ConvertDeclarationIntoProperty_ShouldGenerateCorrectProperty()
    {
        var sourceFile = await SourceFile.LoadAsync(OneFieldFilepath);
        var fixedFile = new SourceFile(FixedOneFieldFileName, FixedOneFieldFile);

        var diagnostic1 = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 5, 20)
            .WithLocation(sourceFile.Name, 5, 20)
            .WithLocation(sourceFile.Name, 7, 19)
            .WithLocation(sourceFile.Name, 12, 17)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "field"));
        var diagnostic2 = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 7, 19)
            .WithLocation(sourceFile.Name, 5, 20)
            .WithLocation(sourceFile.Name, 7, 19)
            .WithLocation(sourceFile.Name, 12, 17)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "field"));
        var diagnostic3 = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 12, 17)
            .WithLocation(sourceFile.Name, 5, 20)
            .WithLocation(sourceFile.Name, 7, 19)
            .WithLocation(sourceFile.Name, 12, 17)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "field"));

        var test = new CSharpCodeFixTest<DeclarationCouldBeConvertedToPropertyAnalyzer,
            ConvertDeclarationIntoPropertyCodeFixProvider,
            XUnitVerifier>
        {
            TestState =
            {
                Sources = { sourceFile },
                ExpectedDiagnostics = { diagnostic1, diagnostic2, diagnostic3 },
            },
            FixedState =
            {
                Sources = { fixedFile }
            }
        };

        await test.RunAsync();
    }

    private const string OnePublicFieldFilePath =
        "SourceKit.Sample/Analyzers/DeclarationCouldBeConvertedToProperty/OnePublicField.cs";

    private const string FixedOnePublicFieldFileName = "OnePublicField.cs";

    private const string OnePublicFieldFile = """
namespace SourceKit.Sample.Analyzers.DeclarationCouldBeConvertedToProperty;
public class OnePublicField
{
    public string Field { get; set; }
}
""";

    private const string ManyPublicFieldsFilePath =
        "SourceKit.Sample/Analyzers/DeclarationCouldBeConvertedToProperty/ManyPublicFields.cs";

    private const string FixedManyPublicFieldsFileName = "ManyPublicFields.cs";

    private const string FixedManyPublicFieldsFile = """
using System.Collections.Generic;

namespace SourceKit.Sample.Analyzers.DeclarationCouldBeConvertedToProperty;
public class ManyPublicFields
{
    public List<string> First { get; set; }

    public List<string> Second { get; set; }
}
""";

    private const string OneFieldFilepath =
        "SourceKit.Sample/Analyzers/DeclarationCouldBeConvertedToProperty/OneField.cs";

    private const string FixedOneFieldFileName = "OneField.cs";

    private const string FixedOneFieldFile = """
namespace SourceKit.Sample.Analyzers.DeclarationCouldBeConvertedToProperty;
public class OneField
{
    public string Field { get; set; }
}
""";
}