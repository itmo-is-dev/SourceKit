namespace SourceKit.Analyzers.Properties.Samples.DeclarationCouldBeConvertedToProperty;

public class ExpressionBodyMethodsField
{
    private string field;

    public string GetField() => field;

    protected internal void SetField(string value) => field = value;
}