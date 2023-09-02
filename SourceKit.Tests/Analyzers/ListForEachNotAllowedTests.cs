using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using SourceKit.Analyzers.Collections.Analyzers;
using SourceKit.Tests.Tools;
using Xunit;

namespace SourceKit.Tests.Analyzers;

using AnalyzerVerifier = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<ListForEachNotAllowedAnalyzer>;

public class ListForEachNotAllowedTests
{
    [Fact]
    public async Task ListForEachNotAllowed_NoListForEachExpressions_ShouldReportNoDiagnostic()
    {
        var test = new CSharpAnalyzerTest<ListForEachNotAllowedAnalyzer, XUnitVerifier>
        {
            TestState =
            {
                Sources =
                {
                    await SourceFile.LoadAsync(NoListForEachStatementPath)
                }
            }
        };

        await test.RunAsync();
    }

    [Fact]
    public async Task
        ListForEachNotAllowed_ClassMethodContainsListForEachStatementWithLambdaBlock_ShouldReportDiagnostic()
    {
        var sourceFile = await SourceFile.LoadAsync(ListForEachStatementWithLambdaBlockPath);

        var diagnostic = AnalyzerVerifier.Diagnostic(ListForEachNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 12, 9)
            .WithMessage(string.Format(ListForEachNotAllowedAnalyzer.Format));

        var test = new CSharpAnalyzerTest<ListForEachNotAllowedAnalyzer, XUnitVerifier>
        {
            TestState =
            {
                Sources =
                {
                    sourceFile
                }
            },
            ExpectedDiagnostics =
            {
                diagnostic
            }
        };

        await test.RunAsync();
    }

    [Fact]
    public async Task
        ListForEachNotAllowed_ClassMethodContainsListForEachStatementWithLambdaExpression_ShouldReportDiagnostic()
    {
        var sourceFile = await SourceFile.LoadAsync(ListForEachStatementWithLambdaExpressionPath);

        var diagnostic = AnalyzerVerifier.Diagnostic(ListForEachNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 12, 9)
            .WithMessage(string.Format(ListForEachNotAllowedAnalyzer.Format));

        var test = new CSharpAnalyzerTest<ListForEachNotAllowedAnalyzer, XUnitVerifier>
        {
            TestState =
            {
                Sources =
                {
                    sourceFile
                }
            },
            ExpectedDiagnostics =
            {
                diagnostic
            }
        };

        await test.RunAsync();
    }

    [Fact]
    public async Task
        ListForEachNotAllowed_ClassMethodContainsListForEachStatementWithMemberAccessExpression_ShouldReportDiagnostic()
    {
        var sourceFile = await SourceFile.LoadAsync(ListForEachStatementWithMemberAccessExpressionPath);

        var diagnostic = AnalyzerVerifier.Diagnostic(ListForEachNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 12, 9)
            .WithMessage(string.Format(ListForEachNotAllowedAnalyzer.Format));

        var test = new CSharpAnalyzerTest<ListForEachNotAllowedAnalyzer, XUnitVerifier>
        {
            TestState =
            {
                Sources =
                {
                    sourceFile
                }
            },
            ExpectedDiagnostics =
            {
                diagnostic
            }
        };

        await test.RunAsync();
    }

    [Fact]
    public async Task
        ListForEachNotAllowed_ClassMethodContainsListForEachStatementWithMethodLink_ShouldReportDiagnostic()
    {
        var sourceFile = await SourceFile.LoadAsync(ListForEachStatementWithMethodLinkPath);

        var diagnostic = AnalyzerVerifier.Diagnostic(ListForEachNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 13, 9)
            .WithMessage(string.Format(ListForEachNotAllowedAnalyzer.Format));

        var test = new CSharpAnalyzerTest<ListForEachNotAllowedAnalyzer, XUnitVerifier>
        {
            TestState =
            {
                Sources =
                {
                    sourceFile
                }
            },
            ExpectedDiagnostics =
            {
                diagnostic
            }
        };

        await test.RunAsync();
    }

    [Fact]
    public async Task ListForEachNotAllowed_ClassMethodContainsSeveralListForEachExpressions_ShouldReportDiagnostic()
    {
        var sourceFile = await SourceFile.LoadAsync(SeveralListForEachStatementsPath);

        var lambdaWithExpressionDiagnostic = AnalyzerVerifier.Diagnostic(ListForEachNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 15, 9)
            .WithMessage(string.Format(ListForEachNotAllowedAnalyzer.Format));
        var lambdaWithBlockDiagnostic = AnalyzerVerifier.Diagnostic(ListForEachNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 16, 9)
            .WithMessage(string.Format(ListForEachNotAllowedAnalyzer.Format));
        var memberAccessExpressionDiagnostic = AnalyzerVerifier.Diagnostic(ListForEachNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 21, 9)
            .WithMessage(string.Format(ListForEachNotAllowedAnalyzer.Format));
        var identifierNameDiagnostic = AnalyzerVerifier.Diagnostic(ListForEachNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 22, 9)
            .WithMessage(string.Format(ListForEachNotAllowedAnalyzer.Format));

        var test = new CSharpAnalyzerTest<ListForEachNotAllowedAnalyzer, XUnitVerifier>
        {
            TestState =
            {
                Sources =
                {
                    sourceFile
                }
            },
            ExpectedDiagnostics =
            {
                lambdaWithExpressionDiagnostic,
                lambdaWithBlockDiagnostic,
                memberAccessExpressionDiagnostic,
                identifierNameDiagnostic
            }
        };

        await test.RunAsync();
    }

    private const string NoListForEachStatementPath =
        "SourceKit.Sample/Analyzers/ListForEachNotAllowed/NoListForEachStatementExample.cs";

    private const string ListForEachStatementWithLambdaBlockPath =
        "SourceKit.Sample/Analyzers/ListForEachNotAllowed/ListForEachStatementWithLambdaBlockExample.cs";

    private const string ListForEachStatementWithLambdaExpressionPath =
        "SourceKit.Sample/Analyzers/ListForEachNotAllowed/ListForEachStatementWithLambdaExpressionExample.cs";

    private const string ListForEachStatementWithMemberAccessExpressionPath =
        "SourceKit.Sample/Analyzers/ListForEachNotAllowed/ListForEachStatementWithMemberAccessExpressionExample.cs";

    private const string ListForEachStatementWithMethodLinkPath =
        "SourceKit.Sample/Analyzers/ListForEachNotAllowed/ListForEachStatementWithMethodLinkExample.cs";

    private const string SeveralListForEachStatementsPath =
        "SourceKit.Sample/Analyzers/ListForEachNotAllowed/ListForEachStatementComplexExample.cs";
}