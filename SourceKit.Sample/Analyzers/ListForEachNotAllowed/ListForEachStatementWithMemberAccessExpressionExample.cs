﻿using System;
using System.Collections.Generic;

namespace SourceKit.Sample.Analyzers.ListForEachNotAllowed;

public class ListForEachStatementWithMemberAccessExpressionExample
{
    public void DoSomething()
    {
        var a = new List<int> { 1, 2, 3 };
        
        a.ForEach(Console.WriteLine);
    }
}