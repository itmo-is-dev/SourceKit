using Microsoft.CodeAnalysis.Testing;
using SourceKit.Analyzers.Collections.Analyzers;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;
using Xunit;

namespace SourceKit.Analyzers.Collections.Tests;

public class ListForEachNotAllowedTests : AnalyzerTestBase<ListForEachNotAllowedAnalyzer>
{
    [Fact]
    public async Task ShouldReportNoDiagnostic_WhenNoListForEachExpressionsPresent()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Collections.Samples/ListForEachNotAllowed/NoListForEachStatementExample.cs");

        await AnalyzerTest
            .WithSource(sourceFile)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task ShouldReportDiagnostic_WhenClassMethodContainsListForEachStatementWithLambdaBlock()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Collections.Samples/ListForEachNotAllowed/ListForEachStatementWithLambdaBlockExample.cs");

        DiagnosticResult diagnostic = AnalyzerVerifier
            .Diagnostic(ListForEachNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 12, 9)
            .WithMessage(string.Format(ListForEachNotAllowedAnalyzer.Format));

        await AnalyzerTest
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

        DiagnosticResult diagnostic = AnalyzerVerifier
            .Diagnostic(ListForEachNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 12, 9)
            .WithMessage(string.Format(ListForEachNotAllowedAnalyzer.Format));

        await AnalyzerTest
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

        DiagnosticResult diagnostic = AnalyzerVerifier
            .Diagnostic(ListForEachNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 12, 9)
            .WithMessage(string.Format(ListForEachNotAllowedAnalyzer.Format));

        await AnalyzerTest
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

        DiagnosticResult diagnostic = AnalyzerVerifier
            .Diagnostic(ListForEachNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 13, 9)
            .WithMessage(string.Format(ListForEachNotAllowedAnalyzer.Format));

        await AnalyzerTest
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

        DiagnosticResult lambdaWithExpressionDiagnostic = AnalyzerVerifier
            .Diagnostic(ListForEachNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 15, 9)
            .WithMessage(string.Format(ListForEachNotAllowedAnalyzer.Format));

        DiagnosticResult lambdaWithBlockDiagnostic = AnalyzerVerifier
            .Diagnostic(ListForEachNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 16, 9)
            .WithMessage(string.Format(ListForEachNotAllowedAnalyzer.Format));

        DiagnosticResult memberAccessExpressionDiagnostic = AnalyzerVerifier
            .Diagnostic(ListForEachNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 21, 9)
            .WithMessage(string.Format(ListForEachNotAllowedAnalyzer.Format));

        DiagnosticResult identifierNameDiagnostic = AnalyzerVerifier
            .Diagnostic(ListForEachNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 22, 9)
            .WithMessage(string.Format(ListForEachNotAllowedAnalyzer.Format));

        await AnalyzerTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(lambdaWithExpressionDiagnostic)
            .WithExpectedDiagnostic(lambdaWithBlockDiagnostic)
            .WithExpectedDiagnostic(memberAccessExpressionDiagnostic)
            .WithExpectedDiagnostic(identifierNameDiagnostic)
            .Build()
            .RunAsync();
    }
}