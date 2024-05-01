using Microsoft.CodeAnalysis.Testing;
using SourceKit.Analyzers.Collections.Analyzers;
using SourceKit.Analyzers.Collections.CodeFixes;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;
using Xunit;

namespace SourceKit.Analyzers.Collections.Tests;

public class ConvertListForEachIntoForEachLoop : CodeFixTestBase<
    ListForEachNotAllowedAnalyzer,
    ConvertListForEachIntoForEachLoopCodeFixProvider>
{
    [Fact]
    public async Task ShouldConvertListForEachWithLambdaBlockIntoForEachLoop_WhenDiagnosticReported()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Collections.Samples/ListForEachNotAllowed/ListForEachStatementWithLambdaBlockExample.cs");

        var fixedFile = new SourceFile(
            Name: "ListForEachStatementWithLambdaBlockExample.cs",
            Content: """
            using System;
            using System.Collections.Generic;

            namespace SourceKit.Analyzers.Collections.Samples.ListForEachNotAllowed;

            public class ListForEachStatementWithLambdaBlockExample
            {
                public void DoSomething()
                {
                    var a = new List<int> { 1, 2, 3 };

                    foreach (var element in a)
                    {
                        Console.WriteLine(element);
                        Console.WriteLine(element / 2);
                    }
                }
            }
            """);

        DiagnosticResult diagnostic = AnalyzerVerifier
            .Diagnostic(ListForEachNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 12, 9)
            .WithMessage(string.Format(ListForEachNotAllowedAnalyzer.Format));

        await CodeFixTest
            .WithSource(sourceFile)
            .WithFixedSource(fixedFile)
            .WithExpectedDiagnostic(diagnostic)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task ShouldConvertListForEachWithLambdaExpressionIntoForEachLoop_WhenDiagnosticReported()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Collections.Samples/ListForEachNotAllowed/ListForEachStatementWithLambdaExpressionExample.cs");

        var fixedFile = new SourceFile(
            Name: "ListForEachStatementWithLambdaExpressionExample.cs",
            Content: """
            using System.Collections.Generic;

            namespace SourceKit.Analyzers.Collections.Samples.ListForEachNotAllowed;

            public class ListForEachStatementWithLambdaExpressionExample
            {
                public List<int> DoSomething()
                {
                    var a = new List<int> { 10, 20, 30 };
                    var result = new List<int>();

                    foreach (var element in a)
                    {
                        result.Add(element / 2);
                    }

                    return result;
                }
            }
            """);

        DiagnosticResult diagnostic = AnalyzerVerifier
            .Diagnostic(ListForEachNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 12, 9)
            .WithMessage(string.Format(ListForEachNotAllowedAnalyzer.Format));

        await CodeFixTest
            .WithSource(sourceFile)
            .WithFixedSource(fixedFile)
            .WithExpectedDiagnostic(diagnostic)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task ShouldConvertListForEachWithMemberAccessExpressionIntoForEachLoop_WhenDiagnosticReported()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Collections.Samples/ListForEachNotAllowed/ListForEachStatementWithMemberAccessExpressionExample.cs");

        var fixedFile = new SourceFile(
            Name: "ListForEachStatementWithMemberAccessExpressionExample.cs",
            Content: """
            using System;
            using System.Collections.Generic;

            namespace SourceKit.Analyzers.Collections.Samples.ListForEachNotAllowed;

            public class ListForEachStatementWithMemberAccessExpressionExample
            {
                public void DoSomething()
                {
                    var a = new List<int> { 1, 2, 3 };

                    foreach (var element in a)
                    {
                        Console.WriteLine(element);
                    }
                }
            }
            """);

        DiagnosticResult diagnostic = AnalyzerVerifier
            .Diagnostic(ListForEachNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 12, 9)
            .WithMessage(string.Format(ListForEachNotAllowedAnalyzer.Format));

        await CodeFixTest
            .WithSource(sourceFile)
            .WithFixedSource(fixedFile)
            .WithExpectedDiagnostic(diagnostic)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task ShouldConvertListForEachWithMethodLinkIntoForEachLoop_WhenDiagnosticReported()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Collections.Samples/ListForEachNotAllowed/ListForEachStatementWithMethodLinkExample.cs");

        var fixedFile = new SourceFile(
            Name: "ListForEachStatementWithMethodLinkExample.cs",
            Content: """
            using System.Collections.Generic;

            namespace SourceKit.Analyzers.Collections.Samples.ListForEachNotAllowed;

            public class ListForEachStatementWithMethodLinkExample
            {
                private readonly List<int> _list = new();

                public void DoSomething()
                {
                    var a = new List<int> { 13, 21, 32 };

                    foreach (var element in a)
                    {
                        AddToPrivateList(element);
                    }
                }

                private void AddToPrivateList(int element)
                {
                    _list.Add(element * 10);
                }
            }
            """);

        DiagnosticResult diagnostic = AnalyzerVerifier
            .Diagnostic(ListForEachNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 13, 9)
            .WithMessage(string.Format(ListForEachNotAllowedAnalyzer.Format));

        await CodeFixTest
            .WithSource(sourceFile)
            .WithFixedSource(fixedFile)
            .WithExpectedDiagnostic(diagnostic)
            .Build()
            .RunAsync();
    }
}