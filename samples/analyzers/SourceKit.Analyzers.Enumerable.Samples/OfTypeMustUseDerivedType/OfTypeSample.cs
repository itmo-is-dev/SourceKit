using System.Collections.Generic;
using System.Linq;

namespace SourceKit.Analyzers.Enumerable.Samples.OfTypeMustUseDerivedType;

public class OfTypeSample
{
    public IEnumerable<int> A(IEnumerable<string> a)
    {
        return a.OfType<int>();
    }
}