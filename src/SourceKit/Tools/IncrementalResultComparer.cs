namespace SourceKit.Tools;

internal sealed class IncrementalResultComparer<TValue, TComparand>(
    Func<TValue, TComparand> selector)
    : IEqualityComparer<IncrementalResult<TValue>>
{
    public bool Equals(IncrementalResult<TValue> x, IncrementalResult<TValue> y)
    {
        if (x.Kind != y.Kind)
            return false;

        return x.Kind switch
        {
            IncrementalResultKind.Skip => true,
            IncrementalResultKind.Failure => x.Diagnostics.Equals(y.Diagnostics),
            IncrementalResultKind.Success => (selector(x.Value!), selector(y.Value!)) switch
            {
                (null, null) => true,
                (not null, null) or (null, not null) => false,
                var (l, r) => l.Equals(r),
            },

            _ => false,
        };
    }

    public int GetHashCode(IncrementalResult<TValue> obj)
    {
        return obj.Kind switch
        {
            IncrementalResultKind.Skip => 0,
            IncrementalResultKind.Failure => obj.Diagnostics.GetHashCode(),
            IncrementalResultKind.Success => selector(obj.Value!)?.GetHashCode() ?? 0,
            _ => 0,
        };
    }
}
