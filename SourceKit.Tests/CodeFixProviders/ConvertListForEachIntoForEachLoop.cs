using SourceKit.Analyzers.Collections.Analyzers;
using SourceKit.Analyzers.Collections.CodeFixes;
using SourceKit.Tests.Tools;
using Xunit;

namespace SourceKit.Tests.CodeFixProviders;

using AnalyzerVerifier = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<ListForEachNotAllowedAnalyzer>;
using CodeFixTest =
    Microsoft.CodeAnalysis.CSharp.Testing.CSharpCodeFixTest<ListForEachNotAllowedAnalyzer,
        ConvertListForEachIntoForEachLoopCodeFixProvider, Microsoft.CodeAnalysis.Testing.Verifiers.XUnitVerifier>;

public class ConvertListForEachIntoForEachLoop
{
    [Fact]
    public async Task
        ConvertListForEachIntoForEachLoop_WhenDiagnosticReported_ShouldConvertListForEachWithLambdaBlockIntoForEachLoop()
    {
        var sourceFile = await SourceFile.LoadAsync(ListForEachStatementWithLambdaBlockPath);
        var fixedFile = new SourceFile(FixedListForEachStatementWithLambdaBlockExampleFilename,
                                       FixedListForEachStatementWithLambdaBlockExample);

        var diagnostic = AnalyzerVerifier.Diagnostic(ListForEachNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 12, 9)
            .WithMessage(string.Format(ListForEachNotAllowedAnalyzer.Format));

        var test = new CodeFixTest
        {
            TestState =
            {
                Sources = { sourceFile }
            },
            FixedState =
            {
                Sources = { fixedFile }
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
        ConvertListForEachIntoForEachLoop_WhenDiagnosticReported_ShouldConvertListForEachWithLambdaExpressionIntoForEachLoop()
    {
        var sourceFile = await SourceFile.LoadAsync(ListForEachStatementWithLambdaExpressionPath);
        var fixedFile = new SourceFile(FixedListForEachStatementWithLambdaExpressionExampleFilename,
                                       FixedListForEachStatementWithLambdaExpressionExample);

        var diagnostic = AnalyzerVerifier.Diagnostic(ListForEachNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 12, 9)
            .WithMessage(string.Format(ListForEachNotAllowedAnalyzer.Format));

        var test = new CodeFixTest
        {
            TestState =
            {
                Sources = { sourceFile }
            },
            FixedState =
            {
                Sources = { fixedFile }
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
        ConvertListForEachIntoForEachLoop_WhenDiagnosticReported_ShouldConvertListForEachWithMemberAccessExpressionIntoForEachLoop()
    {
        var sourceFile = await SourceFile.LoadAsync(ListForEachStatementWithMemberAccessExpressionPath);
        var fixedFile = new SourceFile(FixedListForEachStatementWithMemberAccessExpressionFilename,
                                       FixedListForEachStatementWithMemberAccessExpressionExample);

        var diagnostic = AnalyzerVerifier.Diagnostic(ListForEachNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 12, 9)
            .WithMessage(string.Format(ListForEachNotAllowedAnalyzer.Format));

        var test = new CodeFixTest
        {
            TestState =
            {
                Sources = { sourceFile }
            },
            FixedState =
            {
                Sources = { fixedFile }
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
        ConvertListForEachIntoForEachLoop_WhenDiagnosticReported_ShouldConvertListForEachWithMethodLinkIntoForEachLoop()
    {
        var sourceFile = await SourceFile.LoadAsync(ListForEachStatementWithMethodLinkPath);
        var fixedFile = new SourceFile(FixedListForEachStatementWithMethodLinkFilename,
                                       FixedListForEachStatementWithMethodLinkExample);

        var diagnostic = AnalyzerVerifier.Diagnostic(ListForEachNotAllowedAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 13, 9)
            .WithMessage(string.Format(ListForEachNotAllowedAnalyzer.Format));

        var test = new CodeFixTest
        {
            TestState =
            {
                Sources = { sourceFile }
            },
            FixedState =
            {
                Sources = { fixedFile }
            },
            ExpectedDiagnostics =
            {
                diagnostic
            }
        };

        await test.RunAsync();
    }

    private const string ListForEachStatementWithLambdaBlockPath =
        "SourceKit.Sample/Analyzers/ListForEachNotAllowed/ListForEachStatementWithLambdaBlockExample.cs";

    private const string ListForEachStatementWithLambdaExpressionPath =
        "SourceKit.Sample/Analyzers/ListForEachNotAllowed/ListForEachStatementWithLambdaExpressionExample.cs";

    private const string ListForEachStatementWithMemberAccessExpressionPath =
        "SourceKit.Sample/Analyzers/ListForEachNotAllowed/ListForEachStatementWithMemberAccessExpressionExample.cs";

    private const string ListForEachStatementWithMethodLinkPath =
        "SourceKit.Sample/Analyzers/ListForEachNotAllowed/ListForEachStatementWithMethodLinkExample.cs";

    private const string FixedListForEachStatementWithLambdaBlockExampleFilename =
        "ListForEachStatementWithLambdaBlockExample.cs";

    private const string FixedListForEachStatementWithLambdaExpressionExampleFilename =
        "ListForEachStatementWithLambdaExpressionExample.cs";

    private const string FixedListForEachStatementWithMemberAccessExpressionFilename =
        "ListForEachStatementWithMemberAccessExpressionExample.cs";

    private const string FixedListForEachStatementWithMethodLinkFilename =
        "ListForEachStatementWithMethodLinkExample.cs";

    private const string FixedListForEachStatementWithLambdaBlockExample = """
using System;
using System.Collections.Generic;

namespace SourceKit.Sample.Analyzers.ListForEachNotAllowed;

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
""";

    private const string FixedListForEachStatementWithLambdaExpressionExample = """
using System.Collections.Generic;

namespace SourceKit.Sample.Analyzers.ListForEachNotAllowed;

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
""";

    private const string FixedListForEachStatementWithMemberAccessExpressionExample = """
using System;
using System.Collections.Generic;

namespace SourceKit.Sample.Analyzers.ListForEachNotAllowed;

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
""";

    private const string FixedListForEachStatementWithMethodLinkExample = """
using System.Collections.Generic;

namespace SourceKit.Sample.Analyzers.ListForEachNotAllowed;

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
""";
}