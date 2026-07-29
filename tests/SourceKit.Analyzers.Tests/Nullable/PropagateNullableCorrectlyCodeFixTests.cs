using SourceKit.Analyzers.Nullable;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;

namespace SourceKit.Analyzers.Tests.Nullable;

public class PropagateNullableCorrectlyCodeFixTests : CodeFixTestBase<
    NullForgivingOperatorNotAllowedAnalyzer,
    PropagateNullableCorrectlyCodeFixProvider>
{
    [Fact]
    public async Task ShouldParseAllSequencesAndChangeTypeOfVariables()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Nullable.Samples/PropagateNullCorrectly/JokeExampleWithVariableDeclarations.cs");

        SourceFile fixedSource = sourceFile with
        {
            Content = """
            namespace SourceKit.Analyzers.Nullable.Samples.PropagateNullCorrectly;

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
            """,
        };

        await CodeFixTest
            .WithSource(sourceFile)
            .WithFixedSource(fixedSource)
            .WithExpectedDiagnostic(
                Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.Name, 34, 60, 34, 89))
            .WithExpectedDiagnostic(
                Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.Name, 36, 35, 36, 68))
            .WithExpectedDiagnostic(
                Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.Name, 38, 47, 38, 97))
            .WithExpectedDiagnostic(
                Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.Name, 39, 45, 39, 95))
            .WithExpectedDiagnostic(
                Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.Name, 40, 46, 40, 75))
            .WithExpectedDiagnostic(
                Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.Name, 42, 55, 42, 105))
            .WithExpectedDiagnostic(
                Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.Name, 43, 52, 43, 85))
            .WithExpectedDiagnostic(
                Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.Name, 46, 13, 46, 63))
            .WithExpectedDiagnostic(
                Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.Name, 48, 13, 48, 67))
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task PropagateNullableCorrectlyCodeFix_ShouldChangeTypeOfFieldAndProperties()
    {
        SourceFile sourceFile = await SourceFile.LoadAsync(
            "SourceKit.Analyzers.Nullable.Samples/PropagateNullCorrectly/JokeExampleWithFieldAndProperties.cs");

        SourceFile fixedSource = sourceFile with
        {
            Content = """
            namespace SourceKit.Analyzers.Nullable.Samples.PropagateNullCorrectly;

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
            """,
        };

        await CodeFixTest
            .WithSource(sourceFile)
            .WithFixedSource(fixedSource)
            .WithExpectedDiagnostic(
                Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.Name, 5, 63, 5, 79))
            .WithExpectedDiagnostic(
                Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.Name, 9, 16, 9, 41))
            .WithExpectedDiagnostic(
                Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.Name, 16, 20, 16, 45))
            .WithExpectedDiagnostic(
                Diagnostic(NullForgivingOperatorNotAllowedAnalyzer.Descriptor)
                    .WithSpan(sourceFile.Name, 20, 32, 20, 57))
            .Build()
            .RunAsync();
    }
}
