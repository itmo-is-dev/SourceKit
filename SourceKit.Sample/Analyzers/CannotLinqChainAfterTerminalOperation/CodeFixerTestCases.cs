using System.Collections.Generic;
using System.Linq;

class Program
{
    private class Comparer<T> : IEqualityComparer<T>
    {
        bool IEqualityComparer<T>.Equals(T x, T y)
        {
            return x.Equals(y);
        }

        int IEqualityComparer<T>.GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }
    static void Main(string[] args)
    {
        var list = GetItems().ToLookup(x => x, new Comparer<int>()).First();
        var list1 = GetItems().ToArray().Where(x => x > 4);
        var list2 = GetItems().ToList().Where(x => x > 4);
        var list3 = GetItems().ToDictionary(x => x).First();
    }

    static IEnumerable<int> GetItems()
    {
        return Enumerable.Range(1, 10);
    }
}