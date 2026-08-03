using Microsoft.CodeAnalysis.Testing;
using SourceKit.Analyzers.Collections;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;

namespace SourceKit.Analyzers.Tests.Collections;

public class ListForEachNotAllowedTests : GeneratorTestBase<ListForEachNotAllowedAnalyzer>
{
    [Fact]
    public async Task ShouldReportNoDiagnostic_WhenNoListForEachExpressionsPresent()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Collections.Samples/ListForEachNotAllowed/NoListForEachStatementExample.cs");

        await GeneratorTest
            .WithSource(sourceFile)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task ShouldReportDiagnostic_WhenClassMethodContainsListForEachStatementWithLambdaBlock()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Collections.Samples/ListForEachNotAllowed/ListForEachStatementWithLambdaBlockExample.cs");

        DiagnosticResult diagnostic = Diagnostic(ListForEachNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.FilePath, 12, 9)
            .WithMessage(string.Format(ListForEachNotAllowedAnalyzer.Format));

        await GeneratorTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(diagnostic)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task ShouldReportDiagnostic_WhenClassMethodContainsListForEachStatementWithLambdaExpression()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Collections.Samples/ListForEachNotAllowed/ListForEachStatementWithLambdaExpressionExample.cs");

        DiagnosticResult diagnostic = Diagnostic(ListForEachNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.FilePath, 12, 9)
            .WithMessage(string.Format(ListForEachNotAllowedAnalyzer.Format));

        await GeneratorTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(diagnostic)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task ShouldReportDiagnostic_WhenClassMethodContainsListForEachStatementWithMemberAccessExpression()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Collections.Samples/ListForEachNotAllowed/ListForEachStatementWithMemberAccessExpressionExample.cs");

        DiagnosticResult diagnostic = Diagnostic(ListForEachNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.FilePath, 12, 9)
            .WithMessage(string.Format(ListForEachNotAllowedAnalyzer.Format));

        await GeneratorTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(diagnostic)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task ShouldReportDiagnostic_WhenClassMethodContainsListForEachStatementWithMethodLink()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Collections.Samples/ListForEachNotAllowed/ListForEachStatementWithMethodLinkExample.cs");

        DiagnosticResult diagnostic = Diagnostic(ListForEachNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.FilePath, 13, 9)
            .WithMessage(string.Format(ListForEachNotAllowedAnalyzer.Format));

        await GeneratorTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(diagnostic)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task ShouldReportDiagnostic_WhenClassMethodContainsSeveralListForEachExpressions()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Collections.Samples/ListForEachNotAllowed/ListForEachStatementComplexExample.cs");

        DiagnosticResult lambdaWithExpressionDiagnostic = Diagnostic(ListForEachNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.FilePath, 15, 9)
            .WithMessage(string.Format(ListForEachNotAllowedAnalyzer.Format));

        DiagnosticResult lambdaWithBlockDiagnostic = Diagnostic(ListForEachNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.FilePath, 16, 9)
            .WithMessage(string.Format(ListForEachNotAllowedAnalyzer.Format));

        DiagnosticResult memberAccessExpressionDiagnostic = Diagnostic(ListForEachNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.FilePath, 21, 9)
            .WithMessage(string.Format(ListForEachNotAllowedAnalyzer.Format));

        DiagnosticResult identifierNameDiagnostic = Diagnostic(ListForEachNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.FilePath, 22, 9)
            .WithMessage(string.Format(ListForEachNotAllowedAnalyzer.Format));

        await GeneratorTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(lambdaWithExpressionDiagnostic)
            .WithExpectedDiagnostic(lambdaWithBlockDiagnostic)
            .WithExpectedDiagnostic(memberAccessExpressionDiagnostic)
            .WithExpectedDiagnostic(identifierNameDiagnostic)
            .Build()
            .RunAsync();
    }
}
