using SourceKit.Analyzers.Members;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;

namespace SourceKit.Analyzers.Tests.Members;

public class DeclarationCouldBeConvertedToPropertyTests : GeneratorTestBase<
    FieldCouldBeConverterToPropertyAnalyzer>
{
    protected override GeneratorTestBuilder ConfigureTest(GeneratorTestBuilder test)
    {
        return test.WithDisabledDiagnostics("CS0649", "CS8618", "CS0169");
    }

    [Fact]
    public async Task ShouldReportNoDiagnostic_WhenPublicPropertyDeclared()
    {
        var sourceFile = SourceFile.FromContent("""
        public class PublicProperty
        {
            public string Field { get; set; }
        }
        """);

        await GeneratorTest
            .WithSource(sourceFile)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task ShouldReportNoDiagnostic_WhenPrivateFieldWithNoGetterAndSetterDeclared()
    {
        var sourceFile = SourceFile.FromContent("""
        public class PrivateFieldWithoutMethods
        {
            private string test;
        
            public string GetTest()
            {
                return "test";
            }
        }
        """);

        await GeneratorTest
            .WithSource(sourceFile)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task ShouldReportDiagnostic_WhenPrivateFieldWithGetterAndSetterDeclared()
    {
        var sourceFile = SourceFile.FromContent("""
        public class OneField
        {
            private string _field = "10";
        
            public string GetField()
            {
                return _field;
            }
        
            public void SetField(string value)
            {
                _field = value;
            }
        }
        """);

        await GeneratorTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(
                Diagnostic(FieldCouldBeConverterToPropertyAnalyzer.Descriptor)
                    .WithLocation(sourceFile.FilePath, line: 5, column: 5)
                    .WithLocation(sourceFile.FilePath, line: 3, column: 20)
                    .WithArguments("_field")
            )
            .WithExpectedDiagnostic(
                Diagnostic(FieldCouldBeConverterToPropertyAnalyzer.Descriptor)
                    .WithLocation(sourceFile.FilePath, line: 10, column: 5)
                    .WithLocation(sourceFile.FilePath, line: 3, column: 20)
                    .WithArguments("_field")
            )
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task ShouldReportDiagnostic_WhenFieldHasUnderscorePrefix()
    {
        var sourceFile = SourceFile.FromContent("""
        public class UnderscoreNameField
        {
            private readonly string _field;
        
            public string GetField()
            {
                return _field;
            }
        }
        """);

        await GeneratorTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(
                Diagnostic(FieldCouldBeConverterToPropertyAnalyzer.Descriptor)
                    .WithLocation(sourceFile.FilePath, 5, 5)
                    .WithLocation(sourceFile.FilePath, 3, 29)
                    .WithArguments("_field")
            )
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task ShouldReportDiagnostic_WhenGetterAndSetterMethodsHasExpressionBody()
    {
        var sourceFile = SourceFile.FromContent("""
        public class ExpressionBodyMethodsField
        {
            private string field;
        
            public string GetField() => field;
        
            protected internal void SetField(string value) => field = value;
        }
        """);

        await GeneratorTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(
                Diagnostic(FieldCouldBeConverterToPropertyAnalyzer.Descriptor)
                    .WithLocation(sourceFile.FilePath, 5, 5)
                    .WithLocation(sourceFile.FilePath, 3, 20)
                    .WithArguments("field")
            )
            .WithExpectedDiagnostic(
                Diagnostic(FieldCouldBeConverterToPropertyAnalyzer.Descriptor)
                    .WithLocation(sourceFile.FilePath, 7, 5)
                    .WithLocation(sourceFile.FilePath, 3, 20)
                    .WithArguments("field")
            )
            .Build()
            .RunAsync();
    }
}
