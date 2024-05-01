using System;
using System.Collections.Generic;

namespace SourceKit.Analyzers.Collections.Samples.ListForEachNotAllowed;

public class NoListForEachStatementExample
{
    public void DoSomething()
    {
        var a = new List<int> { 1, 2, 3 };
        
        foreach (var element in a)
        {
            Console.WriteLine(element);
        }
    }
}