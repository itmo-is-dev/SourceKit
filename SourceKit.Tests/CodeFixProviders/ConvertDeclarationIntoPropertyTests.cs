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
        var fixedFile = new SourceFile(OnePublicFieldName, OnePublicFieldFile);

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
        var fixedFile = new SourceFile(ManyPublicFieldsName, ManyPublicFieldsFile);

        var diagnostic1 = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 5, 19)
            .WithMessage("Variable first could be converted to property.");
        var diagnostic2 = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 5, 26)
            .WithMessage("Variable second could be converted to property.");

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

    private const string OnePublicFieldName = "OnePublicField.cs";

    private const string OnePublicFieldFile = """
namespace SourceKit.Sample.Analyzers.DeclarationCouldBeConvertedToProperty;
public class OnePublicField
{
    public string Field { get; set; }
}
""";

    private const string ManyPublicFieldsName = "ManyPublicFields.cs";

    private const string ManyPublicFieldsFile = """
namespace SourceKit.Sample.Analyzers.DeclarationCouldBeConvertedToProperty;
public class ManyPublicFields
{
    public string First { get; set; }

    public string Second { get; set; }
}
""";
}