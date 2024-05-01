using Microsoft.CodeAnalysis.Testing;
using SourceKit.Analyzers.Properties.Analyzers;
using SourceKit.Analyzers.Properties.CodeFixes;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;
using Xunit;

namespace SourceKit.Analyzers.Properties.Tests;

public class ConvertDeclarationIntoPropertyCodeFixTests : CodeFixTestBase<
    DeclarationCouldBeConvertedToPropertyAnalyzer,
    ConvertDeclarationIntoPropertyCodeFixProvider>
{
    [Fact]
    public async Task ShouldGenerateProperty_WhenSingleFieldPubliclyDeclared()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Properties.Samples/DeclarationCouldBeConvertedToProperty/OnePublicField.cs");

        var fixedFile = new SourceFile(
            Name: "OnePublicField.cs",
            Content: """
            namespace SourceKit.Analyzers.Properties.Samples.DeclarationCouldBeConvertedToProperty;

            public class OnePublicField
            {
                public string Field { get; set; }
            }
            """);

        DiagnosticResult diagnostic = AnalyzerVerifier
            .Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 5, 19)
            .WithLocation(sourceFile.Name, 5, 19)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "field"));

        await CodeFixTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(diagnostic)
            .WithFixedSource(fixedFile)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task ShouldGenerateProperties_WhenMultipleFieldsPubliclyDeclared()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Properties.Samples/DeclarationCouldBeConvertedToProperty/ManyPublicFields.cs");

        var fixedFile = new SourceFile(
            Name: "ManyPublicFields.cs",
            Content: """
            using System.Collections.Generic;

            namespace SourceKit.Analyzers.Properties.Samples.DeclarationCouldBeConvertedToProperty;

            public class ManyPublicFields
            {
                public List<string> First { get; set; }
                public List<string> Second { get; set; }
            }
            """);

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

        await CodeFixTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(diagnostic1)
            .WithExpectedDiagnostic(diagnostic2)
            .WithFixedSource(fixedFile)
            .WithIterationNumber(2)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task ShouldGenerateProperty_WhenPrivateFieldWithGetterAndSetterDeclared()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Properties.Samples/DeclarationCouldBeConvertedToProperty/OneField.cs");

        var fixedFile = new SourceFile(
            Name: "OneField.cs",
            Content: """
            namespace SourceKit.Analyzers.Properties.Samples.DeclarationCouldBeConvertedToProperty;

            public class OneField
            {
                public string Field { get; set; } = "10";
            }
            """);

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

        await CodeFixTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(diagnostic1)
            .WithExpectedDiagnostic(diagnostic2)
            .WithExpectedDiagnostic(diagnostic3)
            .WithFixedSource(fixedFile)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task ShouldGenerateCorrectPropertyName_WhenFieldHasUnderscorePrefix()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Properties.Samples/DeclarationCouldBeConvertedToProperty/UnderscoreNameField.cs");

        var fixedFile = new SourceFile(
            Name: "UnderscoreNameField.cs",
            Content: """
            namespace SourceKit.Analyzers.Properties.Samples.DeclarationCouldBeConvertedToProperty;

            public class UnderscoreNameField
            {
                public string Field { get; }
            }
            """);

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

        await CodeFixTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(diagnostic1)
            .WithExpectedDiagnostic(diagnostic2)
            .WithFixedSource(fixedFile)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task ShouldGenerateCorrectProperty_WhenGetterAndSetterMethodsAreExpressionBodied()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Properties.Samples/DeclarationCouldBeConvertedToProperty/ExpressionBodyMethodsField.cs");

        var fixedFile = new SourceFile(
            Name: "ExpressionBodyMethodsField.cs",
            Content: """
            namespace SourceKit.Analyzers.Properties.Samples.DeclarationCouldBeConvertedToProperty;

            public class ExpressionBodyMethodsField
            {
                public string Field { get; protected internal set; }
            }
            """);

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

        await CodeFixTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(diagnostic1)
            .WithExpectedDiagnostic(diagnostic2)
            .WithExpectedDiagnostic(diagnostic3)
            .WithFixedSource(fixedFile)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task ShouldGenerateGetOnlyProperty_WhenPublicFieldIsReadonly()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Properties.Samples/DeclarationCouldBeConvertedToProperty/ReadonlyPublicField.cs");

        var fixedFile = new SourceFile(
            Name: "ReadonlyPublicField.cs",
            Content: """
            namespace SourceKit.Analyzers.Properties.Samples.DeclarationCouldBeConvertedToProperty;

            public class ReadonlyPublicField
            {
                public string Field { get; }
            }
            """);

        DiagnosticResult diagnostic = AnalyzerVerifier
            .Diagnostic(DeclarationCouldBeConvertedToPropertyAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 5, 28)
            .WithLocation(sourceFile.Name, 5, 28)
            .WithMessage(string.Format(DeclarationCouldBeConvertedToPropertyAnalyzer.Format, "field"));

        await CodeFixTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(diagnostic)
            .WithFixedSource(fixedFile)
            .Build()
            .RunAsync();
    }
}