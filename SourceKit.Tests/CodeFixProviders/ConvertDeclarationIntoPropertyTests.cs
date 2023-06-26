using SourceKit.Analyzers.Properties.Analyzers;
using SourceKit.Tests.Tools;
using AnalyzerVerifier = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<
    SourceKit.Analyzers.Properties.Analyzers.DeclarationCouldBeConvertedToPropertyAnalyzer>;
using CodeFixTest = Microsoft.CodeAnalysis.CSharp.Testing.CSharpCodeFixTest<
    SourceKit.Analyzers.Properties.Analyzers.DeclarationCouldBeConvertedToPropertyAnalyzer,
    SourceKit.Analyzers.Properties.CodeFixes.ConvertDeclarationIntoPropertyCodeFixProvider,
    Microsoft.CodeAnalysis.Testing.Verifiers.XUnitVerifier>;
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

        var test = new CodeFixTest
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

        var test = new CodeFixTest
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
            .WithLocation(sourceFile.Name, 3, 1)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "field"));
        var diagnostic2 = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 7, 19)
            .WithLocation(sourceFile.Name, 5, 20)
            .WithLocation(sourceFile.Name, 3, 1)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "field"));
        var diagnostic3 = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 12, 17)
            .WithLocation(sourceFile.Name, 5, 20)
            .WithLocation(sourceFile.Name, 3, 1)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "field"));

        var test = new CodeFixTest
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

    [Fact]
    public async Task ConvertDeclarationWithUnderscoreIntoProperty_ShouldGenerateCorrectProperty()
    {
        var sourceFile = await SourceFile.LoadAsync(UnderscoreNameFieldFilePath);
        var fixedFile = new SourceFile(FixedUnderscoreNameFieldFileName, FixedUnderscoreNameFieldFile);

        var diagnostic1 = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 5, 29)
            .WithLocation(sourceFile.Name, 5, 29)
            .WithLocation(sourceFile.Name, 3, 1)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "_field"));
        var diagnostic2 = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 7, 19)
            .WithLocation(sourceFile.Name, 5, 29)
            .WithLocation(sourceFile.Name, 3, 1)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "_field"));

        var test = new CodeFixTest
        {
            TestState =
            {
                Sources = { sourceFile },
                ExpectedDiagnostics = { diagnostic1, diagnostic2 },
            },
            FixedState =
            {
                Sources = { fixedFile }
            }
        };

        await test.RunAsync();
    }

    [Fact]
    public async Task ConvertDeclarationWithExpressionBodyIntoProperty_ShouldGenerateCorrectProperty()
    {
        var sourceFile = await SourceFile.LoadAsync(ExpressionBodyMethodsFieldFilePath);
        var fixedFile = new SourceFile(FixedExpressionBodyMethodsFieldFileName, FixedExpressionBodyMethodsFieldFile);

        var diagnostic1 = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 5, 20)
            .WithLocation(sourceFile.Name, 5, 20)
            .WithLocation(sourceFile.Name, 3, 1)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "field"));
        var diagnostic2 = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 7, 19)
            .WithLocation(sourceFile.Name, 5, 20)
            .WithLocation(sourceFile.Name, 3, 1)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "field"));
        var diagnostic3 = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 9, 29)
            .WithLocation(sourceFile.Name, 5, 20)
            .WithLocation(sourceFile.Name, 3, 1)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "field"));

        var test = new CodeFixTest
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
    
    [Fact]
    public async Task ConvertDeclarationWithReadonlyIntoProperty_ShouldGenerateCorrectProperty()
    {
        var sourceFile = await SourceFile.LoadAsync(ReadonlyPublicFieldFilePath);
        var fixedFile = new SourceFile(ReadonlyPublicFieldFileName, ReadonlyPublicFieldFile);

        var diagnostic = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 5, 28)
            .WithLocation(sourceFile.Name, 5, 28)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "field"));

        var test = new CodeFixTest
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

    private const string UnderscoreNameFieldFilePath =
        "SourceKit.Sample/Analyzers/DeclarationCouldBeConvertedToProperty/UnderscoreNameField.cs";

    private const string FixedUnderscoreNameFieldFileName = "UnderscoreNameField.cs";

    private const string FixedUnderscoreNameFieldFile = """
namespace SourceKit.Sample.Analyzers.DeclarationCouldBeConvertedToProperty;

public class UnderscoreNameField
{
    public string Field { get; }
}
""";

    private const string ExpressionBodyMethodsFieldFilePath =
        "SourceKit.Sample/Analyzers/DeclarationCouldBeConvertedToProperty/ExpressionBodyMethodsField.cs";

    private const string FixedExpressionBodyMethodsFieldFileName = "ExpressionBodyMethodsField.cs";

    private const string FixedExpressionBodyMethodsFieldFile = """
namespace SourceKit.Sample.Analyzers.DeclarationCouldBeConvertedToProperty;

public class ExpressionBodyMethodsField
{
    public string Field { get; protected internal set; }
}
""";
    
    private const string ReadonlyPublicFieldFilePath =
        "SourceKit.Sample/Analyzers/DeclarationCouldBeConvertedToProperty/ReadonlyPublicField.cs";

    private const string ReadonlyPublicFieldFileName = "ReadonlyPublicField.cs";

    private const string ReadonlyPublicFieldFile = """
namespace SourceKit.Sample.Analyzers.DeclarationCouldBeConvertedToProperty;

public class ReadonlyPublicField
{
    public string Field { get; }
}
""";
}