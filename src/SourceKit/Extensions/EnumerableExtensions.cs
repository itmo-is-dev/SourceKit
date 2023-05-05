namespace SourceKit.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> enumerable)
        => from x in enumerable where x is not null select x;
}