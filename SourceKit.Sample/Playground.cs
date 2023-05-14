using System;
using System.Collections.Generic;
using System.Linq;
using SourceKit.Sample.Generators;

namespace SourceKit.Sample;

public class Playground
{
    private static IEnumerable<int> A(IEnumerable<int> e)
        => e.Where(x => new[] { x }.Where(y => y > 1).Any());

    public static void X()
    {
        SomeQuery.Build(x => x.WithOrderById(null));
    }
}