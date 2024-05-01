using System;
using System.Collections.Generic;

namespace SourceKit.Analyzers.Collections.Samples.ListForEachNotAllowed;

public class ListForEachStatementComplexExample
{
    private readonly List<int> _list = new();

    public List<int> DoSomething()
    {
        var a = new List<int> { 1, 2, 3 };
        var result = new List<int>();

        a.ForEach(element => result.Add(element * 123123));
        a.ForEach(element =>
        {
            result.Add(element * 123123);
            Console.WriteLine(element);
        });
        result.ForEach(Console.WriteLine);
        a.ForEach(AddToPrivateList);

        return result;
    }

    private void AddToPrivateList(int element)
    {
        _list.Add(element * 2);
    }
}