using System.Collections.Generic;

namespace SourceKit.Sample.Analyzers.DeclarationCouldBeConvertedToProperty;

public class UltimateTest
{
    public List<List<string>> a, b;
    private List<string> c;
    private string d;

    protected List<string> GetC()
    {
        return c;
    }
    
    protected string GetD()
    {
        return d;
    }

    protected void SetD(string value)
    {
        d = value;
    }

    private int e;
    
    protected int GetE()
    {
        return e;
    }

    private void SetE(int value)
    {
        e = value;
    }

    public void Trash(int value)
    {
        return;
    }
}