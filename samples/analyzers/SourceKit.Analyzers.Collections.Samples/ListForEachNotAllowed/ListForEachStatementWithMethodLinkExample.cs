using System.Collections.Generic;

namespace SourceKit.Analyzers.Collections.Samples.ListForEachNotAllowed;

public class ListForEachStatementWithMethodLinkExample
{
    private readonly List<int> _list = new();

    public void DoSomething()
    {
        var a = new List<int> { 13, 21, 32 };

        a.ForEach(AddToPrivateList);
    }

    private void AddToPrivateList(int element)
    {
        _list.Add(element * 10);
    }
}