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
        var sourceFile = await SourceFile.LoadAsync(PublicPropertyFilePath);

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
        var sourceFile = await SourceFile.LoadAsync(PrivateFieldWithoutMethodsFilePath);

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
        var sourceFile = await SourceFile.LoadAsync(OnePublicFieldFilePath);

        var diagnostic = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 5, 19)
            .WithLocation(sourceFile.Name, 5, 19)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "field"));

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
        var sourceFile = await SourceFile.LoadAsync(ManyPublicFieldsFilePath);

        var diagnostic1 = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 7, 25)
            .WithLocation(sourceFile.Name, 7, 25)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "first"));
        var diagnostic2 = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 7, 32)
            .WithLocation(sourceFile.Name, 7, 32)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "second"));

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
        var sourceFile = await SourceFile.LoadAsync(OneFieldFilePath);

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

        var test = new CSharpAnalyzerTest<DeclarationCouldBeConvertedToPropertyAnalyzer, XUnitVerifier>
        {
            TestState =
            {
                Sources =
                {
                    sourceFile,
                },
            },
            ExpectedDiagnostics = { diagnostic1, diagnostic2, diagnostic3 },
        };

        await test.RunAsync();
    }

    [Fact]
    public async Task DeclarationCouldBeConvertedToProperties_ShouldReportDiagnostics_WhenUnderscoreNameField()
    {
        var sourceFile = await SourceFile.LoadAsync(UnderscoreNameFieldFilePath);

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
    public async Task DeclarationCouldBeConvertedToProperties_ShouldReportDiagnostics_WhenMethodsWithExpressionBody()
    {
        var sourceFile = await SourceFile.LoadAsync(ExpressionBodyMethodsFieldFilePath);

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

        var test = new CSharpAnalyzerTest<DeclarationCouldBeConvertedToPropertyAnalyzer, XUnitVerifier>
        {
            TestState =
            {
                Sources =
                {
                    sourceFile,
                },
            },
            ExpectedDiagnostics = { diagnostic1, diagnostic2, diagnostic3 },
        };

        await test.RunAsync();
    }

    [Fact]
    public async Task PublicReadonlyDeclarationCouldBeConvertedToProperties_ShouldReportDiagnostics_WhenField()
    {
        var sourceFile = await SourceFile.LoadAsync(ReadonlyPublicFieldsFilePath);

        var diagnostic = AnalyzerVerifier.Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 5, 28)
            .WithLocation(sourceFile.Name, 5, 28)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "field"));

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

    private const string PublicPropertyFilePath =
        "SourceKit.Sample/Analyzers/DeclarationCouldBeConvertedToProperty/PublicProperty.cs";

    private const string PrivateFieldWithoutMethodsFilePath =
        "SourceKit.Sample/Analyzers/DeclarationCouldBeConvertedToProperty/PrivateFieldWithoutMethods.cs";

    private const string OnePublicFieldFilePath =
        "SourceKit.Sample/Analyzers/DeclarationCouldBeConvertedToProperty/OnePublicField.cs";

    private const string ManyPublicFieldsFilePath =
        "SourceKit.Sample/Analyzers/DeclarationCouldBeConvertedToProperty/ManyPublicFields.cs";

    private const string OneFieldFilePath =
        "SourceKit.Sample/Analyzers/DeclarationCouldBeConvertedToProperty/OneField.cs";

    private const string UnderscoreNameFieldFilePath =
        "SourceKit.Sample/Analyzers/DeclarationCouldBeConvertedToProperty/UnderscoreNameField.cs";

    private const string ExpressionBodyMethodsFieldFilePath =
        "SourceKit.Sample/Analyzers/DeclarationCouldBeConvertedToProperty/ExpressionBodyMethodsField.cs";

    private const string ReadonlyPublicFieldsFilePath =
        "SourceKit.Sample/Analyzers/DeclarationCouldBeConvertedToProperty/ReadonlyPublicField.cs";
}