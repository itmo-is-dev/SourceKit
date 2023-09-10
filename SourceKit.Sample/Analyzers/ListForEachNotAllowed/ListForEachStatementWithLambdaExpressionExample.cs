using System.Collections.Generic;

namespace SourceKit.Sample.Analyzers.ListForEachNotAllowed;

public class ListForEachStatementWithLambdaExpressionExample
{
    public List<int> DoSomething()
    {
        var a = new List<int> { 10, 20, 30 };
        var result = new List<int>();
        
        a.ForEach(element => result.Add(element / 2));

        return result;
    }
}