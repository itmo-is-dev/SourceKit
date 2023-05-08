using System;
using System.Collections.Generic;
using System.Linq;
using SourceKit.Sample.Models;

namespace SourceKit.Sample;

public class Class
{
    public static void Run()
    {
        var point = new Point(0, 1);

        var point2 = new Point(0, 0)
        {
            Y = 2,
        };
    }

    public static int LocalBinaryReference(int multiplier)
    {
        var a = 20 * multiplier;
        return a + 1;
    }

    public static int MethodExtensionMethodCall(IEnumerable<int> enumerable)
    {
        return enumerable.ToArray().Sum() + LocalBinaryReference(10);
    }

    public static string CustomAggregate(IEnumerable<int> values, int b)
    {
        IEnumerable<string> projected = values
            .Select(x =>
            {
                x *= 2 + b;
                return new[] { x }.Select(y => Math.Pow(y, y)).Single();
            })
            .Select(x => x + 45)
            .OrderBy(x => x / 2)
            .GroupBy(x => x % 2)
            .Select(x => $"{x.Key}: {x.Count()} {x.Max():,2#.00}");

        return string.Join(", ", projected);
    }
}