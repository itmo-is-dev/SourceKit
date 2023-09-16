using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using SourceKit.Analyzers.Nullable.Analyzers;
using SourceKit.Analyzers.Nullable.CodeFixes;
using SourceKit.Tests.Tools;
using Xunit;
using AnalyzerVerifier = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<
    SourceKit.Analyzers.Nullable.Analyzers.NullForgivingOperatorNotAllowedAnalyzer>;
using CodeFixTest = Microsoft.CodeAnalysis.CSharp.Testing.CSharpCodeFixTest<
    SourceKit.Analyzers.Nullable.Analyzers.NullForgivingOperatorNotAllowedAnalyzer,
    SourceKit.Analyzers.Nullable.CodeFixes.PropagateNullableCorrectlyCodeFixProvider,
    Microsoft.CodeAnalysis.Testing.Verifiers.XUnitVerifier>;

namespace SourceKit.Tests.CodeFixProviders;

public class PropagateNullableCorrectlyCodeFixTests
{
    [Fact]
    public async Task PropagateNullableCorrectlyCodeFix_ShouldParseAllSequencesAndChangeTypeOfVariables()
    {
        var sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Sample/CodeFixProviders/PropagateNullCorrectly/JokeExampleWithVariableDeclarations.cs");

        var test = new CSharpCodeFixTest<
            NullForgivingOperatorNotAllowedAnalyzer,
            PropagateNullableCorrectlyCodeFixProvider,
            XUnitVerifier>
        {
            TestState =
            {
                Sources =
                {
                    sourceFile,
                },
                ExpectedDiagnostics =
                {
                    AnalyzerVerifier.Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                        .WithSpan(sourceFile.Name, 34, 60, 34, 89),
                    AnalyzerVerifier.Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                        .WithSpan(sourceFile.Name, 36, 35, 36, 68),
                    AnalyzerVerifier.Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                        .WithSpan(sourceFile.Name, 38, 47, 38, 97),
                    AnalyzerVerifier.Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                        .WithSpan(sourceFile.Name, 39, 45, 39, 95),
                    AnalyzerVerifier.Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                        .WithSpan(sourceFile.Name, 40, 46, 40, 75),
                    AnalyzerVerifier.Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                        .WithSpan(sourceFile.Name, 42, 55, 42, 105),
                    AnalyzerVerifier.Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                        .WithSpan(sourceFile.Name, 43, 52, 43, 85),
                    AnalyzerVerifier.Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                        .WithSpan(sourceFile.Name, 46, 13, 46, 63),
                    AnalyzerVerifier.Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                        .WithSpan(sourceFile.Name, 48, 13, 48, 67),
                }
            },
            FixedState =
            {
                Sources =
                {
                    new SourceFile(sourceFile.Name, JokeExampleWithVariableDeclarations)
                }
            }
        };

        await test.RunAsync();
    }

    [Fact]
    public async Task PropagateNullableCorrectlyCodeFix_ShouldChangeTypeOfFieldAndProperties()
    {
        var sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Sample/CodeFixProviders/PropagateNullCorrectly/JokeExampleWithFieldAndProperties.cs");

        var test =
            new CSharpCodeFixTest<NullForgivingOperatorNotAllowedAnalyzer, PropagateNullableCorrectlyCodeFixProvider,
                XUnitVerifier>()
            {
                TestState =
                {
                    Sources =
                    {
                        sourceFile,
                    },
                    ExpectedDiagnostics =
                    {
                        AnalyzerVerifier.Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                            .WithSpan(sourceFile.Name, 5, 63, 5, 79),
                        AnalyzerVerifier.Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                            .WithSpan(sourceFile.Name, 9, 16, 9, 41),
                        AnalyzerVerifier.Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                            .WithSpan(sourceFile.Name, 16, 20, 16, 45),
                        AnalyzerVerifier.Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                            .WithSpan(sourceFile.Name, 20, 32, 20, 57),
                    }
                },
                FixedState =
                {
                    Sources =
                    {
                        new SourceFile(sourceFile.Name, JokeExampleWithFieldAndProperties)
                    }
                }
            };

        await test.RunAsync();
    }

    const string JokeExampleWithVariableDeclarations = """
namespace SourceKit.Sample.CodeFixProviders;

public class JokeExampleWithVariableDeclarations
{
    public string? GetJoke(int number)
    {
        switch (number)
        {
            case 1:
                return "Lol";
            case 2:
                return "Kek";
            case 3:
                return "Trololo";
            default:
                return null;
        }
    }

    public static JokeExampleWithVariableDeclarations? GetJokeClass(int number)
    {
        if (number == 0)
        {
            return new JokeExampleWithVariableDeclarations();
        }
        else
        {
            return null;
        }
    }

    public static string GetUpperJoke(int instanceNumber, int jokeNumber)
    {
        JokeExampleWithVariableDeclarations? jokeInstance = GetJokeClass(instanceNumber);
        
        string? jokeFromInstance = jokeInstance.GetJoke(jokeNumber);
        
        var jokeFromSequenceNothingNullable = GetJokeClass(instanceNumber)?.GetJoke(jokeNumber);
        var jokeFromSequenceFirstNullable = GetJokeClass(instanceNumber)?.GetJoke(jokeNumber);
        var jokeFromSequenceSecondNullable = GetJokeClass(instanceNumber)?.GetJoke(jokeNumber);
        
        string? upperJokeFromSequenceNothingNullable = GetJokeClass(instanceNumber)?.GetJoke(jokeNumber)?.ToUpper();
        string? upperJokeFromSequenceFromInstance = jokeInstance.GetJoke(jokeNumber)?.ToUpper();
        
        var upperJokeFirstCringeCompilation =
            GetJokeClass(instanceNumber)?.GetJoke(jokeNumber)?.ToUpper().ToUpper().ToUpper();
        var upperJokeSecondCringeCompilation =
            jokeInstance.GetJoke(jokeNumber)?.ToUpper().ToUpper()?.ToUpper();
        
        return upperJokeFirstCringeCompilation;
    }
}
""";

    const string JokeExampleWithFieldAndProperties = """
namespace SourceKit.Sample.CodeFixProviders;

public class JokeExampleWithFieldAndProperties
{
    private JokeExampleWithFieldAndProperties? _jokeInstance = GetJokeClass(0);

    public string? FirstJoke
    {
        get => _jokeInstance.GetJoke(1);
    }

    public string? SecondJoke
    {
        get
        {
            return _jokeInstance.GetJoke(2);
        }
    }

    public string? ThirdJoke => _jokeInstance.GetJoke(3);
    
    public string? GetJoke(int number)
    {
        switch (number)
        {
            case 1:
                return "Lol";
            case 2:
                return "Kek";
            case 3:
                return "Trololo";
            default:
                return null;
        }
    }

    public static JokeExampleWithFieldAndProperties? GetJokeClass(int number)
    {
        if (number == 0)
        {
            return new JokeExampleWithFieldAndProperties();
        }
        else
        {
            return null;
        }
    }
}
""";
}