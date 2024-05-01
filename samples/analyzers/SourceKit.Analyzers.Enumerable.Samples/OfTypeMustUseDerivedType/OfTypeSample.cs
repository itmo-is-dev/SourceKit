using System.Collections.Generic;
using System.Linq;

namespace SourceKit.Sample.Analyzers.Enumerable;

public class OfTypeSample
{
    public IEnumerable<int> A(IEnumerable<string> a)
    {
        return a.OfType<int>();
    }
}