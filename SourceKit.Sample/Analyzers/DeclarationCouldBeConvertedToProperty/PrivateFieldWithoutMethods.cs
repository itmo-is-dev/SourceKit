namespace SourceKit.Sample.Analyzers.DeclarationCouldBeConvertedToProperty;

public class PrivateFieldWithoutMethods
{
    private string test;

    public string GetTest()
    {
        return "test";
    }
}