﻿namespace SourceKit.Sample.CodeFixProviders;

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
        JokeExampleWithVariableDeclarations jokeInstance = GetJokeClass(instanceNumber)!;
        
        string jokeFromInstance = jokeInstance.GetJoke(jokeNumber)!;
        
        var jokeFromSequenceNothingNullable = GetJokeClass(instanceNumber)!.GetJoke(jokeNumber)!;
        var jokeFromSequenceFirstNullable = GetJokeClass(instanceNumber)?.GetJoke(jokeNumber)!;
        var jokeFromSequenceSecondNullable = GetJokeClass(instanceNumber)!.GetJoke(jokeNumber);
        
        string upperJokeFromSequenceNothingNullable = GetJokeClass(instanceNumber)!.GetJoke(jokeNumber)!.ToUpper();
        string upperJokeFromSequenceFromInstance = jokeInstance.GetJoke(jokeNumber)!.ToUpper();
        
        var upperJokeFirstCringeCompilation =
            GetJokeClass(instanceNumber)!.GetJoke(jokeNumber)!.ToUpper().ToUpper().ToUpper();
        var upperJokeSecondCringeCompilation =
            jokeInstance.GetJoke(jokeNumber)!.ToUpper().ToUpper()!.ToUpper();
        
        return upperJokeFirstCringeCompilation;
    }
}