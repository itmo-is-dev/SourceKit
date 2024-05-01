using Microsoft.CodeAnalysis.Testing;
using SourceKit.Analyzers.Enumerable.Analyzers;
using SourceKit.Analyzers.Enumerable.CodeFixes;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;
using Xunit;

namespace SourceKit.Analyzers.Enumerable.Tests;

public class RemoveTerminalOperationCodeFixProviderTests : CodeFixTestBase<
    DoNotEnumerateCollectionInLinqChainAnalyzer,
    RemoveTerminalOperationCodeFixProvider>
{
    [Fact]
    public async Task ShouldRemoveTerminalOperation_WhenLinqUsedAfterTerminationMethods()
    {
        var sourceFile = new SourceFile(
            Name: "Program.cs",
            Content: """
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
                    var list = GetItems().ToLookup(x => x, new Comparer<int>()).First();
                }
                static IEnumerable<int> GetItems()
                {
                    return Enumerable.Range(1, 10);
                }
            }
            """);

        var fixedSource = new SourceFile(
            Name: "Program.cs",
            Content: """
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
                    var list = GetItems().First();
                }
                static IEnumerable<int> GetItems()
                {
                    return Enumerable.Range(1, 10);
                }
            }
            """);

        DiagnosticResult diagnostic = AnalyzerVerifier
            .Diagnostic(DoNotEnumerateCollectionInLinqChainAnalyzer.Descriptor)
            .WithLocation(sourceFile.Name, 18, 31)
            .WithArguments("ToLookup");

        await CodeFixTest
            .WithSource(sourceFile)
            .WithFixedSource(fixedSource)
            .WithExpectedDiagnostic(diagnostic)
            .WithDisabledDiagnostic("CS1061")
            .Build()
            .RunAsync();
    }
}