using SourceKit.Analyzers.Enumerable.Analyzers;
using Xunit;
using CSharpAnalyzerTest = Microsoft.CodeAnalysis.CSharp.Testing.CSharpAnalyzerTest<
    SourceKit.Analyzers.Enumerable.Analyzers.DoNotEnumerateCollectionInLinqChain,
    Microsoft.CodeAnalysis.Testing.Verifiers.XUnitVerifier>;
using AnalyzerVerifier = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<
    SourceKit.Analyzers.Enumerable.Analyzers.DoNotEnumerateCollectionInLinqChain>;
using CodeFixTest = Microsoft.CodeAnalysis.CSharp.Testing.CSharpCodeFixTest<
    SourceKit.Analyzers.Enumerable.Analyzers.DoNotEnumerateCollectionInLinqChain,
    SourceKit.Analyzers.Enumerable.CodeFixes.RemoveTerminalOperationCodeFixProvider,
    Microsoft.CodeAnalysis.Testing.Verifiers.XUnitVerifier>;

namespace SourceKit.Tests.Analyzers;

public class DoNotEnumerateCollectionInLinqChainTests
{
    [Fact]
    public async Task NoIssues_WhenNoLinqMethodsUsedAfterConversionMethods()
    {
        const string testCode = 
            """
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
            """;
        var test = new CSharpAnalyzerTest
        {
            TestCode = testCode,
        };
        await test.RunAsync();
    }

    [Fact]
    public async Task AnalyzerReportsIssue_WhenLinqMethodUsedAfterToList()
    {
        const string testCode = 
            """
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
            """;

        var expectedDiagnostic = AnalyzerVerifier.Diagnostic(DoNotEnumerateCollectionInLinqChain.Descriptor)
            .WithLocation(8, 31).WithArguments("ToList");

        var test = new CSharpAnalyzerTest
        {
            TestCode = testCode,
            ExpectedDiagnostics = { expectedDiagnostic }
        };

        await test.RunAsync();
    }
    
    [Fact]
    public async Task AnalyzerReportsIssue_WhenLinqMethodUsedAfterToLookup()
    {
        const string testCode = 
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
            """;

        var expectedDiagnostic = AnalyzerVerifier.Diagnostic(DoNotEnumerateCollectionInLinqChain.Descriptor)
            .WithLocation(18, 31).WithArguments("ToLookup");

        var test = new CSharpAnalyzerTest
        {
            TestCode = testCode,
            ExpectedDiagnostics = { expectedDiagnostic }
        };

        await test.RunAsync();
    }
    
    [Fact]
    public async Task AnalyzerReportsIssue_WhenLinqMethodUsedAfterToArray()
    {
        const string testCode =
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
            """;

        var expectedDiagnostic = AnalyzerVerifier.Diagnostic(DoNotEnumerateCollectionInLinqChain.Descriptor)
            .WithLocation(8, 31).WithArguments("ToArray");

        var test = new CSharpAnalyzerTest
        {
            TestCode = testCode,
            ExpectedDiagnostics = { expectedDiagnostic }
        };

        await test.RunAsync();
    }

    [Fact]
    public async Task CodeFixerFix_WhenLinqUsedAfterTerminationMethods()
    {
        string fix = 
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
                    var list = GetItems().First();
                }
                static IEnumerable<int> GetItems()
                {
                    return Enumerable.Range(1, 10);
                }
            }
            """;
        var testCode = 
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
                    var list = GetItems().ToLookup(x => x, new Comparer<int>()).First();
                }
                static IEnumerable<int> GetItems()
                {
                    return Enumerable.Range(1, 10);
                }
            }
            """;

        var test = new CodeFixTest
        {
            TestCode = testCode,
            FixedCode = fix,
            ExpectedDiagnostics =
            {
                AnalyzerVerifier.Diagnostic(DoNotEnumerateCollectionInLinqChain.Descriptor)
                    .WithLocation(18, 31).WithArguments("ToLookup"),
            },
            DisabledDiagnostics =
            {
                "CS1061", 
            }
        };
        await test.RunAsync();
    }
}