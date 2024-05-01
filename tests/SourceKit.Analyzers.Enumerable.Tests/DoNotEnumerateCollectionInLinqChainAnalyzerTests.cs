using Microsoft.CodeAnalysis.Testing;
using SourceKit.Analyzers.Enumerable.Analyzers;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;
using Xunit;

namespace SourceKit.Analyzers.Enumerable.Tests;

public class DoNotEnumerateCollectionInLinqChainAnalyzerTests : AnalyzerTestBase<
    DoNotEnumerateCollectionInLinqChainAnalyzer>
{
    [Fact]
    public async Task ShouldNotReportDiagnostics_WhenNoLinqMethodsUsedAfterTerminalMethods()
    {
        var sourceFile = new SourceFile(
            Name: "Program.cs",
            Content: """
            using System;
            using System.Collections.Generic;
            using System.Linq;
            
            class Program
            {
                static void Main(string[] args)
                {
                    var list = GetItems().Select(x => new List<int>{x}).Select(x => x.First()).ToList();
                }
                static IEnumerable<int> GetItems()
                {
                    return Enumerable.Range(1, 10);
                }
            }
            """);

        await AnalyzerTest
            .WithSource(sourceFile)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task ShouldReportDiagnostic_WhenLinqMethodUsedAfterToList()
    {
        var sourceFile = new SourceFile(
            Name: "Program.cs",
            Content: """
            using System;
            using System.Collections.Generic;
            using System.Linq;
            class Program
            {
                static void Main(string[] args)
                {
                    var list = GetItems().ToList().Where(x => x > 5);
                }
                static IEnumerable<int> GetItems()
                {
                    return Enumerable.Range(1, 10);
                }
            }
            """);

        DiagnosticResult diagnostic = AnalyzerVerifier
            .Diagnostic(DoNotEnumerateCollectionInLinqChainAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 8, 31)
            .WithArguments("ToList");

        await AnalyzerTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(diagnostic)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task ShouldReportDiagnostic_WhenLinqMethodUsedAfterToLookup()
    {
        var sourceFile = new SourceFile(
            Name: "Program.cs",
            Content:
            """
            using System.Collections.Generic;
            using System.Linq;
            class Program
            {
                private class Comparer<T> : IEqualityComparer<T>
                {
                    bool IEqualityComparer<T>.Equals(T x, T y)
                    {
                        return x.Equals(y);
                    }
                    int IEqualityComparer<T>.GetHashCode(T obj)
                    {
                        return obj.GetHashCode();
                    }
                }
                static void Main(string[] args)
                {
                    var list = GetItems().ToLookup(x => x, new Comparer<int>()).Where(x => x.Key > 4);
                }
                static IEnumerable<int> GetItems()
                {
                    return Enumerable.Range(1, 10);
                }
            }
            """);

        DiagnosticResult diagnostic = AnalyzerVerifier
            .Diagnostic(DoNotEnumerateCollectionInLinqChainAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 18, 31).WithArguments("ToLookup");

        await AnalyzerTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(diagnostic)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task ShouldReportDiagnostic_WhenLinqMethodUsedAfterToArray()
    {
        var sourceFile = new SourceFile(
            Name: "Program.cs",
            Content:
            """
            using System;
            using System.Collections.Generic;
            using System.Linq;
            class Program
            {
                static void Main(string[] args)
                {
                    var list = GetItems().ToArray().Where(x => x > 5);
                }
                static IEnumerable<int> GetItems()
                {
                    return Enumerable.Range(1, 10);
                }
            }
            """);

        DiagnosticResult diagnostic = AnalyzerVerifier
            .Diagnostic(DoNotEnumerateCollectionInLinqChainAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 8, 31).WithArguments("ToArray");

        await AnalyzerTest
            .WithSource(sourceFile)
            .WithExpectedDiagnostic(diagnostic)
            .Build()
            .RunAsync();
    }
}