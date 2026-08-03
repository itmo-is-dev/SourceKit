using Microsoft.CodeAnalysis.Testing;
using SourceKit.Analyzers.Annotations.MustBePartial;
using SourceKit.Analyzers.MustBePartial;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;

namespace SourceKit.Analyzers.Tests.MustBePartial;

public class MakeTypePartialCodeFixTests : CodeFixTestBase<TypeMustBePartialAnalyzer, MakeTypePartialCodeFixProvider>
{
    [Fact]
    public async Task ShouldMakeTypePartial_WhenDiagnosticReported()
    {
        SourceFile interfaceSource = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.MustBePartial.Samples/IPartialBase.cs");

        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.MustBePartial.Samples/NonPartialDerivative.cs");

        string fixedContent = sourceFile.Content.Replace("public class", "public partial class");

        SourceFile fixedSource = sourceFile with { Content = fixedContent };

        DiagnosticResult diagnostic = Diagnostic(TypeMustBePartialAnalyzer.Descriptor)
            .WithLocation(sourceFile.FilePath, 3, 14)
            .WithArguments("NonPartialDerivative");

        await CodeFixTest
            .WithSource(sourceFile)
            .WithSource(interfaceSource)
            .WithFixedSource(fixedSource)
            .WithFixedSource(interfaceSource)
            .WithExpectedDiagnostic(diagnostic)
            .WithAdditionalReference(typeof(DerivativesMustBePartialAttribute).Assembly)
            .Build()
            .RunAsync();
    }
}