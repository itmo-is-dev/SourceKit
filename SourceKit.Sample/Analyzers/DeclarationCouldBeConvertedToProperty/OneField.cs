namespace SourceKit.Sample.Analyzers.DeclarationCouldBeConvertedToProperty;

public class OneField
{
    private string field;

    public string GetField()
    {
        return field;
    }

    public void SetField(string value)
    {
        field = value;
    }
}