using System;
using System.Collections.Generic;
using System.Linq;
using SourceKit.Sample.Generators;
using SourceKit.Sample.Models;

namespace SourceKit.Sample;

public class Playground
{
    private static IEnumerable<int> A(IEnumerable<int> e)
        => e.Where(x => new[] { x }.Where(y => y > 1).Any());

    public static void X()
    {
        var temp = new List<string> { "asd" };

        var g = temp.FirstOrDefault(x => x == "asd")!.ToString();

        /*SomeQuery.Build(x => x.WithOrderById(null));
        ArrayQuery.Build(x => x.WithId(Guid.NewGuid()));
        CustomModelQuery.Build(x => x.WithPoint(new Point()));*/
    }
}