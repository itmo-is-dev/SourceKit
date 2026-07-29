namespace SourceKit.Tools;

public static class EqualityComparerFactory
{
    public static IEqualityComparer<T> Create<T>(Func<T, T, bool> comparer, Func<T, int>? hashFactory = null)
    {
        return new GenericEqualityComparer<T>(comparer, hashFactory);
    }

    public static IEqualityComparer<T1> CreateFromSelector<T1, T2>(Func<T1, T2> selector)
    {
        return Create<T1>(
            comparer: (left, right) => (selector(left), selector(right)) switch
            {
                (null, null) => true,
                (null, not null) => false,
                (not null, null) => false,
                var (l, r) => l.Equals(r),
            },
            hashFactory: value => selector(value)?.GetHashCode() ?? 0);
    }

    public static IEqualityComparer<T2> Cast<T1, T2>(this IEqualityComparer<T1> comparer)
        where T2 : T1
    {
        return Create<T2>(
            (left, right) => comparer.Equals(left, right),
            tuple => comparer.GetHashCode(tuple));
    }

    private class GenericEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _comparer;
        private readonly Func<T, int>? _hashFactory;

        public GenericEqualityComparer(Func<T, T, bool> comparer, Func<T, int>? hashFactory)
        {
            _comparer = comparer;
            _hashFactory = hashFactory;
        }

        public bool Equals(T? x, T? y)
        {
            return (x, y) switch
            {
                (null, null) => true,
                (null, not null) or (not null, null) => false,
                _ => _comparer.Invoke(x, y),
            };
        }

        public int GetHashCode(T obj)
            => _hashFactory?.Invoke(obj) ?? obj?.GetHashCode() ?? 0;
    }
}
