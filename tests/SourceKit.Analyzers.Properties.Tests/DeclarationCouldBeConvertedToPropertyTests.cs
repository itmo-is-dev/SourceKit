using Microsoft.CodeAnalysis.Testing;
using SourceKit.Analyzers.Properties.Analyzers;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;
using Xunit;

namespace SourceKit.Analyzers.Properties.Tests;

public class DeclarationCouldBeConvertedToPropertyTests : AnalyzerTestBase<
    DeclarationCouldBeConvertedToPropertyAnalyzer>
{
    [Fact]
    public async Task ShouldReportNoDiagnostic_WhenPublicPropertyDeclared()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Properties.Samples/DeclarationCouldBeConvertedToProperty/PublicProperty.cs");

        await AnalyzerTest
            .WithSource(sourceFile)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task ShouldReportNoDiagnostic_WhenPrivateFieldWithNoGetterAndSetterDeclared()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Properties.Samples/DeclarationCouldBeConvertedToProperty/PrivateFieldWithoutMethods.cs");

        await AnalyzerTest
            .WithSource(sourceFile)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task ShouldReportDiagnostic_WhenPublicFieldDeclared()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Properties.Samples/DeclarationCouldBeConvertedToProperty/OnePublicField.cs");

        DiagnosticResult diagnostic = AnalyzerVerifier
            .Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 5, 19)
            .WithLocation(sourceFile.Name, 5, 19)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "field"));

        await AnalyzerTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(diagnostic)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task ShouldReportDiagnotics_WhenMultiplePublicFieldDeclared()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Properties.Samples/DeclarationCouldBeConvertedToProperty/ManyPublicFields.cs");

        DiagnosticResult diagnostic1 = AnalyzerVerifier
            .Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 7, 25)
            .WithLocation(sourceFile.Name, 7, 25)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "first"));

        DiagnosticResult diagnostic2 = AnalyzerVerifier
            .Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 7, 32)
            .WithLocation(sourceFile.Name, 7, 32)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "second"));

        await AnalyzerTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(diagnostic1)
            .WithExpectedDiagnostic(diagnostic2)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task ShouldReportDiagnostic_WhenPrivateFieldWithGetterAndSetterDeclared()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Properties.Samples/DeclarationCouldBeConvertedToProperty/OneField.cs");

        DiagnosticResult diagnostic1 = AnalyzerVerifier
            .Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 5, 20)
            .WithLocation(sourceFile.Name, 5, 20)
            .WithLocation(sourceFile.Name, 3, 1)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "field"));

        DiagnosticResult diagnostic2 = AnalyzerVerifier
            .Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 7, 19)
            .WithLocation(sourceFile.Name, 5, 20)
            .WithLocation(sourceFile.Name, 3, 1)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "field"));

        DiagnosticResult diagnostic3 = AnalyzerVerifier
            .Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 12, 17)
            .WithLocation(sourceFile.Name, 5, 20)
            .WithLocation(sourceFile.Name, 3, 1)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "field"));

        await AnalyzerTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(diagnostic1)
            .WithExpectedDiagnostic(diagnostic2)
            .WithExpectedDiagnostic(diagnostic3)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task ShouldReportDiagnostic_WhenFieldHasUnderscorePrefix()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Properties.Samples/DeclarationCouldBeConvertedToProperty/UnderscoreNameField.cs");

        DiagnosticResult diagnostic1 = AnalyzerVerifier
            .Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 5, 29)
            .WithLocation(sourceFile.Name, 5, 29)
            .WithLocation(sourceFile.Name, 3, 1)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "_field"));

        DiagnosticResult diagnostic2 = AnalyzerVerifier
            .Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 7, 19)
            .WithLocation(sourceFile.Name, 5, 29)
            .WithLocation(sourceFile.Name, 3, 1)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "_field"));

        await AnalyzerTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(diagnostic1)
            .WithExpectedDiagnostic(diagnostic2)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task ShouldReportDiagnostic_WhenGetterAndSetterMethodsHasExpressionBody()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Properties.Samples/DeclarationCouldBeConvertedToProperty/ExpressionBodyMethodsField.cs");

        DiagnosticResult diagnostic1 = AnalyzerVerifier
            .Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 5, 20)
            .WithLocation(sourceFile.Name, 5, 20)
            .WithLocation(sourceFile.Name, 3, 1)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "field"));

        DiagnosticResult diagnostic2 = AnalyzerVerifier
            .Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 7, 19)
            .WithLocation(sourceFile.Name, 5, 20)
            .WithLocation(sourceFile.Name, 3, 1)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "field"));

        DiagnosticResult diagnostic3 = AnalyzerVerifier
            .Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 9, 29)
            .WithLocation(sourceFile.Name, 5, 20)
            .WithLocation(sourceFile.Name, 3, 1)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "field"));

        await AnalyzerTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(diagnostic1)
            .WithExpectedDiagnostic(diagnostic2)
            .WithExpectedDiagnostic(diagnostic3)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task ShouldReportDiagnostic_WhenPublicReadonlyFieldDeclared()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Properties.Samples/DeclarationCouldBeConvertedToProperty/ReadonlyPublicField.cs");

        DiagnosticResult diagnostic = AnalyzerVerifier
            .Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 5, 28)
            .WithLocation(sourceFile.Name, 5, 28)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "field"));

        await AnalyzerTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(diagnostic)
            .Build()
            .RunAsync();
    }
}