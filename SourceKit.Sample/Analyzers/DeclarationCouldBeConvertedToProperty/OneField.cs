namespace SourceKit.Sample.Analyzers.DeclarationCouldBeConvertedToProperty;

public class OneField
{
    private string field = "10";

    public string GetField()
    {
        return field;
    }

    public void SetField(string value)
    {
        field = value;
    }
}