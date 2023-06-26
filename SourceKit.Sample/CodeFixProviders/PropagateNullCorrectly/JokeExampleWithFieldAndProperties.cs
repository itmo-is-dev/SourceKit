namespace SourceKit.Sample.CodeFixProviders;

public class JokeExampleWithFieldAndProperties
{
    private JokeExampleWithFieldAndProperties _jokeInstance = GetJokeClass(0)!;

    public string FirstJoke
    {
        get => _jokeInstance.GetJoke(1)!;
    }

    public string SecondJoke
    {
        get
        {
            return _jokeInstance.GetJoke(2)!;
        }
    }

    public string ThirdJoke => _jokeInstance.GetJoke(3)!;
    
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