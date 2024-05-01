namespace SourceKit.Analyzers.Properties.Samples.DeclarationCouldBeConvertedToProperty;

public class PrivateFieldWithoutMethods
{
    private string test;

    public string GetTest()
    {
        return "test";
    }
}