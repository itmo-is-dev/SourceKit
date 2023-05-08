using System.Collections.Generic;
using System.Linq;

namespace SourceKit.Sample;

public class Playground
{
    private static IEnumerable<int> A(IEnumerable<int> e)
        => e.Where(x => new[] { x }.Where(y => y > 1).Any());

    public static void Main()
    {
    }
}