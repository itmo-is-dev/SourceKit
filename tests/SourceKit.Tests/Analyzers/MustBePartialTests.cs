using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using SourceKit.Analyzers.MustBePartial.Analyzers;
using SourceKit.Analyzers.MustBePartial.Annotations;
using SourceKit.Tests.Tools;
using Xunit;
using AnalyzerVerifier = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<
    SourceKit.Analyzers.MustBePartial.Analyzers.TypeMustBePartialAnalyzer>;
using CodeFixTest = Microsoft.CodeAnalysis.CSharp.Testing.CSharpCodeFixTest<
    SourceKit.Analyzers.MustBePartial.Analyzers.TypeMustBePartialAnalyzer,
    SourceKit.Analyzers.MustBePartial.CodeFixes.MakeTypePartialCodeFixProvider,
    Microsoft.CodeAnalysis.Testing.Verifiers.XUnitVerifier>;

namespace SourceKit.Tests.Analyzers;

public class MustBePartialTests
{
    [Fact]
    public async Task DerivativesMustBePartial_ShouldReportDiagnostic_WhenTypeIsNotPartial()
    {
        var sourceFile = await SourceFile.LoadAsync("SourceKit.Sample/Analyzers/MustBePartial/NonPartialDerivative.cs");

        var diagnostic = AnalyzerVerifier.Diagnostic(TypeMustBePartialAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 3, 14)
            .WithArguments("NonPartialDerivative");

        var test = new CSharpAnalyzerTest<TypeMustBePartialAnalyzer, XUnitVerifier>
        {
            TestState =
            {
                Sources =
                {
                    sourceFile,
                    await SourceFile.LoadAsync("SourceKit.Sample/Analyzers/MustBePartial/IPartialBase.cs"),
                },
                AdditionalReferences = { typeof(DerivativesMustBePartialAttribute).Assembly },
            },
            ExpectedDiagnostics = { diagnostic },
        };

        await test.RunAsync();
    }

    [Fact]
    public async Task DerivativesMustBePartial_ShouldReportNoDiagnostic_WhenTypeIsPartial()
    {
        var test = new CSharpAnalyzerTest<TypeMustBePartialAnalyzer, XUnitVerifier>
        {
            TestState =
            {
                Sources =
                {
                    await SourceFile.LoadAsync("SourceKit.Sample/Analyzers/MustBePartial/PartialDerivative.cs"),
                    await SourceFile.LoadAsync("SourceKit.Sample/Analyzers/MustBePartial/IPartialBase.cs"),
                },
                AdditionalReferences = { typeof(DerivativesMustBePartialAttribute).Assembly },
            },
        };

        await test.RunAsync();
    }

    [Fact]
    public async Task MakeTypePartial_ShouldMakeTypePartial_WhenDiagnosticReported()
    {
        var interfaceSource = await SourceFile.LoadAsync("SourceKit.Sample/Analyzers/MustBePartial/IPartialBase.cs");
        var sourceFile = await SourceFile.LoadAsync("SourceKit.Sample/Analyzers/MustBePartial/NonPartialDerivative.cs");
        var fixedContent = sourceFile.Content.Replace("public class", "public partial class");

        var fixedSource = sourceFile with { Content = fixedContent };

        var diagnostic = AnalyzerVerifier.Diagnostic(TypeMustBePartialAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 3, 14)
            .WithArguments("NonPartialDerivative");

        var test = new CodeFixTest
        {
            TestState =
            {
                Sources = { sourceFile, interfaceSource, },
                AdditionalReferences = { typeof(DerivativesMustBePartialAttribute).Assembly },
            },
            FixedState =
            {
                Sources = { fixedSource, interfaceSource },
            },
            ExpectedDiagnostics = { diagnostic },
        };

        await test.RunAsync();
    }
}