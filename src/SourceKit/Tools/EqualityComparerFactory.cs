namespace SourceKit.Tools;

public static class EqualityComparerFactory
{
    public static IEqualityComparer<T> Create<T>(Func<T, T, bool> comparer, Func<T, int>? hashFactory = null)
    {
        return new GenericEqualityComparer<T>(comparer, hashFactory);
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