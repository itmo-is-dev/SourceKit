namespace SourceKit.Sample.Analyzers.DeclarationCouldBeConvertedToProperty;

public class UnderscoreNameField
{
    private readonly string _field;

    public string GetField()
    {
        return _field;
    }
}