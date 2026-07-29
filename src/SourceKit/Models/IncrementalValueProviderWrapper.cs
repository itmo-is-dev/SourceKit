
using System.Collections.Immutable;


// ReSharper disable InvokeAsExtensionMember
// ReSharper disable once CheckNamespace
namespace Microsoft.CodeAnalysis;

public readonly struct IncrementalValueProvider<T1, T2>(IncrementalValueProvider<(T1, T2)> provider)
{
    private readonly IncrementalValueProvider<(T1, T2)> _provider = provider;

    public static implicit operator IncrementalValueProvider<T1, T2>(IncrementalValueProvider<(T1, T2)> provider)
    {
        return new IncrementalValueProvider<T1, T2>(provider);
    }

    public static implicit operator IncrementalValueProvider<(T1, T2)>(IncrementalValueProvider<T1, T2> provider)
    {
        return provider._provider;
    }

    public IncrementalValueProvider<T1, T2> WithComparer(IEqualityComparer<(T1, T2)> comparer)
    {
        return _provider.WithComparer(comparer);
    }

    public IncrementalValueProvider<T1, T2> WithTrackingName(string name)
    {
        return _provider.WithTrackingName(name);
    }

    public IncrementalValuesProvider<T1, T2> Where(Func<T1, T2, bool> predicate)
    {
        return IncrementalValueProviderExtensions.Where(_provider, tuple => predicate(tuple.Item1, tuple.Item2));
    }

    public IncrementalValueProvider<T3> Select<T3>(
        Func<T1, T2, CancellationToken, T3> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValueProvider<T3> Select<T3>(
        Func<T1, T2, T3> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2));
    }

    public IncrementalValuesProvider<T3> SelectMany<T3>(
        Func<T1, T2, CancellationToken, IEnumerable<T3>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValuesProvider<T3> SelectMany<T3>(
        Func<T1, T2, IEnumerable<T3>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2));
    }
    
    public IncrementalValueProvider<T1, T2, T3> Combine<T3>(
        IncrementalValueProvider<T3> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Right));
    }

    public IncrementalValueProvider<T3, T4> Select<T3, T4>(
        Func<T1, T2, CancellationToken, (T3, T4)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValueProvider<T3, T4> Select<T3, T4>(
        Func<T1, T2, (T3, T4)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2));
    }

    public IncrementalValuesProvider<T3, T4> SelectMany<T3, T4>(
        Func<T1, T2, CancellationToken, IEnumerable<(T3, T4)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValuesProvider<T3, T4> SelectMany<T3, T4>(
        Func<T1, T2, IEnumerable<(T3, T4)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4> Combine<T3, T4>(
        IncrementalValueProvider<(T3, T4)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Right.Item1, tuple.Right.Item2));
    }

    public IncrementalValueProvider<T3, T4, T5> Select<T3, T4, T5>(
        Func<T1, T2, CancellationToken, (T3, T4, T5)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValueProvider<T3, T4, T5> Select<T3, T4, T5>(
        Func<T1, T2, (T3, T4, T5)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2));
    }

    public IncrementalValuesProvider<T3, T4, T5> SelectMany<T3, T4, T5>(
        Func<T1, T2, CancellationToken, IEnumerable<(T3, T4, T5)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValuesProvider<T3, T4, T5> SelectMany<T3, T4, T5>(
        Func<T1, T2, IEnumerable<(T3, T4, T5)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5> Combine<T3, T4, T5>(
        IncrementalValueProvider<(T3, T4, T5)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3));
    }

    public IncrementalValueProvider<T3, T4, T5, T6> Select<T3, T4, T5, T6>(
        Func<T1, T2, CancellationToken, (T3, T4, T5, T6)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValueProvider<T3, T4, T5, T6> Select<T3, T4, T5, T6>(
        Func<T1, T2, (T3, T4, T5, T6)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6> SelectMany<T3, T4, T5, T6>(
        Func<T1, T2, CancellationToken, IEnumerable<(T3, T4, T5, T6)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6> SelectMany<T3, T4, T5, T6>(
        Func<T1, T2, IEnumerable<(T3, T4, T5, T6)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6> Combine<T3, T4, T5, T6>(
        IncrementalValueProvider<(T3, T4, T5, T6)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3, tuple.Right.Item4));
    }

    public IncrementalValueProvider<T3, T4, T5, T6, T7> Select<T3, T4, T5, T6, T7>(
        Func<T1, T2, CancellationToken, (T3, T4, T5, T6, T7)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValueProvider<T3, T4, T5, T6, T7> Select<T3, T4, T5, T6, T7>(
        Func<T1, T2, (T3, T4, T5, T6, T7)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6, T7> SelectMany<T3, T4, T5, T6, T7>(
        Func<T1, T2, CancellationToken, IEnumerable<(T3, T4, T5, T6, T7)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6, T7> SelectMany<T3, T4, T5, T6, T7>(
        Func<T1, T2, IEnumerable<(T3, T4, T5, T6, T7)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7> Combine<T3, T4, T5, T6, T7>(
        IncrementalValueProvider<(T3, T4, T5, T6, T7)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3, tuple.Right.Item4, tuple.Right.Item5));
    }

    public IncrementalValueProvider<T3, T4, T5, T6, T7, T8> Select<T3, T4, T5, T6, T7, T8>(
        Func<T1, T2, CancellationToken, (T3, T4, T5, T6, T7, T8)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValueProvider<T3, T4, T5, T6, T7, T8> Select<T3, T4, T5, T6, T7, T8>(
        Func<T1, T2, (T3, T4, T5, T6, T7, T8)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6, T7, T8> SelectMany<T3, T4, T5, T6, T7, T8>(
        Func<T1, T2, CancellationToken, IEnumerable<(T3, T4, T5, T6, T7, T8)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6, T7, T8> SelectMany<T3, T4, T5, T6, T7, T8>(
        Func<T1, T2, IEnumerable<(T3, T4, T5, T6, T7, T8)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8> Combine<T3, T4, T5, T6, T7, T8>(
        IncrementalValueProvider<(T3, T4, T5, T6, T7, T8)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3, tuple.Right.Item4, tuple.Right.Item5, tuple.Right.Item6));
    }

    public IncrementalValueProvider<T3, T4, T5, T6, T7, T8, T9> Select<T3, T4, T5, T6, T7, T8, T9>(
        Func<T1, T2, CancellationToken, (T3, T4, T5, T6, T7, T8, T9)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValueProvider<T3, T4, T5, T6, T7, T8, T9> Select<T3, T4, T5, T6, T7, T8, T9>(
        Func<T1, T2, (T3, T4, T5, T6, T7, T8, T9)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6, T7, T8, T9> SelectMany<T3, T4, T5, T6, T7, T8, T9>(
        Func<T1, T2, CancellationToken, IEnumerable<(T3, T4, T5, T6, T7, T8, T9)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6, T7, T8, T9> SelectMany<T3, T4, T5, T6, T7, T8, T9>(
        Func<T1, T2, IEnumerable<(T3, T4, T5, T6, T7, T8, T9)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> Combine<T3, T4, T5, T6, T7, T8, T9>(
        IncrementalValueProvider<(T3, T4, T5, T6, T7, T8, T9)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3, tuple.Right.Item4, tuple.Right.Item5, tuple.Right.Item6, tuple.Right.Item7));
    }

    public IncrementalValueProvider<T3, T4, T5, T6, T7, T8, T9, T10> Select<T3, T4, T5, T6, T7, T8, T9, T10>(
        Func<T1, T2, CancellationToken, (T3, T4, T5, T6, T7, T8, T9, T10)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValueProvider<T3, T4, T5, T6, T7, T8, T9, T10> Select<T3, T4, T5, T6, T7, T8, T9, T10>(
        Func<T1, T2, (T3, T4, T5, T6, T7, T8, T9, T10)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6, T7, T8, T9, T10> SelectMany<T3, T4, T5, T6, T7, T8, T9, T10>(
        Func<T1, T2, CancellationToken, IEnumerable<(T3, T4, T5, T6, T7, T8, T9, T10)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6, T7, T8, T9, T10> SelectMany<T3, T4, T5, T6, T7, T8, T9, T10>(
        Func<T1, T2, IEnumerable<(T3, T4, T5, T6, T7, T8, T9, T10)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Combine<T3, T4, T5, T6, T7, T8, T9, T10>(
        IncrementalValueProvider<(T3, T4, T5, T6, T7, T8, T9, T10)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3, tuple.Right.Item4, tuple.Right.Item5, tuple.Right.Item6, tuple.Right.Item7, tuple.Right.Item8));
    }

    public IncrementalValueProvider<T3, T4, T5, T6, T7, T8, T9, T10, T11> Select<T3, T4, T5, T6, T7, T8, T9, T10, T11>(
        Func<T1, T2, CancellationToken, (T3, T4, T5, T6, T7, T8, T9, T10, T11)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValueProvider<T3, T4, T5, T6, T7, T8, T9, T10, T11> Select<T3, T4, T5, T6, T7, T8, T9, T10, T11>(
        Func<T1, T2, (T3, T4, T5, T6, T7, T8, T9, T10, T11)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6, T7, T8, T9, T10, T11> SelectMany<T3, T4, T5, T6, T7, T8, T9, T10, T11>(
        Func<T1, T2, CancellationToken, IEnumerable<(T3, T4, T5, T6, T7, T8, T9, T10, T11)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6, T7, T8, T9, T10, T11> SelectMany<T3, T4, T5, T6, T7, T8, T9, T10, T11>(
        Func<T1, T2, IEnumerable<(T3, T4, T5, T6, T7, T8, T9, T10, T11)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2));
    }
    
}
public readonly struct IncrementalValueProvider<T1, T2, T3>(IncrementalValueProvider<(T1, T2, T3)> provider)
{
    private readonly IncrementalValueProvider<(T1, T2, T3)> _provider = provider;

    public static implicit operator IncrementalValueProvider<T1, T2, T3>(IncrementalValueProvider<(T1, T2, T3)> provider)
    {
        return new IncrementalValueProvider<T1, T2, T3>(provider);
    }

    public static implicit operator IncrementalValueProvider<(T1, T2, T3)>(IncrementalValueProvider<T1, T2, T3> provider)
    {
        return provider._provider;
    }

    public IncrementalValueProvider<T1, T2, T3> WithComparer(IEqualityComparer<(T1, T2, T3)> comparer)
    {
        return _provider.WithComparer(comparer);
    }

    public IncrementalValueProvider<T1, T2, T3> WithTrackingName(string name)
    {
        return _provider.WithTrackingName(name);
    }

    public IncrementalValuesProvider<T1, T2, T3> Where(Func<T1, T2, T3, bool> predicate)
    {
        return IncrementalValueProviderExtensions.Where(_provider, tuple => predicate(tuple.Item1, tuple.Item2, tuple.Item3));
    }

    public IncrementalValueProvider<T4> Select<T4>(
        Func<T1, T2, T3, CancellationToken, T4> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValueProvider<T4> Select<T4>(
        Func<T1, T2, T3, T4> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }

    public IncrementalValuesProvider<T4> SelectMany<T4>(
        Func<T1, T2, T3, CancellationToken, IEnumerable<T4>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValuesProvider<T4> SelectMany<T4>(
        Func<T1, T2, T3, IEnumerable<T4>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4> Combine<T4>(
        IncrementalValueProvider<T4> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Right));
    }

    public IncrementalValueProvider<T4, T5> Select<T4, T5>(
        Func<T1, T2, T3, CancellationToken, (T4, T5)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValueProvider<T4, T5> Select<T4, T5>(
        Func<T1, T2, T3, (T4, T5)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }

    public IncrementalValuesProvider<T4, T5> SelectMany<T4, T5>(
        Func<T1, T2, T3, CancellationToken, IEnumerable<(T4, T5)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValuesProvider<T4, T5> SelectMany<T4, T5>(
        Func<T1, T2, T3, IEnumerable<(T4, T5)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5> Combine<T4, T5>(
        IncrementalValueProvider<(T4, T5)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Right.Item1, tuple.Right.Item2));
    }

    public IncrementalValueProvider<T4, T5, T6> Select<T4, T5, T6>(
        Func<T1, T2, T3, CancellationToken, (T4, T5, T6)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValueProvider<T4, T5, T6> Select<T4, T5, T6>(
        Func<T1, T2, T3, (T4, T5, T6)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }

    public IncrementalValuesProvider<T4, T5, T6> SelectMany<T4, T5, T6>(
        Func<T1, T2, T3, CancellationToken, IEnumerable<(T4, T5, T6)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValuesProvider<T4, T5, T6> SelectMany<T4, T5, T6>(
        Func<T1, T2, T3, IEnumerable<(T4, T5, T6)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6> Combine<T4, T5, T6>(
        IncrementalValueProvider<(T4, T5, T6)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3));
    }

    public IncrementalValueProvider<T4, T5, T6, T7> Select<T4, T5, T6, T7>(
        Func<T1, T2, T3, CancellationToken, (T4, T5, T6, T7)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValueProvider<T4, T5, T6, T7> Select<T4, T5, T6, T7>(
        Func<T1, T2, T3, (T4, T5, T6, T7)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7> SelectMany<T4, T5, T6, T7>(
        Func<T1, T2, T3, CancellationToken, IEnumerable<(T4, T5, T6, T7)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7> SelectMany<T4, T5, T6, T7>(
        Func<T1, T2, T3, IEnumerable<(T4, T5, T6, T7)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7> Combine<T4, T5, T6, T7>(
        IncrementalValueProvider<(T4, T5, T6, T7)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3, tuple.Right.Item4));
    }

    public IncrementalValueProvider<T4, T5, T6, T7, T8> Select<T4, T5, T6, T7, T8>(
        Func<T1, T2, T3, CancellationToken, (T4, T5, T6, T7, T8)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValueProvider<T4, T5, T6, T7, T8> Select<T4, T5, T6, T7, T8>(
        Func<T1, T2, T3, (T4, T5, T6, T7, T8)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7, T8> SelectMany<T4, T5, T6, T7, T8>(
        Func<T1, T2, T3, CancellationToken, IEnumerable<(T4, T5, T6, T7, T8)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7, T8> SelectMany<T4, T5, T6, T7, T8>(
        Func<T1, T2, T3, IEnumerable<(T4, T5, T6, T7, T8)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8> Combine<T4, T5, T6, T7, T8>(
        IncrementalValueProvider<(T4, T5, T6, T7, T8)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3, tuple.Right.Item4, tuple.Right.Item5));
    }

    public IncrementalValueProvider<T4, T5, T6, T7, T8, T9> Select<T4, T5, T6, T7, T8, T9>(
        Func<T1, T2, T3, CancellationToken, (T4, T5, T6, T7, T8, T9)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValueProvider<T4, T5, T6, T7, T8, T9> Select<T4, T5, T6, T7, T8, T9>(
        Func<T1, T2, T3, (T4, T5, T6, T7, T8, T9)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7, T8, T9> SelectMany<T4, T5, T6, T7, T8, T9>(
        Func<T1, T2, T3, CancellationToken, IEnumerable<(T4, T5, T6, T7, T8, T9)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7, T8, T9> SelectMany<T4, T5, T6, T7, T8, T9>(
        Func<T1, T2, T3, IEnumerable<(T4, T5, T6, T7, T8, T9)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> Combine<T4, T5, T6, T7, T8, T9>(
        IncrementalValueProvider<(T4, T5, T6, T7, T8, T9)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3, tuple.Right.Item4, tuple.Right.Item5, tuple.Right.Item6));
    }

    public IncrementalValueProvider<T4, T5, T6, T7, T8, T9, T10> Select<T4, T5, T6, T7, T8, T9, T10>(
        Func<T1, T2, T3, CancellationToken, (T4, T5, T6, T7, T8, T9, T10)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValueProvider<T4, T5, T6, T7, T8, T9, T10> Select<T4, T5, T6, T7, T8, T9, T10>(
        Func<T1, T2, T3, (T4, T5, T6, T7, T8, T9, T10)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7, T8, T9, T10> SelectMany<T4, T5, T6, T7, T8, T9, T10>(
        Func<T1, T2, T3, CancellationToken, IEnumerable<(T4, T5, T6, T7, T8, T9, T10)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7, T8, T9, T10> SelectMany<T4, T5, T6, T7, T8, T9, T10>(
        Func<T1, T2, T3, IEnumerable<(T4, T5, T6, T7, T8, T9, T10)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Combine<T4, T5, T6, T7, T8, T9, T10>(
        IncrementalValueProvider<(T4, T5, T6, T7, T8, T9, T10)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3, tuple.Right.Item4, tuple.Right.Item5, tuple.Right.Item6, tuple.Right.Item7));
    }

    public IncrementalValueProvider<T4, T5, T6, T7, T8, T9, T10, T11> Select<T4, T5, T6, T7, T8, T9, T10, T11>(
        Func<T1, T2, T3, CancellationToken, (T4, T5, T6, T7, T8, T9, T10, T11)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValueProvider<T4, T5, T6, T7, T8, T9, T10, T11> Select<T4, T5, T6, T7, T8, T9, T10, T11>(
        Func<T1, T2, T3, (T4, T5, T6, T7, T8, T9, T10, T11)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7, T8, T9, T10, T11> SelectMany<T4, T5, T6, T7, T8, T9, T10, T11>(
        Func<T1, T2, T3, CancellationToken, IEnumerable<(T4, T5, T6, T7, T8, T9, T10, T11)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7, T8, T9, T10, T11> SelectMany<T4, T5, T6, T7, T8, T9, T10, T11>(
        Func<T1, T2, T3, IEnumerable<(T4, T5, T6, T7, T8, T9, T10, T11)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }
    

    public IncrementalValueProvider<T4, T5, T6, T7, T8, T9, T10, T11, T12> Select<T4, T5, T6, T7, T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, CancellationToken, (T4, T5, T6, T7, T8, T9, T10, T11, T12)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValueProvider<T4, T5, T6, T7, T8, T9, T10, T11, T12> Select<T4, T5, T6, T7, T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, (T4, T5, T6, T7, T8, T9, T10, T11, T12)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7, T8, T9, T10, T11, T12> SelectMany<T4, T5, T6, T7, T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, CancellationToken, IEnumerable<(T4, T5, T6, T7, T8, T9, T10, T11, T12)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7, T8, T9, T10, T11, T12> SelectMany<T4, T5, T6, T7, T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, IEnumerable<(T4, T5, T6, T7, T8, T9, T10, T11, T12)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }
    
}
public readonly struct IncrementalValueProvider<T1, T2, T3, T4>(IncrementalValueProvider<(T1, T2, T3, T4)> provider)
{
    private readonly IncrementalValueProvider<(T1, T2, T3, T4)> _provider = provider;

    public static implicit operator IncrementalValueProvider<T1, T2, T3, T4>(IncrementalValueProvider<(T1, T2, T3, T4)> provider)
    {
        return new IncrementalValueProvider<T1, T2, T3, T4>(provider);
    }

    public static implicit operator IncrementalValueProvider<(T1, T2, T3, T4)>(IncrementalValueProvider<T1, T2, T3, T4> provider)
    {
        return provider._provider;
    }

    public IncrementalValueProvider<T1, T2, T3, T4> WithComparer(IEqualityComparer<(T1, T2, T3, T4)> comparer)
    {
        return _provider.WithComparer(comparer);
    }

    public IncrementalValueProvider<T1, T2, T3, T4> WithTrackingName(string name)
    {
        return _provider.WithTrackingName(name);
    }

    public IncrementalValuesProvider<T1, T2, T3, T4> Where(Func<T1, T2, T3, T4, bool> predicate)
    {
        return IncrementalValueProviderExtensions.Where(_provider, tuple => predicate(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }

    public IncrementalValueProvider<T5> Select<T5>(
        Func<T1, T2, T3, T4, CancellationToken, T5> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValueProvider<T5> Select<T5>(
        Func<T1, T2, T3, T4, T5> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }

    public IncrementalValuesProvider<T5> SelectMany<T5>(
        Func<T1, T2, T3, T4, CancellationToken, IEnumerable<T5>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValuesProvider<T5> SelectMany<T5>(
        Func<T1, T2, T3, T4, IEnumerable<T5>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5> Combine<T5>(
        IncrementalValueProvider<T5> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Right));
    }

    public IncrementalValueProvider<T5, T6> Select<T5, T6>(
        Func<T1, T2, T3, T4, CancellationToken, (T5, T6)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValueProvider<T5, T6> Select<T5, T6>(
        Func<T1, T2, T3, T4, (T5, T6)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }

    public IncrementalValuesProvider<T5, T6> SelectMany<T5, T6>(
        Func<T1, T2, T3, T4, CancellationToken, IEnumerable<(T5, T6)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValuesProvider<T5, T6> SelectMany<T5, T6>(
        Func<T1, T2, T3, T4, IEnumerable<(T5, T6)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6> Combine<T5, T6>(
        IncrementalValueProvider<(T5, T6)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Right.Item1, tuple.Right.Item2));
    }

    public IncrementalValueProvider<T5, T6, T7> Select<T5, T6, T7>(
        Func<T1, T2, T3, T4, CancellationToken, (T5, T6, T7)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValueProvider<T5, T6, T7> Select<T5, T6, T7>(
        Func<T1, T2, T3, T4, (T5, T6, T7)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }

    public IncrementalValuesProvider<T5, T6, T7> SelectMany<T5, T6, T7>(
        Func<T1, T2, T3, T4, CancellationToken, IEnumerable<(T5, T6, T7)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValuesProvider<T5, T6, T7> SelectMany<T5, T6, T7>(
        Func<T1, T2, T3, T4, IEnumerable<(T5, T6, T7)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7> Combine<T5, T6, T7>(
        IncrementalValueProvider<(T5, T6, T7)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3));
    }

    public IncrementalValueProvider<T5, T6, T7, T8> Select<T5, T6, T7, T8>(
        Func<T1, T2, T3, T4, CancellationToken, (T5, T6, T7, T8)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValueProvider<T5, T6, T7, T8> Select<T5, T6, T7, T8>(
        Func<T1, T2, T3, T4, (T5, T6, T7, T8)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8> SelectMany<T5, T6, T7, T8>(
        Func<T1, T2, T3, T4, CancellationToken, IEnumerable<(T5, T6, T7, T8)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8> SelectMany<T5, T6, T7, T8>(
        Func<T1, T2, T3, T4, IEnumerable<(T5, T6, T7, T8)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8> Combine<T5, T6, T7, T8>(
        IncrementalValueProvider<(T5, T6, T7, T8)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3, tuple.Right.Item4));
    }

    public IncrementalValueProvider<T5, T6, T7, T8, T9> Select<T5, T6, T7, T8, T9>(
        Func<T1, T2, T3, T4, CancellationToken, (T5, T6, T7, T8, T9)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValueProvider<T5, T6, T7, T8, T9> Select<T5, T6, T7, T8, T9>(
        Func<T1, T2, T3, T4, (T5, T6, T7, T8, T9)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8, T9> SelectMany<T5, T6, T7, T8, T9>(
        Func<T1, T2, T3, T4, CancellationToken, IEnumerable<(T5, T6, T7, T8, T9)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8, T9> SelectMany<T5, T6, T7, T8, T9>(
        Func<T1, T2, T3, T4, IEnumerable<(T5, T6, T7, T8, T9)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> Combine<T5, T6, T7, T8, T9>(
        IncrementalValueProvider<(T5, T6, T7, T8, T9)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3, tuple.Right.Item4, tuple.Right.Item5));
    }

    public IncrementalValueProvider<T5, T6, T7, T8, T9, T10> Select<T5, T6, T7, T8, T9, T10>(
        Func<T1, T2, T3, T4, CancellationToken, (T5, T6, T7, T8, T9, T10)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValueProvider<T5, T6, T7, T8, T9, T10> Select<T5, T6, T7, T8, T9, T10>(
        Func<T1, T2, T3, T4, (T5, T6, T7, T8, T9, T10)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8, T9, T10> SelectMany<T5, T6, T7, T8, T9, T10>(
        Func<T1, T2, T3, T4, CancellationToken, IEnumerable<(T5, T6, T7, T8, T9, T10)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8, T9, T10> SelectMany<T5, T6, T7, T8, T9, T10>(
        Func<T1, T2, T3, T4, IEnumerable<(T5, T6, T7, T8, T9, T10)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Combine<T5, T6, T7, T8, T9, T10>(
        IncrementalValueProvider<(T5, T6, T7, T8, T9, T10)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3, tuple.Right.Item4, tuple.Right.Item5, tuple.Right.Item6));
    }

    public IncrementalValueProvider<T5, T6, T7, T8, T9, T10, T11> Select<T5, T6, T7, T8, T9, T10, T11>(
        Func<T1, T2, T3, T4, CancellationToken, (T5, T6, T7, T8, T9, T10, T11)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValueProvider<T5, T6, T7, T8, T9, T10, T11> Select<T5, T6, T7, T8, T9, T10, T11>(
        Func<T1, T2, T3, T4, (T5, T6, T7, T8, T9, T10, T11)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8, T9, T10, T11> SelectMany<T5, T6, T7, T8, T9, T10, T11>(
        Func<T1, T2, T3, T4, CancellationToken, IEnumerable<(T5, T6, T7, T8, T9, T10, T11)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8, T9, T10, T11> SelectMany<T5, T6, T7, T8, T9, T10, T11>(
        Func<T1, T2, T3, T4, IEnumerable<(T5, T6, T7, T8, T9, T10, T11)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }
    

    public IncrementalValueProvider<T5, T6, T7, T8, T9, T10, T11, T12> Select<T5, T6, T7, T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, CancellationToken, (T5, T6, T7, T8, T9, T10, T11, T12)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValueProvider<T5, T6, T7, T8, T9, T10, T11, T12> Select<T5, T6, T7, T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, (T5, T6, T7, T8, T9, T10, T11, T12)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8, T9, T10, T11, T12> SelectMany<T5, T6, T7, T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, CancellationToken, IEnumerable<(T5, T6, T7, T8, T9, T10, T11, T12)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8, T9, T10, T11, T12> SelectMany<T5, T6, T7, T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, IEnumerable<(T5, T6, T7, T8, T9, T10, T11, T12)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }
    

    public IncrementalValueProvider<T5, T6, T7, T8, T9, T10, T11, T12, T13> Select<T5, T6, T7, T8, T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, CancellationToken, (T5, T6, T7, T8, T9, T10, T11, T12, T13)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValueProvider<T5, T6, T7, T8, T9, T10, T11, T12, T13> Select<T5, T6, T7, T8, T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, (T5, T6, T7, T8, T9, T10, T11, T12, T13)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8, T9, T10, T11, T12, T13> SelectMany<T5, T6, T7, T8, T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, CancellationToken, IEnumerable<(T5, T6, T7, T8, T9, T10, T11, T12, T13)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8, T9, T10, T11, T12, T13> SelectMany<T5, T6, T7, T8, T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, IEnumerable<(T5, T6, T7, T8, T9, T10, T11, T12, T13)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }
    
}
public readonly struct IncrementalValueProvider<T1, T2, T3, T4, T5>(IncrementalValueProvider<(T1, T2, T3, T4, T5)> provider)
{
    private readonly IncrementalValueProvider<(T1, T2, T3, T4, T5)> _provider = provider;

    public static implicit operator IncrementalValueProvider<T1, T2, T3, T4, T5>(IncrementalValueProvider<(T1, T2, T3, T4, T5)> provider)
    {
        return new IncrementalValueProvider<T1, T2, T3, T4, T5>(provider);
    }

    public static implicit operator IncrementalValueProvider<(T1, T2, T3, T4, T5)>(IncrementalValueProvider<T1, T2, T3, T4, T5> provider)
    {
        return provider._provider;
    }

    public IncrementalValueProvider<T1, T2, T3, T4, T5> WithComparer(IEqualityComparer<(T1, T2, T3, T4, T5)> comparer)
    {
        return _provider.WithComparer(comparer);
    }

    public IncrementalValueProvider<T1, T2, T3, T4, T5> WithTrackingName(string name)
    {
        return _provider.WithTrackingName(name);
    }

    public IncrementalValuesProvider<T1, T2, T3, T4, T5> Where(Func<T1, T2, T3, T4, T5, bool> predicate)
    {
        return IncrementalValueProviderExtensions.Where(_provider, tuple => predicate(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }

    public IncrementalValueProvider<T6> Select<T6>(
        Func<T1, T2, T3, T4, T5, CancellationToken, T6> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValueProvider<T6> Select<T6>(
        Func<T1, T2, T3, T4, T5, T6> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }

    public IncrementalValuesProvider<T6> SelectMany<T6>(
        Func<T1, T2, T3, T4, T5, CancellationToken, IEnumerable<T6>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValuesProvider<T6> SelectMany<T6>(
        Func<T1, T2, T3, T4, T5, IEnumerable<T6>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6> Combine<T6>(
        IncrementalValueProvider<T6> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Right));
    }

    public IncrementalValueProvider<T6, T7> Select<T6, T7>(
        Func<T1, T2, T3, T4, T5, CancellationToken, (T6, T7)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValueProvider<T6, T7> Select<T6, T7>(
        Func<T1, T2, T3, T4, T5, (T6, T7)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }

    public IncrementalValuesProvider<T6, T7> SelectMany<T6, T7>(
        Func<T1, T2, T3, T4, T5, CancellationToken, IEnumerable<(T6, T7)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValuesProvider<T6, T7> SelectMany<T6, T7>(
        Func<T1, T2, T3, T4, T5, IEnumerable<(T6, T7)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7> Combine<T6, T7>(
        IncrementalValueProvider<(T6, T7)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Right.Item1, tuple.Right.Item2));
    }

    public IncrementalValueProvider<T6, T7, T8> Select<T6, T7, T8>(
        Func<T1, T2, T3, T4, T5, CancellationToken, (T6, T7, T8)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValueProvider<T6, T7, T8> Select<T6, T7, T8>(
        Func<T1, T2, T3, T4, T5, (T6, T7, T8)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }

    public IncrementalValuesProvider<T6, T7, T8> SelectMany<T6, T7, T8>(
        Func<T1, T2, T3, T4, T5, CancellationToken, IEnumerable<(T6, T7, T8)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValuesProvider<T6, T7, T8> SelectMany<T6, T7, T8>(
        Func<T1, T2, T3, T4, T5, IEnumerable<(T6, T7, T8)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8> Combine<T6, T7, T8>(
        IncrementalValueProvider<(T6, T7, T8)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3));
    }

    public IncrementalValueProvider<T6, T7, T8, T9> Select<T6, T7, T8, T9>(
        Func<T1, T2, T3, T4, T5, CancellationToken, (T6, T7, T8, T9)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValueProvider<T6, T7, T8, T9> Select<T6, T7, T8, T9>(
        Func<T1, T2, T3, T4, T5, (T6, T7, T8, T9)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9> SelectMany<T6, T7, T8, T9>(
        Func<T1, T2, T3, T4, T5, CancellationToken, IEnumerable<(T6, T7, T8, T9)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9> SelectMany<T6, T7, T8, T9>(
        Func<T1, T2, T3, T4, T5, IEnumerable<(T6, T7, T8, T9)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> Combine<T6, T7, T8, T9>(
        IncrementalValueProvider<(T6, T7, T8, T9)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3, tuple.Right.Item4));
    }

    public IncrementalValueProvider<T6, T7, T8, T9, T10> Select<T6, T7, T8, T9, T10>(
        Func<T1, T2, T3, T4, T5, CancellationToken, (T6, T7, T8, T9, T10)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValueProvider<T6, T7, T8, T9, T10> Select<T6, T7, T8, T9, T10>(
        Func<T1, T2, T3, T4, T5, (T6, T7, T8, T9, T10)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9, T10> SelectMany<T6, T7, T8, T9, T10>(
        Func<T1, T2, T3, T4, T5, CancellationToken, IEnumerable<(T6, T7, T8, T9, T10)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9, T10> SelectMany<T6, T7, T8, T9, T10>(
        Func<T1, T2, T3, T4, T5, IEnumerable<(T6, T7, T8, T9, T10)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Combine<T6, T7, T8, T9, T10>(
        IncrementalValueProvider<(T6, T7, T8, T9, T10)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3, tuple.Right.Item4, tuple.Right.Item5));
    }

    public IncrementalValueProvider<T6, T7, T8, T9, T10, T11> Select<T6, T7, T8, T9, T10, T11>(
        Func<T1, T2, T3, T4, T5, CancellationToken, (T6, T7, T8, T9, T10, T11)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValueProvider<T6, T7, T8, T9, T10, T11> Select<T6, T7, T8, T9, T10, T11>(
        Func<T1, T2, T3, T4, T5, (T6, T7, T8, T9, T10, T11)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9, T10, T11> SelectMany<T6, T7, T8, T9, T10, T11>(
        Func<T1, T2, T3, T4, T5, CancellationToken, IEnumerable<(T6, T7, T8, T9, T10, T11)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9, T10, T11> SelectMany<T6, T7, T8, T9, T10, T11>(
        Func<T1, T2, T3, T4, T5, IEnumerable<(T6, T7, T8, T9, T10, T11)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }
    

    public IncrementalValueProvider<T6, T7, T8, T9, T10, T11, T12> Select<T6, T7, T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, CancellationToken, (T6, T7, T8, T9, T10, T11, T12)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValueProvider<T6, T7, T8, T9, T10, T11, T12> Select<T6, T7, T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, (T6, T7, T8, T9, T10, T11, T12)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9, T10, T11, T12> SelectMany<T6, T7, T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, CancellationToken, IEnumerable<(T6, T7, T8, T9, T10, T11, T12)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9, T10, T11, T12> SelectMany<T6, T7, T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, IEnumerable<(T6, T7, T8, T9, T10, T11, T12)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }
    

    public IncrementalValueProvider<T6, T7, T8, T9, T10, T11, T12, T13> Select<T6, T7, T8, T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, CancellationToken, (T6, T7, T8, T9, T10, T11, T12, T13)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValueProvider<T6, T7, T8, T9, T10, T11, T12, T13> Select<T6, T7, T8, T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, (T6, T7, T8, T9, T10, T11, T12, T13)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9, T10, T11, T12, T13> SelectMany<T6, T7, T8, T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, CancellationToken, IEnumerable<(T6, T7, T8, T9, T10, T11, T12, T13)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9, T10, T11, T12, T13> SelectMany<T6, T7, T8, T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, IEnumerable<(T6, T7, T8, T9, T10, T11, T12, T13)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }
    

    public IncrementalValueProvider<T6, T7, T8, T9, T10, T11, T12, T13, T14> Select<T6, T7, T8, T9, T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, CancellationToken, (T6, T7, T8, T9, T10, T11, T12, T13, T14)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValueProvider<T6, T7, T8, T9, T10, T11, T12, T13, T14> Select<T6, T7, T8, T9, T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, (T6, T7, T8, T9, T10, T11, T12, T13, T14)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9, T10, T11, T12, T13, T14> SelectMany<T6, T7, T8, T9, T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, CancellationToken, IEnumerable<(T6, T7, T8, T9, T10, T11, T12, T13, T14)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9, T10, T11, T12, T13, T14> SelectMany<T6, T7, T8, T9, T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, IEnumerable<(T6, T7, T8, T9, T10, T11, T12, T13, T14)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }
    
}
public readonly struct IncrementalValueProvider<T1, T2, T3, T4, T5, T6>(IncrementalValueProvider<(T1, T2, T3, T4, T5, T6)> provider)
{
    private readonly IncrementalValueProvider<(T1, T2, T3, T4, T5, T6)> _provider = provider;

    public static implicit operator IncrementalValueProvider<T1, T2, T3, T4, T5, T6>(IncrementalValueProvider<(T1, T2, T3, T4, T5, T6)> provider)
    {
        return new IncrementalValueProvider<T1, T2, T3, T4, T5, T6>(provider);
    }

    public static implicit operator IncrementalValueProvider<(T1, T2, T3, T4, T5, T6)>(IncrementalValueProvider<T1, T2, T3, T4, T5, T6> provider)
    {
        return provider._provider;
    }

    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6> WithComparer(IEqualityComparer<(T1, T2, T3, T4, T5, T6)> comparer)
    {
        return _provider.WithComparer(comparer);
    }

    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6> WithTrackingName(string name)
    {
        return _provider.WithTrackingName(name);
    }

    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6> Where(Func<T1, T2, T3, T4, T5, T6, bool> predicate)
    {
        return IncrementalValueProviderExtensions.Where(_provider, tuple => predicate(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }

    public IncrementalValueProvider<T7> Select<T7>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, T7> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValueProvider<T7> Select<T7>(
        Func<T1, T2, T3, T4, T5, T6, T7> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }

    public IncrementalValuesProvider<T7> SelectMany<T7>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, IEnumerable<T7>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValuesProvider<T7> SelectMany<T7>(
        Func<T1, T2, T3, T4, T5, T6, IEnumerable<T7>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7> Combine<T7>(
        IncrementalValueProvider<T7> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Left.Item6, tuple.Right));
    }

    public IncrementalValueProvider<T7, T8> Select<T7, T8>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, (T7, T8)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValueProvider<T7, T8> Select<T7, T8>(
        Func<T1, T2, T3, T4, T5, T6, (T7, T8)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }

    public IncrementalValuesProvider<T7, T8> SelectMany<T7, T8>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, IEnumerable<(T7, T8)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValuesProvider<T7, T8> SelectMany<T7, T8>(
        Func<T1, T2, T3, T4, T5, T6, IEnumerable<(T7, T8)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8> Combine<T7, T8>(
        IncrementalValueProvider<(T7, T8)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Left.Item6, tuple.Right.Item1, tuple.Right.Item2));
    }

    public IncrementalValueProvider<T7, T8, T9> Select<T7, T8, T9>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, (T7, T8, T9)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValueProvider<T7, T8, T9> Select<T7, T8, T9>(
        Func<T1, T2, T3, T4, T5, T6, (T7, T8, T9)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }

    public IncrementalValuesProvider<T7, T8, T9> SelectMany<T7, T8, T9>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, IEnumerable<(T7, T8, T9)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValuesProvider<T7, T8, T9> SelectMany<T7, T8, T9>(
        Func<T1, T2, T3, T4, T5, T6, IEnumerable<(T7, T8, T9)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> Combine<T7, T8, T9>(
        IncrementalValueProvider<(T7, T8, T9)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Left.Item6, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3));
    }

    public IncrementalValueProvider<T7, T8, T9, T10> Select<T7, T8, T9, T10>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, (T7, T8, T9, T10)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValueProvider<T7, T8, T9, T10> Select<T7, T8, T9, T10>(
        Func<T1, T2, T3, T4, T5, T6, (T7, T8, T9, T10)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }

    public IncrementalValuesProvider<T7, T8, T9, T10> SelectMany<T7, T8, T9, T10>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, IEnumerable<(T7, T8, T9, T10)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValuesProvider<T7, T8, T9, T10> SelectMany<T7, T8, T9, T10>(
        Func<T1, T2, T3, T4, T5, T6, IEnumerable<(T7, T8, T9, T10)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Combine<T7, T8, T9, T10>(
        IncrementalValueProvider<(T7, T8, T9, T10)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Left.Item6, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3, tuple.Right.Item4));
    }

    public IncrementalValueProvider<T7, T8, T9, T10, T11> Select<T7, T8, T9, T10, T11>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, (T7, T8, T9, T10, T11)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValueProvider<T7, T8, T9, T10, T11> Select<T7, T8, T9, T10, T11>(
        Func<T1, T2, T3, T4, T5, T6, (T7, T8, T9, T10, T11)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }

    public IncrementalValuesProvider<T7, T8, T9, T10, T11> SelectMany<T7, T8, T9, T10, T11>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, IEnumerable<(T7, T8, T9, T10, T11)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValuesProvider<T7, T8, T9, T10, T11> SelectMany<T7, T8, T9, T10, T11>(
        Func<T1, T2, T3, T4, T5, T6, IEnumerable<(T7, T8, T9, T10, T11)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }
    

    public IncrementalValueProvider<T7, T8, T9, T10, T11, T12> Select<T7, T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, (T7, T8, T9, T10, T11, T12)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValueProvider<T7, T8, T9, T10, T11, T12> Select<T7, T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, (T7, T8, T9, T10, T11, T12)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }

    public IncrementalValuesProvider<T7, T8, T9, T10, T11, T12> SelectMany<T7, T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, IEnumerable<(T7, T8, T9, T10, T11, T12)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValuesProvider<T7, T8, T9, T10, T11, T12> SelectMany<T7, T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, IEnumerable<(T7, T8, T9, T10, T11, T12)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }
    

    public IncrementalValueProvider<T7, T8, T9, T10, T11, T12, T13> Select<T7, T8, T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, (T7, T8, T9, T10, T11, T12, T13)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValueProvider<T7, T8, T9, T10, T11, T12, T13> Select<T7, T8, T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, (T7, T8, T9, T10, T11, T12, T13)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }

    public IncrementalValuesProvider<T7, T8, T9, T10, T11, T12, T13> SelectMany<T7, T8, T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, IEnumerable<(T7, T8, T9, T10, T11, T12, T13)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValuesProvider<T7, T8, T9, T10, T11, T12, T13> SelectMany<T7, T8, T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, IEnumerable<(T7, T8, T9, T10, T11, T12, T13)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }
    

    public IncrementalValueProvider<T7, T8, T9, T10, T11, T12, T13, T14> Select<T7, T8, T9, T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, (T7, T8, T9, T10, T11, T12, T13, T14)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValueProvider<T7, T8, T9, T10, T11, T12, T13, T14> Select<T7, T8, T9, T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, (T7, T8, T9, T10, T11, T12, T13, T14)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }

    public IncrementalValuesProvider<T7, T8, T9, T10, T11, T12, T13, T14> SelectMany<T7, T8, T9, T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, IEnumerable<(T7, T8, T9, T10, T11, T12, T13, T14)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValuesProvider<T7, T8, T9, T10, T11, T12, T13, T14> SelectMany<T7, T8, T9, T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, IEnumerable<(T7, T8, T9, T10, T11, T12, T13, T14)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }
    

    public IncrementalValueProvider<T7, T8, T9, T10, T11, T12, T13, T14, T15> Select<T7, T8, T9, T10, T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, (T7, T8, T9, T10, T11, T12, T13, T14, T15)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValueProvider<T7, T8, T9, T10, T11, T12, T13, T14, T15> Select<T7, T8, T9, T10, T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, (T7, T8, T9, T10, T11, T12, T13, T14, T15)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }

    public IncrementalValuesProvider<T7, T8, T9, T10, T11, T12, T13, T14, T15> SelectMany<T7, T8, T9, T10, T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, IEnumerable<(T7, T8, T9, T10, T11, T12, T13, T14, T15)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValuesProvider<T7, T8, T9, T10, T11, T12, T13, T14, T15> SelectMany<T7, T8, T9, T10, T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, IEnumerable<(T7, T8, T9, T10, T11, T12, T13, T14, T15)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }
    
}
public readonly struct IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7>(IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7)> provider)
{
    private readonly IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7)> _provider = provider;

    public static implicit operator IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7>(IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7)> provider)
    {
        return new IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7>(provider);
    }

    public static implicit operator IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7)>(IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7> provider)
    {
        return provider._provider;
    }

    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7> WithComparer(IEqualityComparer<(T1, T2, T3, T4, T5, T6, T7)> comparer)
    {
        return _provider.WithComparer(comparer);
    }

    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7> WithTrackingName(string name)
    {
        return _provider.WithTrackingName(name);
    }

    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7> Where(Func<T1, T2, T3, T4, T5, T6, T7, bool> predicate)
    {
        return IncrementalValueProviderExtensions.Where(_provider, tuple => predicate(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }

    public IncrementalValueProvider<T8> Select<T8>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, T8> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValueProvider<T8> Select<T8>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }

    public IncrementalValuesProvider<T8> SelectMany<T8>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, IEnumerable<T8>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValuesProvider<T8> SelectMany<T8>(
        Func<T1, T2, T3, T4, T5, T6, T7, IEnumerable<T8>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8> Combine<T8>(
        IncrementalValueProvider<T8> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Left.Item6, tuple.Left.Item7, tuple.Right));
    }

    public IncrementalValueProvider<T8, T9> Select<T8, T9>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, (T8, T9)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValueProvider<T8, T9> Select<T8, T9>(
        Func<T1, T2, T3, T4, T5, T6, T7, (T8, T9)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }

    public IncrementalValuesProvider<T8, T9> SelectMany<T8, T9>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, IEnumerable<(T8, T9)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValuesProvider<T8, T9> SelectMany<T8, T9>(
        Func<T1, T2, T3, T4, T5, T6, T7, IEnumerable<(T8, T9)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> Combine<T8, T9>(
        IncrementalValueProvider<(T8, T9)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Left.Item6, tuple.Left.Item7, tuple.Right.Item1, tuple.Right.Item2));
    }

    public IncrementalValueProvider<T8, T9, T10> Select<T8, T9, T10>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, (T8, T9, T10)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValueProvider<T8, T9, T10> Select<T8, T9, T10>(
        Func<T1, T2, T3, T4, T5, T6, T7, (T8, T9, T10)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }

    public IncrementalValuesProvider<T8, T9, T10> SelectMany<T8, T9, T10>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, IEnumerable<(T8, T9, T10)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValuesProvider<T8, T9, T10> SelectMany<T8, T9, T10>(
        Func<T1, T2, T3, T4, T5, T6, T7, IEnumerable<(T8, T9, T10)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Combine<T8, T9, T10>(
        IncrementalValueProvider<(T8, T9, T10)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Left.Item6, tuple.Left.Item7, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3));
    }

    public IncrementalValueProvider<T8, T9, T10, T11> Select<T8, T9, T10, T11>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, (T8, T9, T10, T11)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValueProvider<T8, T9, T10, T11> Select<T8, T9, T10, T11>(
        Func<T1, T2, T3, T4, T5, T6, T7, (T8, T9, T10, T11)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }

    public IncrementalValuesProvider<T8, T9, T10, T11> SelectMany<T8, T9, T10, T11>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, IEnumerable<(T8, T9, T10, T11)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValuesProvider<T8, T9, T10, T11> SelectMany<T8, T9, T10, T11>(
        Func<T1, T2, T3, T4, T5, T6, T7, IEnumerable<(T8, T9, T10, T11)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }
    

    public IncrementalValueProvider<T8, T9, T10, T11, T12> Select<T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, (T8, T9, T10, T11, T12)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValueProvider<T8, T9, T10, T11, T12> Select<T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, T7, (T8, T9, T10, T11, T12)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }

    public IncrementalValuesProvider<T8, T9, T10, T11, T12> SelectMany<T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, IEnumerable<(T8, T9, T10, T11, T12)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValuesProvider<T8, T9, T10, T11, T12> SelectMany<T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, T7, IEnumerable<(T8, T9, T10, T11, T12)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }
    

    public IncrementalValueProvider<T8, T9, T10, T11, T12, T13> Select<T8, T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, (T8, T9, T10, T11, T12, T13)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValueProvider<T8, T9, T10, T11, T12, T13> Select<T8, T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, T7, (T8, T9, T10, T11, T12, T13)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }

    public IncrementalValuesProvider<T8, T9, T10, T11, T12, T13> SelectMany<T8, T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, IEnumerable<(T8, T9, T10, T11, T12, T13)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValuesProvider<T8, T9, T10, T11, T12, T13> SelectMany<T8, T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, T7, IEnumerable<(T8, T9, T10, T11, T12, T13)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }
    

    public IncrementalValueProvider<T8, T9, T10, T11, T12, T13, T14> Select<T8, T9, T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, (T8, T9, T10, T11, T12, T13, T14)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValueProvider<T8, T9, T10, T11, T12, T13, T14> Select<T8, T9, T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, T7, (T8, T9, T10, T11, T12, T13, T14)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }

    public IncrementalValuesProvider<T8, T9, T10, T11, T12, T13, T14> SelectMany<T8, T9, T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, IEnumerable<(T8, T9, T10, T11, T12, T13, T14)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValuesProvider<T8, T9, T10, T11, T12, T13, T14> SelectMany<T8, T9, T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, T7, IEnumerable<(T8, T9, T10, T11, T12, T13, T14)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }
    

    public IncrementalValueProvider<T8, T9, T10, T11, T12, T13, T14, T15> Select<T8, T9, T10, T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, (T8, T9, T10, T11, T12, T13, T14, T15)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValueProvider<T8, T9, T10, T11, T12, T13, T14, T15> Select<T8, T9, T10, T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, T7, (T8, T9, T10, T11, T12, T13, T14, T15)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }

    public IncrementalValuesProvider<T8, T9, T10, T11, T12, T13, T14, T15> SelectMany<T8, T9, T10, T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, IEnumerable<(T8, T9, T10, T11, T12, T13, T14, T15)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValuesProvider<T8, T9, T10, T11, T12, T13, T14, T15> SelectMany<T8, T9, T10, T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, T7, IEnumerable<(T8, T9, T10, T11, T12, T13, T14, T15)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }
    

    public IncrementalValueProvider<T8, T9, T10, T11, T12, T13, T14, T15, T16> Select<T8, T9, T10, T11, T12, T13, T14, T15, T16>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, (T8, T9, T10, T11, T12, T13, T14, T15, T16)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValueProvider<T8, T9, T10, T11, T12, T13, T14, T15, T16> Select<T8, T9, T10, T11, T12, T13, T14, T15, T16>(
        Func<T1, T2, T3, T4, T5, T6, T7, (T8, T9, T10, T11, T12, T13, T14, T15, T16)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }

    public IncrementalValuesProvider<T8, T9, T10, T11, T12, T13, T14, T15, T16> SelectMany<T8, T9, T10, T11, T12, T13, T14, T15, T16>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, IEnumerable<(T8, T9, T10, T11, T12, T13, T14, T15, T16)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValuesProvider<T8, T9, T10, T11, T12, T13, T14, T15, T16> SelectMany<T8, T9, T10, T11, T12, T13, T14, T15, T16>(
        Func<T1, T2, T3, T4, T5, T6, T7, IEnumerable<(T8, T9, T10, T11, T12, T13, T14, T15, T16)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }
    
}
public readonly struct IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8>(IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8)> provider)
{
    private readonly IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8)> _provider = provider;

    public static implicit operator IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8>(IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8)> provider)
    {
        return new IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8>(provider);
    }

    public static implicit operator IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8)>(IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8> provider)
    {
        return provider._provider;
    }

    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8> WithComparer(IEqualityComparer<(T1, T2, T3, T4, T5, T6, T7, T8)> comparer)
    {
        return _provider.WithComparer(comparer);
    }

    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8> WithTrackingName(string name)
    {
        return _provider.WithTrackingName(name);
    }

    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8> Where(Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> predicate)
    {
        return IncrementalValueProviderExtensions.Where(_provider, tuple => predicate(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }

    public IncrementalValueProvider<T9> Select<T9>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, T9> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValueProvider<T9> Select<T9>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }

    public IncrementalValuesProvider<T9> SelectMany<T9>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, IEnumerable<T9>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValuesProvider<T9> SelectMany<T9>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, IEnumerable<T9>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> Combine<T9>(
        IncrementalValueProvider<T9> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Left.Item6, tuple.Left.Item7, tuple.Left.Item8, tuple.Right));
    }

    public IncrementalValueProvider<T9, T10> Select<T9, T10>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, (T9, T10)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValueProvider<T9, T10> Select<T9, T10>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, (T9, T10)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }

    public IncrementalValuesProvider<T9, T10> SelectMany<T9, T10>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, IEnumerable<(T9, T10)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValuesProvider<T9, T10> SelectMany<T9, T10>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, IEnumerable<(T9, T10)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Combine<T9, T10>(
        IncrementalValueProvider<(T9, T10)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Left.Item6, tuple.Left.Item7, tuple.Left.Item8, tuple.Right.Item1, tuple.Right.Item2));
    }

    public IncrementalValueProvider<T9, T10, T11> Select<T9, T10, T11>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, (T9, T10, T11)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValueProvider<T9, T10, T11> Select<T9, T10, T11>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, (T9, T10, T11)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }

    public IncrementalValuesProvider<T9, T10, T11> SelectMany<T9, T10, T11>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, IEnumerable<(T9, T10, T11)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValuesProvider<T9, T10, T11> SelectMany<T9, T10, T11>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, IEnumerable<(T9, T10, T11)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }
    

    public IncrementalValueProvider<T9, T10, T11, T12> Select<T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, (T9, T10, T11, T12)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValueProvider<T9, T10, T11, T12> Select<T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, (T9, T10, T11, T12)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }

    public IncrementalValuesProvider<T9, T10, T11, T12> SelectMany<T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, IEnumerable<(T9, T10, T11, T12)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValuesProvider<T9, T10, T11, T12> SelectMany<T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, IEnumerable<(T9, T10, T11, T12)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }
    

    public IncrementalValueProvider<T9, T10, T11, T12, T13> Select<T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, (T9, T10, T11, T12, T13)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValueProvider<T9, T10, T11, T12, T13> Select<T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, (T9, T10, T11, T12, T13)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }

    public IncrementalValuesProvider<T9, T10, T11, T12, T13> SelectMany<T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, IEnumerable<(T9, T10, T11, T12, T13)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValuesProvider<T9, T10, T11, T12, T13> SelectMany<T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, IEnumerable<(T9, T10, T11, T12, T13)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }
    

    public IncrementalValueProvider<T9, T10, T11, T12, T13, T14> Select<T9, T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, (T9, T10, T11, T12, T13, T14)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValueProvider<T9, T10, T11, T12, T13, T14> Select<T9, T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, (T9, T10, T11, T12, T13, T14)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }

    public IncrementalValuesProvider<T9, T10, T11, T12, T13, T14> SelectMany<T9, T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, IEnumerable<(T9, T10, T11, T12, T13, T14)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValuesProvider<T9, T10, T11, T12, T13, T14> SelectMany<T9, T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, IEnumerable<(T9, T10, T11, T12, T13, T14)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }
    

    public IncrementalValueProvider<T9, T10, T11, T12, T13, T14, T15> Select<T9, T10, T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, (T9, T10, T11, T12, T13, T14, T15)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValueProvider<T9, T10, T11, T12, T13, T14, T15> Select<T9, T10, T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, (T9, T10, T11, T12, T13, T14, T15)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }

    public IncrementalValuesProvider<T9, T10, T11, T12, T13, T14, T15> SelectMany<T9, T10, T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, IEnumerable<(T9, T10, T11, T12, T13, T14, T15)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValuesProvider<T9, T10, T11, T12, T13, T14, T15> SelectMany<T9, T10, T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, IEnumerable<(T9, T10, T11, T12, T13, T14, T15)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }
    

    public IncrementalValueProvider<T9, T10, T11, T12, T13, T14, T15, T16> Select<T9, T10, T11, T12, T13, T14, T15, T16>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, (T9, T10, T11, T12, T13, T14, T15, T16)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValueProvider<T9, T10, T11, T12, T13, T14, T15, T16> Select<T9, T10, T11, T12, T13, T14, T15, T16>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, (T9, T10, T11, T12, T13, T14, T15, T16)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }

    public IncrementalValuesProvider<T9, T10, T11, T12, T13, T14, T15, T16> SelectMany<T9, T10, T11, T12, T13, T14, T15, T16>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, IEnumerable<(T9, T10, T11, T12, T13, T14, T15, T16)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValuesProvider<T9, T10, T11, T12, T13, T14, T15, T16> SelectMany<T9, T10, T11, T12, T13, T14, T15, T16>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, IEnumerable<(T9, T10, T11, T12, T13, T14, T15, T16)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }
    

    public IncrementalValueProvider<T9, T10, T11, T12, T13, T14, T15, T16, T17> Select<T9, T10, T11, T12, T13, T14, T15, T16, T17>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, (T9, T10, T11, T12, T13, T14, T15, T16, T17)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValueProvider<T9, T10, T11, T12, T13, T14, T15, T16, T17> Select<T9, T10, T11, T12, T13, T14, T15, T16, T17>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, (T9, T10, T11, T12, T13, T14, T15, T16, T17)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }

    public IncrementalValuesProvider<T9, T10, T11, T12, T13, T14, T15, T16, T17> SelectMany<T9, T10, T11, T12, T13, T14, T15, T16, T17>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, IEnumerable<(T9, T10, T11, T12, T13, T14, T15, T16, T17)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValuesProvider<T9, T10, T11, T12, T13, T14, T15, T16, T17> SelectMany<T9, T10, T11, T12, T13, T14, T15, T16, T17>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, IEnumerable<(T9, T10, T11, T12, T13, T14, T15, T16, T17)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }
    
}
public readonly struct IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9>(IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> provider)
{
    private readonly IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> _provider = provider;

    public static implicit operator IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9>(IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> provider)
    {
        return new IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9>(provider);
    }

    public static implicit operator IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9)>(IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> provider)
    {
        return provider._provider;
    }

    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> WithComparer(IEqualityComparer<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> comparer)
    {
        return _provider.WithComparer(comparer);
    }

    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> WithTrackingName(string name)
    {
        return _provider.WithTrackingName(name);
    }

    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> Where(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> predicate)
    {
        return IncrementalValueProviderExtensions.Where(_provider, tuple => predicate(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }

    public IncrementalValueProvider<T10> Select<T10>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, T10> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValueProvider<T10> Select<T10>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }

    public IncrementalValuesProvider<T10> SelectMany<T10>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, IEnumerable<T10>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValuesProvider<T10> SelectMany<T10>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IEnumerable<T10>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }
    
    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Combine<T10>(
        IncrementalValueProvider<T10> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Left.Item6, tuple.Left.Item7, tuple.Left.Item8, tuple.Left.Item9, tuple.Right));
    }

    public IncrementalValueProvider<T10, T11> Select<T10, T11>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, (T10, T11)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValueProvider<T10, T11> Select<T10, T11>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, (T10, T11)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }

    public IncrementalValuesProvider<T10, T11> SelectMany<T10, T11>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, IEnumerable<(T10, T11)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValuesProvider<T10, T11> SelectMany<T10, T11>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IEnumerable<(T10, T11)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }
    

    public IncrementalValueProvider<T10, T11, T12> Select<T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, (T10, T11, T12)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValueProvider<T10, T11, T12> Select<T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, (T10, T11, T12)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }

    public IncrementalValuesProvider<T10, T11, T12> SelectMany<T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, IEnumerable<(T10, T11, T12)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValuesProvider<T10, T11, T12> SelectMany<T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IEnumerable<(T10, T11, T12)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }
    

    public IncrementalValueProvider<T10, T11, T12, T13> Select<T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, (T10, T11, T12, T13)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValueProvider<T10, T11, T12, T13> Select<T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, (T10, T11, T12, T13)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }

    public IncrementalValuesProvider<T10, T11, T12, T13> SelectMany<T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, IEnumerable<(T10, T11, T12, T13)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValuesProvider<T10, T11, T12, T13> SelectMany<T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IEnumerable<(T10, T11, T12, T13)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }
    

    public IncrementalValueProvider<T10, T11, T12, T13, T14> Select<T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, (T10, T11, T12, T13, T14)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValueProvider<T10, T11, T12, T13, T14> Select<T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, (T10, T11, T12, T13, T14)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }

    public IncrementalValuesProvider<T10, T11, T12, T13, T14> SelectMany<T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, IEnumerable<(T10, T11, T12, T13, T14)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValuesProvider<T10, T11, T12, T13, T14> SelectMany<T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IEnumerable<(T10, T11, T12, T13, T14)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }
    

    public IncrementalValueProvider<T10, T11, T12, T13, T14, T15> Select<T10, T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, (T10, T11, T12, T13, T14, T15)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValueProvider<T10, T11, T12, T13, T14, T15> Select<T10, T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, (T10, T11, T12, T13, T14, T15)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }

    public IncrementalValuesProvider<T10, T11, T12, T13, T14, T15> SelectMany<T10, T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, IEnumerable<(T10, T11, T12, T13, T14, T15)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValuesProvider<T10, T11, T12, T13, T14, T15> SelectMany<T10, T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IEnumerable<(T10, T11, T12, T13, T14, T15)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }
    

    public IncrementalValueProvider<T10, T11, T12, T13, T14, T15, T16> Select<T10, T11, T12, T13, T14, T15, T16>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, (T10, T11, T12, T13, T14, T15, T16)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValueProvider<T10, T11, T12, T13, T14, T15, T16> Select<T10, T11, T12, T13, T14, T15, T16>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, (T10, T11, T12, T13, T14, T15, T16)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }

    public IncrementalValuesProvider<T10, T11, T12, T13, T14, T15, T16> SelectMany<T10, T11, T12, T13, T14, T15, T16>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, IEnumerable<(T10, T11, T12, T13, T14, T15, T16)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValuesProvider<T10, T11, T12, T13, T14, T15, T16> SelectMany<T10, T11, T12, T13, T14, T15, T16>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IEnumerable<(T10, T11, T12, T13, T14, T15, T16)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }
    

    public IncrementalValueProvider<T10, T11, T12, T13, T14, T15, T16, T17> Select<T10, T11, T12, T13, T14, T15, T16, T17>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, (T10, T11, T12, T13, T14, T15, T16, T17)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValueProvider<T10, T11, T12, T13, T14, T15, T16, T17> Select<T10, T11, T12, T13, T14, T15, T16, T17>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, (T10, T11, T12, T13, T14, T15, T16, T17)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }

    public IncrementalValuesProvider<T10, T11, T12, T13, T14, T15, T16, T17> SelectMany<T10, T11, T12, T13, T14, T15, T16, T17>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, IEnumerable<(T10, T11, T12, T13, T14, T15, T16, T17)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValuesProvider<T10, T11, T12, T13, T14, T15, T16, T17> SelectMany<T10, T11, T12, T13, T14, T15, T16, T17>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IEnumerable<(T10, T11, T12, T13, T14, T15, T16, T17)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }
    

    public IncrementalValueProvider<T10, T11, T12, T13, T14, T15, T16, T17, T18> Select<T10, T11, T12, T13, T14, T15, T16, T17, T18>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, (T10, T11, T12, T13, T14, T15, T16, T17, T18)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValueProvider<T10, T11, T12, T13, T14, T15, T16, T17, T18> Select<T10, T11, T12, T13, T14, T15, T16, T17, T18>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, (T10, T11, T12, T13, T14, T15, T16, T17, T18)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }

    public IncrementalValuesProvider<T10, T11, T12, T13, T14, T15, T16, T17, T18> SelectMany<T10, T11, T12, T13, T14, T15, T16, T17, T18>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, IEnumerable<(T10, T11, T12, T13, T14, T15, T16, T17, T18)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValuesProvider<T10, T11, T12, T13, T14, T15, T16, T17, T18> SelectMany<T10, T11, T12, T13, T14, T15, T16, T17, T18>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IEnumerable<(T10, T11, T12, T13, T14, T15, T16, T17, T18)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }
    
}
public readonly struct IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> provider)
{
    private readonly IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> _provider = provider;

    public static implicit operator IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> provider)
    {
        return new IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(provider);
    }

    public static implicit operator IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)>(IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> provider)
    {
        return provider._provider;
    }

    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> WithComparer(IEqualityComparer<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> comparer)
    {
        return _provider.WithComparer(comparer);
    }

    public IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> WithTrackingName(string name)
    {
        return _provider.WithTrackingName(name);
    }

    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Where(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> predicate)
    {
        return IncrementalValueProviderExtensions.Where(_provider, tuple => predicate(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }

    public IncrementalValueProvider<T11> Select<T11>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, T11> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValueProvider<T11> Select<T11>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }

    public IncrementalValuesProvider<T11> SelectMany<T11>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, IEnumerable<T11>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValuesProvider<T11> SelectMany<T11>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IEnumerable<T11>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }
    

    public IncrementalValueProvider<T11, T12> Select<T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, (T11, T12)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValueProvider<T11, T12> Select<T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, (T11, T12)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }

    public IncrementalValuesProvider<T11, T12> SelectMany<T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, IEnumerable<(T11, T12)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValuesProvider<T11, T12> SelectMany<T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IEnumerable<(T11, T12)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }
    

    public IncrementalValueProvider<T11, T12, T13> Select<T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, (T11, T12, T13)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValueProvider<T11, T12, T13> Select<T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, (T11, T12, T13)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }

    public IncrementalValuesProvider<T11, T12, T13> SelectMany<T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, IEnumerable<(T11, T12, T13)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValuesProvider<T11, T12, T13> SelectMany<T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IEnumerable<(T11, T12, T13)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }
    

    public IncrementalValueProvider<T11, T12, T13, T14> Select<T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, (T11, T12, T13, T14)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValueProvider<T11, T12, T13, T14> Select<T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, (T11, T12, T13, T14)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }

    public IncrementalValuesProvider<T11, T12, T13, T14> SelectMany<T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, IEnumerable<(T11, T12, T13, T14)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValuesProvider<T11, T12, T13, T14> SelectMany<T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IEnumerable<(T11, T12, T13, T14)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }
    

    public IncrementalValueProvider<T11, T12, T13, T14, T15> Select<T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, (T11, T12, T13, T14, T15)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValueProvider<T11, T12, T13, T14, T15> Select<T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, (T11, T12, T13, T14, T15)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }

    public IncrementalValuesProvider<T11, T12, T13, T14, T15> SelectMany<T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, IEnumerable<(T11, T12, T13, T14, T15)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValuesProvider<T11, T12, T13, T14, T15> SelectMany<T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IEnumerable<(T11, T12, T13, T14, T15)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }
    

    public IncrementalValueProvider<T11, T12, T13, T14, T15, T16> Select<T11, T12, T13, T14, T15, T16>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, (T11, T12, T13, T14, T15, T16)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValueProvider<T11, T12, T13, T14, T15, T16> Select<T11, T12, T13, T14, T15, T16>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, (T11, T12, T13, T14, T15, T16)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }

    public IncrementalValuesProvider<T11, T12, T13, T14, T15, T16> SelectMany<T11, T12, T13, T14, T15, T16>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, IEnumerable<(T11, T12, T13, T14, T15, T16)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValuesProvider<T11, T12, T13, T14, T15, T16> SelectMany<T11, T12, T13, T14, T15, T16>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IEnumerable<(T11, T12, T13, T14, T15, T16)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }
    

    public IncrementalValueProvider<T11, T12, T13, T14, T15, T16, T17> Select<T11, T12, T13, T14, T15, T16, T17>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, (T11, T12, T13, T14, T15, T16, T17)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValueProvider<T11, T12, T13, T14, T15, T16, T17> Select<T11, T12, T13, T14, T15, T16, T17>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, (T11, T12, T13, T14, T15, T16, T17)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }

    public IncrementalValuesProvider<T11, T12, T13, T14, T15, T16, T17> SelectMany<T11, T12, T13, T14, T15, T16, T17>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, IEnumerable<(T11, T12, T13, T14, T15, T16, T17)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValuesProvider<T11, T12, T13, T14, T15, T16, T17> SelectMany<T11, T12, T13, T14, T15, T16, T17>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IEnumerable<(T11, T12, T13, T14, T15, T16, T17)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }
    

    public IncrementalValueProvider<T11, T12, T13, T14, T15, T16, T17, T18> Select<T11, T12, T13, T14, T15, T16, T17, T18>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, (T11, T12, T13, T14, T15, T16, T17, T18)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValueProvider<T11, T12, T13, T14, T15, T16, T17, T18> Select<T11, T12, T13, T14, T15, T16, T17, T18>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, (T11, T12, T13, T14, T15, T16, T17, T18)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }

    public IncrementalValuesProvider<T11, T12, T13, T14, T15, T16, T17, T18> SelectMany<T11, T12, T13, T14, T15, T16, T17, T18>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, IEnumerable<(T11, T12, T13, T14, T15, T16, T17, T18)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValuesProvider<T11, T12, T13, T14, T15, T16, T17, T18> SelectMany<T11, T12, T13, T14, T15, T16, T17, T18>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IEnumerable<(T11, T12, T13, T14, T15, T16, T17, T18)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }
    

    public IncrementalValueProvider<T11, T12, T13, T14, T15, T16, T17, T18, T19> Select<T11, T12, T13, T14, T15, T16, T17, T18, T19>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, (T11, T12, T13, T14, T15, T16, T17, T18, T19)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValueProvider<T11, T12, T13, T14, T15, T16, T17, T18, T19> Select<T11, T12, T13, T14, T15, T16, T17, T18, T19>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, (T11, T12, T13, T14, T15, T16, T17, T18, T19)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }

    public IncrementalValuesProvider<T11, T12, T13, T14, T15, T16, T17, T18, T19> SelectMany<T11, T12, T13, T14, T15, T16, T17, T18, T19>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, IEnumerable<(T11, T12, T13, T14, T15, T16, T17, T18, T19)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValuesProvider<T11, T12, T13, T14, T15, T16, T17, T18, T19> SelectMany<T11, T12, T13, T14, T15, T16, T17, T18, T19>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IEnumerable<(T11, T12, T13, T14, T15, T16, T17, T18, T19)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }
    
}
public readonly struct IncrementalValuesProvider<T1, T2>(IncrementalValuesProvider<(T1, T2)> provider)
{
    private readonly IncrementalValuesProvider<(T1, T2)> _provider = provider;

    public static implicit operator IncrementalValuesProvider<T1, T2>(IncrementalValuesProvider<(T1, T2)> provider)
    {
        return new IncrementalValuesProvider<T1, T2>(provider);
    }

    public static implicit operator IncrementalValuesProvider<(T1, T2)>(IncrementalValuesProvider<T1, T2> provider)
    {
        return provider._provider;
    }

    public IncrementalValuesProvider<T1, T2> WithComparer(IEqualityComparer<(T1, T2)> comparer)
    {
        return _provider.WithComparer(comparer);
    }

    public IncrementalValuesProvider<T1, T2> WithTrackingName(string name)
    {
        return _provider.WithTrackingName(name);
    }

    public IncrementalValuesProvider<T1, T2> Where(Func<T1, T2, bool> predicate)
    {
        return IncrementalValueProviderExtensions.Where(_provider, tuple => predicate(tuple.Item1, tuple.Item2));
    }
    public IncrementalValueProvider<ImmutableArray<(T1, T2)>> Collect()
    {
        return _provider.Collect();
    }

    public IncrementalValuesProvider<T3> Select<T3>(
        Func<T1, T2, CancellationToken, T3> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValuesProvider<T3> Select<T3>(
        Func<T1, T2, T3> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2));
    }

    public IncrementalValuesProvider<T3> SelectMany<T3>(
        Func<T1, T2, CancellationToken, IEnumerable<T3>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValuesProvider<T3> SelectMany<T3>(
        Func<T1, T2, IEnumerable<T3>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2));
    }
    
    public IncrementalValuesProvider<T1, T2, T3> Combine<T3>(
        IncrementalValueProvider<T3> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Right));
    }

    public IncrementalValuesProvider<T3, T4> Select<T3, T4>(
        Func<T1, T2, CancellationToken, (T3, T4)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValuesProvider<T3, T4> Select<T3, T4>(
        Func<T1, T2, (T3, T4)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2));
    }

    public IncrementalValuesProvider<T3, T4> SelectMany<T3, T4>(
        Func<T1, T2, CancellationToken, IEnumerable<(T3, T4)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValuesProvider<T3, T4> SelectMany<T3, T4>(
        Func<T1, T2, IEnumerable<(T3, T4)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4> Combine<T3, T4>(
        IncrementalValueProvider<(T3, T4)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Right.Item1, tuple.Right.Item2));
    }

    public IncrementalValuesProvider<T3, T4, T5> Select<T3, T4, T5>(
        Func<T1, T2, CancellationToken, (T3, T4, T5)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValuesProvider<T3, T4, T5> Select<T3, T4, T5>(
        Func<T1, T2, (T3, T4, T5)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2));
    }

    public IncrementalValuesProvider<T3, T4, T5> SelectMany<T3, T4, T5>(
        Func<T1, T2, CancellationToken, IEnumerable<(T3, T4, T5)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValuesProvider<T3, T4, T5> SelectMany<T3, T4, T5>(
        Func<T1, T2, IEnumerable<(T3, T4, T5)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5> Combine<T3, T4, T5>(
        IncrementalValueProvider<(T3, T4, T5)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6> Select<T3, T4, T5, T6>(
        Func<T1, T2, CancellationToken, (T3, T4, T5, T6)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6> Select<T3, T4, T5, T6>(
        Func<T1, T2, (T3, T4, T5, T6)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6> SelectMany<T3, T4, T5, T6>(
        Func<T1, T2, CancellationToken, IEnumerable<(T3, T4, T5, T6)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6> SelectMany<T3, T4, T5, T6>(
        Func<T1, T2, IEnumerable<(T3, T4, T5, T6)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6> Combine<T3, T4, T5, T6>(
        IncrementalValueProvider<(T3, T4, T5, T6)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3, tuple.Right.Item4));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6, T7> Select<T3, T4, T5, T6, T7>(
        Func<T1, T2, CancellationToken, (T3, T4, T5, T6, T7)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6, T7> Select<T3, T4, T5, T6, T7>(
        Func<T1, T2, (T3, T4, T5, T6, T7)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6, T7> SelectMany<T3, T4, T5, T6, T7>(
        Func<T1, T2, CancellationToken, IEnumerable<(T3, T4, T5, T6, T7)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6, T7> SelectMany<T3, T4, T5, T6, T7>(
        Func<T1, T2, IEnumerable<(T3, T4, T5, T6, T7)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7> Combine<T3, T4, T5, T6, T7>(
        IncrementalValueProvider<(T3, T4, T5, T6, T7)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3, tuple.Right.Item4, tuple.Right.Item5));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6, T7, T8> Select<T3, T4, T5, T6, T7, T8>(
        Func<T1, T2, CancellationToken, (T3, T4, T5, T6, T7, T8)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6, T7, T8> Select<T3, T4, T5, T6, T7, T8>(
        Func<T1, T2, (T3, T4, T5, T6, T7, T8)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6, T7, T8> SelectMany<T3, T4, T5, T6, T7, T8>(
        Func<T1, T2, CancellationToken, IEnumerable<(T3, T4, T5, T6, T7, T8)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6, T7, T8> SelectMany<T3, T4, T5, T6, T7, T8>(
        Func<T1, T2, IEnumerable<(T3, T4, T5, T6, T7, T8)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8> Combine<T3, T4, T5, T6, T7, T8>(
        IncrementalValueProvider<(T3, T4, T5, T6, T7, T8)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3, tuple.Right.Item4, tuple.Right.Item5, tuple.Right.Item6));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6, T7, T8, T9> Select<T3, T4, T5, T6, T7, T8, T9>(
        Func<T1, T2, CancellationToken, (T3, T4, T5, T6, T7, T8, T9)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6, T7, T8, T9> Select<T3, T4, T5, T6, T7, T8, T9>(
        Func<T1, T2, (T3, T4, T5, T6, T7, T8, T9)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6, T7, T8, T9> SelectMany<T3, T4, T5, T6, T7, T8, T9>(
        Func<T1, T2, CancellationToken, IEnumerable<(T3, T4, T5, T6, T7, T8, T9)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6, T7, T8, T9> SelectMany<T3, T4, T5, T6, T7, T8, T9>(
        Func<T1, T2, IEnumerable<(T3, T4, T5, T6, T7, T8, T9)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> Combine<T3, T4, T5, T6, T7, T8, T9>(
        IncrementalValueProvider<(T3, T4, T5, T6, T7, T8, T9)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3, tuple.Right.Item4, tuple.Right.Item5, tuple.Right.Item6, tuple.Right.Item7));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6, T7, T8, T9, T10> Select<T3, T4, T5, T6, T7, T8, T9, T10>(
        Func<T1, T2, CancellationToken, (T3, T4, T5, T6, T7, T8, T9, T10)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6, T7, T8, T9, T10> Select<T3, T4, T5, T6, T7, T8, T9, T10>(
        Func<T1, T2, (T3, T4, T5, T6, T7, T8, T9, T10)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6, T7, T8, T9, T10> SelectMany<T3, T4, T5, T6, T7, T8, T9, T10>(
        Func<T1, T2, CancellationToken, IEnumerable<(T3, T4, T5, T6, T7, T8, T9, T10)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6, T7, T8, T9, T10> SelectMany<T3, T4, T5, T6, T7, T8, T9, T10>(
        Func<T1, T2, IEnumerable<(T3, T4, T5, T6, T7, T8, T9, T10)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Combine<T3, T4, T5, T6, T7, T8, T9, T10>(
        IncrementalValueProvider<(T3, T4, T5, T6, T7, T8, T9, T10)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3, tuple.Right.Item4, tuple.Right.Item5, tuple.Right.Item6, tuple.Right.Item7, tuple.Right.Item8));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6, T7, T8, T9, T10, T11> Select<T3, T4, T5, T6, T7, T8, T9, T10, T11>(
        Func<T1, T2, CancellationToken, (T3, T4, T5, T6, T7, T8, T9, T10, T11)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6, T7, T8, T9, T10, T11> Select<T3, T4, T5, T6, T7, T8, T9, T10, T11>(
        Func<T1, T2, (T3, T4, T5, T6, T7, T8, T9, T10, T11)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6, T7, T8, T9, T10, T11> SelectMany<T3, T4, T5, T6, T7, T8, T9, T10, T11>(
        Func<T1, T2, CancellationToken, IEnumerable<(T3, T4, T5, T6, T7, T8, T9, T10, T11)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    public IncrementalValuesProvider<T3, T4, T5, T6, T7, T8, T9, T10, T11> SelectMany<T3, T4, T5, T6, T7, T8, T9, T10, T11>(
        Func<T1, T2, IEnumerable<(T3, T4, T5, T6, T7, T8, T9, T10, T11)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2));
    }
    
}
public readonly struct IncrementalValuesProvider<T1, T2, T3>(IncrementalValuesProvider<(T1, T2, T3)> provider)
{
    private readonly IncrementalValuesProvider<(T1, T2, T3)> _provider = provider;

    public static implicit operator IncrementalValuesProvider<T1, T2, T3>(IncrementalValuesProvider<(T1, T2, T3)> provider)
    {
        return new IncrementalValuesProvider<T1, T2, T3>(provider);
    }

    public static implicit operator IncrementalValuesProvider<(T1, T2, T3)>(IncrementalValuesProvider<T1, T2, T3> provider)
    {
        return provider._provider;
    }

    public IncrementalValuesProvider<T1, T2, T3> WithComparer(IEqualityComparer<(T1, T2, T3)> comparer)
    {
        return _provider.WithComparer(comparer);
    }

    public IncrementalValuesProvider<T1, T2, T3> WithTrackingName(string name)
    {
        return _provider.WithTrackingName(name);
    }

    public IncrementalValuesProvider<T1, T2, T3> Where(Func<T1, T2, T3, bool> predicate)
    {
        return IncrementalValueProviderExtensions.Where(_provider, tuple => predicate(tuple.Item1, tuple.Item2, tuple.Item3));
    }
    public IncrementalValueProvider<ImmutableArray<(T1, T2, T3)>> Collect()
    {
        return _provider.Collect();
    }

    public IncrementalValuesProvider<T4> Select<T4>(
        Func<T1, T2, T3, CancellationToken, T4> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValuesProvider<T4> Select<T4>(
        Func<T1, T2, T3, T4> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }

    public IncrementalValuesProvider<T4> SelectMany<T4>(
        Func<T1, T2, T3, CancellationToken, IEnumerable<T4>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValuesProvider<T4> SelectMany<T4>(
        Func<T1, T2, T3, IEnumerable<T4>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4> Combine<T4>(
        IncrementalValueProvider<T4> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Right));
    }

    public IncrementalValuesProvider<T4, T5> Select<T4, T5>(
        Func<T1, T2, T3, CancellationToken, (T4, T5)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValuesProvider<T4, T5> Select<T4, T5>(
        Func<T1, T2, T3, (T4, T5)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }

    public IncrementalValuesProvider<T4, T5> SelectMany<T4, T5>(
        Func<T1, T2, T3, CancellationToken, IEnumerable<(T4, T5)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValuesProvider<T4, T5> SelectMany<T4, T5>(
        Func<T1, T2, T3, IEnumerable<(T4, T5)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5> Combine<T4, T5>(
        IncrementalValueProvider<(T4, T5)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Right.Item1, tuple.Right.Item2));
    }

    public IncrementalValuesProvider<T4, T5, T6> Select<T4, T5, T6>(
        Func<T1, T2, T3, CancellationToken, (T4, T5, T6)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValuesProvider<T4, T5, T6> Select<T4, T5, T6>(
        Func<T1, T2, T3, (T4, T5, T6)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }

    public IncrementalValuesProvider<T4, T5, T6> SelectMany<T4, T5, T6>(
        Func<T1, T2, T3, CancellationToken, IEnumerable<(T4, T5, T6)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValuesProvider<T4, T5, T6> SelectMany<T4, T5, T6>(
        Func<T1, T2, T3, IEnumerable<(T4, T5, T6)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6> Combine<T4, T5, T6>(
        IncrementalValueProvider<(T4, T5, T6)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7> Select<T4, T5, T6, T7>(
        Func<T1, T2, T3, CancellationToken, (T4, T5, T6, T7)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7> Select<T4, T5, T6, T7>(
        Func<T1, T2, T3, (T4, T5, T6, T7)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7> SelectMany<T4, T5, T6, T7>(
        Func<T1, T2, T3, CancellationToken, IEnumerable<(T4, T5, T6, T7)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7> SelectMany<T4, T5, T6, T7>(
        Func<T1, T2, T3, IEnumerable<(T4, T5, T6, T7)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7> Combine<T4, T5, T6, T7>(
        IncrementalValueProvider<(T4, T5, T6, T7)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3, tuple.Right.Item4));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7, T8> Select<T4, T5, T6, T7, T8>(
        Func<T1, T2, T3, CancellationToken, (T4, T5, T6, T7, T8)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7, T8> Select<T4, T5, T6, T7, T8>(
        Func<T1, T2, T3, (T4, T5, T6, T7, T8)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7, T8> SelectMany<T4, T5, T6, T7, T8>(
        Func<T1, T2, T3, CancellationToken, IEnumerable<(T4, T5, T6, T7, T8)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7, T8> SelectMany<T4, T5, T6, T7, T8>(
        Func<T1, T2, T3, IEnumerable<(T4, T5, T6, T7, T8)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8> Combine<T4, T5, T6, T7, T8>(
        IncrementalValueProvider<(T4, T5, T6, T7, T8)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3, tuple.Right.Item4, tuple.Right.Item5));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7, T8, T9> Select<T4, T5, T6, T7, T8, T9>(
        Func<T1, T2, T3, CancellationToken, (T4, T5, T6, T7, T8, T9)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7, T8, T9> Select<T4, T5, T6, T7, T8, T9>(
        Func<T1, T2, T3, (T4, T5, T6, T7, T8, T9)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7, T8, T9> SelectMany<T4, T5, T6, T7, T8, T9>(
        Func<T1, T2, T3, CancellationToken, IEnumerable<(T4, T5, T6, T7, T8, T9)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7, T8, T9> SelectMany<T4, T5, T6, T7, T8, T9>(
        Func<T1, T2, T3, IEnumerable<(T4, T5, T6, T7, T8, T9)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> Combine<T4, T5, T6, T7, T8, T9>(
        IncrementalValueProvider<(T4, T5, T6, T7, T8, T9)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3, tuple.Right.Item4, tuple.Right.Item5, tuple.Right.Item6));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7, T8, T9, T10> Select<T4, T5, T6, T7, T8, T9, T10>(
        Func<T1, T2, T3, CancellationToken, (T4, T5, T6, T7, T8, T9, T10)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7, T8, T9, T10> Select<T4, T5, T6, T7, T8, T9, T10>(
        Func<T1, T2, T3, (T4, T5, T6, T7, T8, T9, T10)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7, T8, T9, T10> SelectMany<T4, T5, T6, T7, T8, T9, T10>(
        Func<T1, T2, T3, CancellationToken, IEnumerable<(T4, T5, T6, T7, T8, T9, T10)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7, T8, T9, T10> SelectMany<T4, T5, T6, T7, T8, T9, T10>(
        Func<T1, T2, T3, IEnumerable<(T4, T5, T6, T7, T8, T9, T10)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Combine<T4, T5, T6, T7, T8, T9, T10>(
        IncrementalValueProvider<(T4, T5, T6, T7, T8, T9, T10)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3, tuple.Right.Item4, tuple.Right.Item5, tuple.Right.Item6, tuple.Right.Item7));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7, T8, T9, T10, T11> Select<T4, T5, T6, T7, T8, T9, T10, T11>(
        Func<T1, T2, T3, CancellationToken, (T4, T5, T6, T7, T8, T9, T10, T11)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7, T8, T9, T10, T11> Select<T4, T5, T6, T7, T8, T9, T10, T11>(
        Func<T1, T2, T3, (T4, T5, T6, T7, T8, T9, T10, T11)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7, T8, T9, T10, T11> SelectMany<T4, T5, T6, T7, T8, T9, T10, T11>(
        Func<T1, T2, T3, CancellationToken, IEnumerable<(T4, T5, T6, T7, T8, T9, T10, T11)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7, T8, T9, T10, T11> SelectMany<T4, T5, T6, T7, T8, T9, T10, T11>(
        Func<T1, T2, T3, IEnumerable<(T4, T5, T6, T7, T8, T9, T10, T11)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }
    

    public IncrementalValuesProvider<T4, T5, T6, T7, T8, T9, T10, T11, T12> Select<T4, T5, T6, T7, T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, CancellationToken, (T4, T5, T6, T7, T8, T9, T10, T11, T12)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7, T8, T9, T10, T11, T12> Select<T4, T5, T6, T7, T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, (T4, T5, T6, T7, T8, T9, T10, T11, T12)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7, T8, T9, T10, T11, T12> SelectMany<T4, T5, T6, T7, T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, CancellationToken, IEnumerable<(T4, T5, T6, T7, T8, T9, T10, T11, T12)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    public IncrementalValuesProvider<T4, T5, T6, T7, T8, T9, T10, T11, T12> SelectMany<T4, T5, T6, T7, T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, IEnumerable<(T4, T5, T6, T7, T8, T9, T10, T11, T12)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }
    
}
public readonly struct IncrementalValuesProvider<T1, T2, T3, T4>(IncrementalValuesProvider<(T1, T2, T3, T4)> provider)
{
    private readonly IncrementalValuesProvider<(T1, T2, T3, T4)> _provider = provider;

    public static implicit operator IncrementalValuesProvider<T1, T2, T3, T4>(IncrementalValuesProvider<(T1, T2, T3, T4)> provider)
    {
        return new IncrementalValuesProvider<T1, T2, T3, T4>(provider);
    }

    public static implicit operator IncrementalValuesProvider<(T1, T2, T3, T4)>(IncrementalValuesProvider<T1, T2, T3, T4> provider)
    {
        return provider._provider;
    }

    public IncrementalValuesProvider<T1, T2, T3, T4> WithComparer(IEqualityComparer<(T1, T2, T3, T4)> comparer)
    {
        return _provider.WithComparer(comparer);
    }

    public IncrementalValuesProvider<T1, T2, T3, T4> WithTrackingName(string name)
    {
        return _provider.WithTrackingName(name);
    }

    public IncrementalValuesProvider<T1, T2, T3, T4> Where(Func<T1, T2, T3, T4, bool> predicate)
    {
        return IncrementalValueProviderExtensions.Where(_provider, tuple => predicate(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }
    public IncrementalValueProvider<ImmutableArray<(T1, T2, T3, T4)>> Collect()
    {
        return _provider.Collect();
    }

    public IncrementalValuesProvider<T5> Select<T5>(
        Func<T1, T2, T3, T4, CancellationToken, T5> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValuesProvider<T5> Select<T5>(
        Func<T1, T2, T3, T4, T5> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }

    public IncrementalValuesProvider<T5> SelectMany<T5>(
        Func<T1, T2, T3, T4, CancellationToken, IEnumerable<T5>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValuesProvider<T5> SelectMany<T5>(
        Func<T1, T2, T3, T4, IEnumerable<T5>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5> Combine<T5>(
        IncrementalValueProvider<T5> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Right));
    }

    public IncrementalValuesProvider<T5, T6> Select<T5, T6>(
        Func<T1, T2, T3, T4, CancellationToken, (T5, T6)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValuesProvider<T5, T6> Select<T5, T6>(
        Func<T1, T2, T3, T4, (T5, T6)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }

    public IncrementalValuesProvider<T5, T6> SelectMany<T5, T6>(
        Func<T1, T2, T3, T4, CancellationToken, IEnumerable<(T5, T6)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValuesProvider<T5, T6> SelectMany<T5, T6>(
        Func<T1, T2, T3, T4, IEnumerable<(T5, T6)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6> Combine<T5, T6>(
        IncrementalValueProvider<(T5, T6)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Right.Item1, tuple.Right.Item2));
    }

    public IncrementalValuesProvider<T5, T6, T7> Select<T5, T6, T7>(
        Func<T1, T2, T3, T4, CancellationToken, (T5, T6, T7)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValuesProvider<T5, T6, T7> Select<T5, T6, T7>(
        Func<T1, T2, T3, T4, (T5, T6, T7)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }

    public IncrementalValuesProvider<T5, T6, T7> SelectMany<T5, T6, T7>(
        Func<T1, T2, T3, T4, CancellationToken, IEnumerable<(T5, T6, T7)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValuesProvider<T5, T6, T7> SelectMany<T5, T6, T7>(
        Func<T1, T2, T3, T4, IEnumerable<(T5, T6, T7)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7> Combine<T5, T6, T7>(
        IncrementalValueProvider<(T5, T6, T7)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8> Select<T5, T6, T7, T8>(
        Func<T1, T2, T3, T4, CancellationToken, (T5, T6, T7, T8)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8> Select<T5, T6, T7, T8>(
        Func<T1, T2, T3, T4, (T5, T6, T7, T8)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8> SelectMany<T5, T6, T7, T8>(
        Func<T1, T2, T3, T4, CancellationToken, IEnumerable<(T5, T6, T7, T8)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8> SelectMany<T5, T6, T7, T8>(
        Func<T1, T2, T3, T4, IEnumerable<(T5, T6, T7, T8)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8> Combine<T5, T6, T7, T8>(
        IncrementalValueProvider<(T5, T6, T7, T8)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3, tuple.Right.Item4));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8, T9> Select<T5, T6, T7, T8, T9>(
        Func<T1, T2, T3, T4, CancellationToken, (T5, T6, T7, T8, T9)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8, T9> Select<T5, T6, T7, T8, T9>(
        Func<T1, T2, T3, T4, (T5, T6, T7, T8, T9)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8, T9> SelectMany<T5, T6, T7, T8, T9>(
        Func<T1, T2, T3, T4, CancellationToken, IEnumerable<(T5, T6, T7, T8, T9)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8, T9> SelectMany<T5, T6, T7, T8, T9>(
        Func<T1, T2, T3, T4, IEnumerable<(T5, T6, T7, T8, T9)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> Combine<T5, T6, T7, T8, T9>(
        IncrementalValueProvider<(T5, T6, T7, T8, T9)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3, tuple.Right.Item4, tuple.Right.Item5));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8, T9, T10> Select<T5, T6, T7, T8, T9, T10>(
        Func<T1, T2, T3, T4, CancellationToken, (T5, T6, T7, T8, T9, T10)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8, T9, T10> Select<T5, T6, T7, T8, T9, T10>(
        Func<T1, T2, T3, T4, (T5, T6, T7, T8, T9, T10)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8, T9, T10> SelectMany<T5, T6, T7, T8, T9, T10>(
        Func<T1, T2, T3, T4, CancellationToken, IEnumerable<(T5, T6, T7, T8, T9, T10)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8, T9, T10> SelectMany<T5, T6, T7, T8, T9, T10>(
        Func<T1, T2, T3, T4, IEnumerable<(T5, T6, T7, T8, T9, T10)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Combine<T5, T6, T7, T8, T9, T10>(
        IncrementalValueProvider<(T5, T6, T7, T8, T9, T10)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3, tuple.Right.Item4, tuple.Right.Item5, tuple.Right.Item6));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8, T9, T10, T11> Select<T5, T6, T7, T8, T9, T10, T11>(
        Func<T1, T2, T3, T4, CancellationToken, (T5, T6, T7, T8, T9, T10, T11)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8, T9, T10, T11> Select<T5, T6, T7, T8, T9, T10, T11>(
        Func<T1, T2, T3, T4, (T5, T6, T7, T8, T9, T10, T11)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8, T9, T10, T11> SelectMany<T5, T6, T7, T8, T9, T10, T11>(
        Func<T1, T2, T3, T4, CancellationToken, IEnumerable<(T5, T6, T7, T8, T9, T10, T11)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8, T9, T10, T11> SelectMany<T5, T6, T7, T8, T9, T10, T11>(
        Func<T1, T2, T3, T4, IEnumerable<(T5, T6, T7, T8, T9, T10, T11)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }
    

    public IncrementalValuesProvider<T5, T6, T7, T8, T9, T10, T11, T12> Select<T5, T6, T7, T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, CancellationToken, (T5, T6, T7, T8, T9, T10, T11, T12)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8, T9, T10, T11, T12> Select<T5, T6, T7, T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, (T5, T6, T7, T8, T9, T10, T11, T12)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8, T9, T10, T11, T12> SelectMany<T5, T6, T7, T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, CancellationToken, IEnumerable<(T5, T6, T7, T8, T9, T10, T11, T12)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8, T9, T10, T11, T12> SelectMany<T5, T6, T7, T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, IEnumerable<(T5, T6, T7, T8, T9, T10, T11, T12)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }
    

    public IncrementalValuesProvider<T5, T6, T7, T8, T9, T10, T11, T12, T13> Select<T5, T6, T7, T8, T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, CancellationToken, (T5, T6, T7, T8, T9, T10, T11, T12, T13)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8, T9, T10, T11, T12, T13> Select<T5, T6, T7, T8, T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, (T5, T6, T7, T8, T9, T10, T11, T12, T13)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8, T9, T10, T11, T12, T13> SelectMany<T5, T6, T7, T8, T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, CancellationToken, IEnumerable<(T5, T6, T7, T8, T9, T10, T11, T12, T13)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    public IncrementalValuesProvider<T5, T6, T7, T8, T9, T10, T11, T12, T13> SelectMany<T5, T6, T7, T8, T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, IEnumerable<(T5, T6, T7, T8, T9, T10, T11, T12, T13)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }
    
}
public readonly struct IncrementalValuesProvider<T1, T2, T3, T4, T5>(IncrementalValuesProvider<(T1, T2, T3, T4, T5)> provider)
{
    private readonly IncrementalValuesProvider<(T1, T2, T3, T4, T5)> _provider = provider;

    public static implicit operator IncrementalValuesProvider<T1, T2, T3, T4, T5>(IncrementalValuesProvider<(T1, T2, T3, T4, T5)> provider)
    {
        return new IncrementalValuesProvider<T1, T2, T3, T4, T5>(provider);
    }

    public static implicit operator IncrementalValuesProvider<(T1, T2, T3, T4, T5)>(IncrementalValuesProvider<T1, T2, T3, T4, T5> provider)
    {
        return provider._provider;
    }

    public IncrementalValuesProvider<T1, T2, T3, T4, T5> WithComparer(IEqualityComparer<(T1, T2, T3, T4, T5)> comparer)
    {
        return _provider.WithComparer(comparer);
    }

    public IncrementalValuesProvider<T1, T2, T3, T4, T5> WithTrackingName(string name)
    {
        return _provider.WithTrackingName(name);
    }

    public IncrementalValuesProvider<T1, T2, T3, T4, T5> Where(Func<T1, T2, T3, T4, T5, bool> predicate)
    {
        return IncrementalValueProviderExtensions.Where(_provider, tuple => predicate(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }
    public IncrementalValueProvider<ImmutableArray<(T1, T2, T3, T4, T5)>> Collect()
    {
        return _provider.Collect();
    }

    public IncrementalValuesProvider<T6> Select<T6>(
        Func<T1, T2, T3, T4, T5, CancellationToken, T6> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValuesProvider<T6> Select<T6>(
        Func<T1, T2, T3, T4, T5, T6> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }

    public IncrementalValuesProvider<T6> SelectMany<T6>(
        Func<T1, T2, T3, T4, T5, CancellationToken, IEnumerable<T6>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValuesProvider<T6> SelectMany<T6>(
        Func<T1, T2, T3, T4, T5, IEnumerable<T6>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6> Combine<T6>(
        IncrementalValueProvider<T6> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Right));
    }

    public IncrementalValuesProvider<T6, T7> Select<T6, T7>(
        Func<T1, T2, T3, T4, T5, CancellationToken, (T6, T7)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValuesProvider<T6, T7> Select<T6, T7>(
        Func<T1, T2, T3, T4, T5, (T6, T7)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }

    public IncrementalValuesProvider<T6, T7> SelectMany<T6, T7>(
        Func<T1, T2, T3, T4, T5, CancellationToken, IEnumerable<(T6, T7)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValuesProvider<T6, T7> SelectMany<T6, T7>(
        Func<T1, T2, T3, T4, T5, IEnumerable<(T6, T7)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7> Combine<T6, T7>(
        IncrementalValueProvider<(T6, T7)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Right.Item1, tuple.Right.Item2));
    }

    public IncrementalValuesProvider<T6, T7, T8> Select<T6, T7, T8>(
        Func<T1, T2, T3, T4, T5, CancellationToken, (T6, T7, T8)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValuesProvider<T6, T7, T8> Select<T6, T7, T8>(
        Func<T1, T2, T3, T4, T5, (T6, T7, T8)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }

    public IncrementalValuesProvider<T6, T7, T8> SelectMany<T6, T7, T8>(
        Func<T1, T2, T3, T4, T5, CancellationToken, IEnumerable<(T6, T7, T8)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValuesProvider<T6, T7, T8> SelectMany<T6, T7, T8>(
        Func<T1, T2, T3, T4, T5, IEnumerable<(T6, T7, T8)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8> Combine<T6, T7, T8>(
        IncrementalValueProvider<(T6, T7, T8)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9> Select<T6, T7, T8, T9>(
        Func<T1, T2, T3, T4, T5, CancellationToken, (T6, T7, T8, T9)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9> Select<T6, T7, T8, T9>(
        Func<T1, T2, T3, T4, T5, (T6, T7, T8, T9)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9> SelectMany<T6, T7, T8, T9>(
        Func<T1, T2, T3, T4, T5, CancellationToken, IEnumerable<(T6, T7, T8, T9)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9> SelectMany<T6, T7, T8, T9>(
        Func<T1, T2, T3, T4, T5, IEnumerable<(T6, T7, T8, T9)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> Combine<T6, T7, T8, T9>(
        IncrementalValueProvider<(T6, T7, T8, T9)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3, tuple.Right.Item4));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9, T10> Select<T6, T7, T8, T9, T10>(
        Func<T1, T2, T3, T4, T5, CancellationToken, (T6, T7, T8, T9, T10)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9, T10> Select<T6, T7, T8, T9, T10>(
        Func<T1, T2, T3, T4, T5, (T6, T7, T8, T9, T10)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9, T10> SelectMany<T6, T7, T8, T9, T10>(
        Func<T1, T2, T3, T4, T5, CancellationToken, IEnumerable<(T6, T7, T8, T9, T10)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9, T10> SelectMany<T6, T7, T8, T9, T10>(
        Func<T1, T2, T3, T4, T5, IEnumerable<(T6, T7, T8, T9, T10)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Combine<T6, T7, T8, T9, T10>(
        IncrementalValueProvider<(T6, T7, T8, T9, T10)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3, tuple.Right.Item4, tuple.Right.Item5));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9, T10, T11> Select<T6, T7, T8, T9, T10, T11>(
        Func<T1, T2, T3, T4, T5, CancellationToken, (T6, T7, T8, T9, T10, T11)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9, T10, T11> Select<T6, T7, T8, T9, T10, T11>(
        Func<T1, T2, T3, T4, T5, (T6, T7, T8, T9, T10, T11)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9, T10, T11> SelectMany<T6, T7, T8, T9, T10, T11>(
        Func<T1, T2, T3, T4, T5, CancellationToken, IEnumerable<(T6, T7, T8, T9, T10, T11)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9, T10, T11> SelectMany<T6, T7, T8, T9, T10, T11>(
        Func<T1, T2, T3, T4, T5, IEnumerable<(T6, T7, T8, T9, T10, T11)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }
    

    public IncrementalValuesProvider<T6, T7, T8, T9, T10, T11, T12> Select<T6, T7, T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, CancellationToken, (T6, T7, T8, T9, T10, T11, T12)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9, T10, T11, T12> Select<T6, T7, T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, (T6, T7, T8, T9, T10, T11, T12)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9, T10, T11, T12> SelectMany<T6, T7, T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, CancellationToken, IEnumerable<(T6, T7, T8, T9, T10, T11, T12)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9, T10, T11, T12> SelectMany<T6, T7, T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, IEnumerable<(T6, T7, T8, T9, T10, T11, T12)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }
    

    public IncrementalValuesProvider<T6, T7, T8, T9, T10, T11, T12, T13> Select<T6, T7, T8, T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, CancellationToken, (T6, T7, T8, T9, T10, T11, T12, T13)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9, T10, T11, T12, T13> Select<T6, T7, T8, T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, (T6, T7, T8, T9, T10, T11, T12, T13)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9, T10, T11, T12, T13> SelectMany<T6, T7, T8, T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, CancellationToken, IEnumerable<(T6, T7, T8, T9, T10, T11, T12, T13)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9, T10, T11, T12, T13> SelectMany<T6, T7, T8, T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, IEnumerable<(T6, T7, T8, T9, T10, T11, T12, T13)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }
    

    public IncrementalValuesProvider<T6, T7, T8, T9, T10, T11, T12, T13, T14> Select<T6, T7, T8, T9, T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, CancellationToken, (T6, T7, T8, T9, T10, T11, T12, T13, T14)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9, T10, T11, T12, T13, T14> Select<T6, T7, T8, T9, T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, (T6, T7, T8, T9, T10, T11, T12, T13, T14)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9, T10, T11, T12, T13, T14> SelectMany<T6, T7, T8, T9, T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, CancellationToken, IEnumerable<(T6, T7, T8, T9, T10, T11, T12, T13, T14)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    public IncrementalValuesProvider<T6, T7, T8, T9, T10, T11, T12, T13, T14> SelectMany<T6, T7, T8, T9, T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, IEnumerable<(T6, T7, T8, T9, T10, T11, T12, T13, T14)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }
    
}
public readonly struct IncrementalValuesProvider<T1, T2, T3, T4, T5, T6>(IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6)> provider)
{
    private readonly IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6)> _provider = provider;

    public static implicit operator IncrementalValuesProvider<T1, T2, T3, T4, T5, T6>(IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6)> provider)
    {
        return new IncrementalValuesProvider<T1, T2, T3, T4, T5, T6>(provider);
    }

    public static implicit operator IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6)>(IncrementalValuesProvider<T1, T2, T3, T4, T5, T6> provider)
    {
        return provider._provider;
    }

    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6> WithComparer(IEqualityComparer<(T1, T2, T3, T4, T5, T6)> comparer)
    {
        return _provider.WithComparer(comparer);
    }

    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6> WithTrackingName(string name)
    {
        return _provider.WithTrackingName(name);
    }

    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6> Where(Func<T1, T2, T3, T4, T5, T6, bool> predicate)
    {
        return IncrementalValueProviderExtensions.Where(_provider, tuple => predicate(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }
    public IncrementalValueProvider<ImmutableArray<(T1, T2, T3, T4, T5, T6)>> Collect()
    {
        return _provider.Collect();
    }

    public IncrementalValuesProvider<T7> Select<T7>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, T7> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValuesProvider<T7> Select<T7>(
        Func<T1, T2, T3, T4, T5, T6, T7> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }

    public IncrementalValuesProvider<T7> SelectMany<T7>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, IEnumerable<T7>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValuesProvider<T7> SelectMany<T7>(
        Func<T1, T2, T3, T4, T5, T6, IEnumerable<T7>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7> Combine<T7>(
        IncrementalValueProvider<T7> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Left.Item6, tuple.Right));
    }

    public IncrementalValuesProvider<T7, T8> Select<T7, T8>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, (T7, T8)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValuesProvider<T7, T8> Select<T7, T8>(
        Func<T1, T2, T3, T4, T5, T6, (T7, T8)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }

    public IncrementalValuesProvider<T7, T8> SelectMany<T7, T8>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, IEnumerable<(T7, T8)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValuesProvider<T7, T8> SelectMany<T7, T8>(
        Func<T1, T2, T3, T4, T5, T6, IEnumerable<(T7, T8)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8> Combine<T7, T8>(
        IncrementalValueProvider<(T7, T8)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Left.Item6, tuple.Right.Item1, tuple.Right.Item2));
    }

    public IncrementalValuesProvider<T7, T8, T9> Select<T7, T8, T9>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, (T7, T8, T9)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValuesProvider<T7, T8, T9> Select<T7, T8, T9>(
        Func<T1, T2, T3, T4, T5, T6, (T7, T8, T9)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }

    public IncrementalValuesProvider<T7, T8, T9> SelectMany<T7, T8, T9>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, IEnumerable<(T7, T8, T9)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValuesProvider<T7, T8, T9> SelectMany<T7, T8, T9>(
        Func<T1, T2, T3, T4, T5, T6, IEnumerable<(T7, T8, T9)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> Combine<T7, T8, T9>(
        IncrementalValueProvider<(T7, T8, T9)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Left.Item6, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3));
    }

    public IncrementalValuesProvider<T7, T8, T9, T10> Select<T7, T8, T9, T10>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, (T7, T8, T9, T10)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValuesProvider<T7, T8, T9, T10> Select<T7, T8, T9, T10>(
        Func<T1, T2, T3, T4, T5, T6, (T7, T8, T9, T10)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }

    public IncrementalValuesProvider<T7, T8, T9, T10> SelectMany<T7, T8, T9, T10>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, IEnumerable<(T7, T8, T9, T10)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValuesProvider<T7, T8, T9, T10> SelectMany<T7, T8, T9, T10>(
        Func<T1, T2, T3, T4, T5, T6, IEnumerable<(T7, T8, T9, T10)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Combine<T7, T8, T9, T10>(
        IncrementalValueProvider<(T7, T8, T9, T10)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Left.Item6, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3, tuple.Right.Item4));
    }

    public IncrementalValuesProvider<T7, T8, T9, T10, T11> Select<T7, T8, T9, T10, T11>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, (T7, T8, T9, T10, T11)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValuesProvider<T7, T8, T9, T10, T11> Select<T7, T8, T9, T10, T11>(
        Func<T1, T2, T3, T4, T5, T6, (T7, T8, T9, T10, T11)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }

    public IncrementalValuesProvider<T7, T8, T9, T10, T11> SelectMany<T7, T8, T9, T10, T11>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, IEnumerable<(T7, T8, T9, T10, T11)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValuesProvider<T7, T8, T9, T10, T11> SelectMany<T7, T8, T9, T10, T11>(
        Func<T1, T2, T3, T4, T5, T6, IEnumerable<(T7, T8, T9, T10, T11)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }
    

    public IncrementalValuesProvider<T7, T8, T9, T10, T11, T12> Select<T7, T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, (T7, T8, T9, T10, T11, T12)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValuesProvider<T7, T8, T9, T10, T11, T12> Select<T7, T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, (T7, T8, T9, T10, T11, T12)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }

    public IncrementalValuesProvider<T7, T8, T9, T10, T11, T12> SelectMany<T7, T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, IEnumerable<(T7, T8, T9, T10, T11, T12)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValuesProvider<T7, T8, T9, T10, T11, T12> SelectMany<T7, T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, IEnumerable<(T7, T8, T9, T10, T11, T12)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }
    

    public IncrementalValuesProvider<T7, T8, T9, T10, T11, T12, T13> Select<T7, T8, T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, (T7, T8, T9, T10, T11, T12, T13)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValuesProvider<T7, T8, T9, T10, T11, T12, T13> Select<T7, T8, T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, (T7, T8, T9, T10, T11, T12, T13)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }

    public IncrementalValuesProvider<T7, T8, T9, T10, T11, T12, T13> SelectMany<T7, T8, T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, IEnumerable<(T7, T8, T9, T10, T11, T12, T13)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValuesProvider<T7, T8, T9, T10, T11, T12, T13> SelectMany<T7, T8, T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, IEnumerable<(T7, T8, T9, T10, T11, T12, T13)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }
    

    public IncrementalValuesProvider<T7, T8, T9, T10, T11, T12, T13, T14> Select<T7, T8, T9, T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, (T7, T8, T9, T10, T11, T12, T13, T14)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValuesProvider<T7, T8, T9, T10, T11, T12, T13, T14> Select<T7, T8, T9, T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, (T7, T8, T9, T10, T11, T12, T13, T14)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }

    public IncrementalValuesProvider<T7, T8, T9, T10, T11, T12, T13, T14> SelectMany<T7, T8, T9, T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, IEnumerable<(T7, T8, T9, T10, T11, T12, T13, T14)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValuesProvider<T7, T8, T9, T10, T11, T12, T13, T14> SelectMany<T7, T8, T9, T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, IEnumerable<(T7, T8, T9, T10, T11, T12, T13, T14)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }
    

    public IncrementalValuesProvider<T7, T8, T9, T10, T11, T12, T13, T14, T15> Select<T7, T8, T9, T10, T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, (T7, T8, T9, T10, T11, T12, T13, T14, T15)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValuesProvider<T7, T8, T9, T10, T11, T12, T13, T14, T15> Select<T7, T8, T9, T10, T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, (T7, T8, T9, T10, T11, T12, T13, T14, T15)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }

    public IncrementalValuesProvider<T7, T8, T9, T10, T11, T12, T13, T14, T15> SelectMany<T7, T8, T9, T10, T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, IEnumerable<(T7, T8, T9, T10, T11, T12, T13, T14, T15)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    public IncrementalValuesProvider<T7, T8, T9, T10, T11, T12, T13, T14, T15> SelectMany<T7, T8, T9, T10, T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, IEnumerable<(T7, T8, T9, T10, T11, T12, T13, T14, T15)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }
    
}
public readonly struct IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7>(IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7)> provider)
{
    private readonly IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7)> _provider = provider;

    public static implicit operator IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7>(IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7)> provider)
    {
        return new IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7>(provider);
    }

    public static implicit operator IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7)>(IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7> provider)
    {
        return provider._provider;
    }

    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7> WithComparer(IEqualityComparer<(T1, T2, T3, T4, T5, T6, T7)> comparer)
    {
        return _provider.WithComparer(comparer);
    }

    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7> WithTrackingName(string name)
    {
        return _provider.WithTrackingName(name);
    }

    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7> Where(Func<T1, T2, T3, T4, T5, T6, T7, bool> predicate)
    {
        return IncrementalValueProviderExtensions.Where(_provider, tuple => predicate(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }
    public IncrementalValueProvider<ImmutableArray<(T1, T2, T3, T4, T5, T6, T7)>> Collect()
    {
        return _provider.Collect();
    }

    public IncrementalValuesProvider<T8> Select<T8>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, T8> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValuesProvider<T8> Select<T8>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }

    public IncrementalValuesProvider<T8> SelectMany<T8>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, IEnumerable<T8>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValuesProvider<T8> SelectMany<T8>(
        Func<T1, T2, T3, T4, T5, T6, T7, IEnumerable<T8>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8> Combine<T8>(
        IncrementalValueProvider<T8> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Left.Item6, tuple.Left.Item7, tuple.Right));
    }

    public IncrementalValuesProvider<T8, T9> Select<T8, T9>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, (T8, T9)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValuesProvider<T8, T9> Select<T8, T9>(
        Func<T1, T2, T3, T4, T5, T6, T7, (T8, T9)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }

    public IncrementalValuesProvider<T8, T9> SelectMany<T8, T9>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, IEnumerable<(T8, T9)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValuesProvider<T8, T9> SelectMany<T8, T9>(
        Func<T1, T2, T3, T4, T5, T6, T7, IEnumerable<(T8, T9)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> Combine<T8, T9>(
        IncrementalValueProvider<(T8, T9)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Left.Item6, tuple.Left.Item7, tuple.Right.Item1, tuple.Right.Item2));
    }

    public IncrementalValuesProvider<T8, T9, T10> Select<T8, T9, T10>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, (T8, T9, T10)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValuesProvider<T8, T9, T10> Select<T8, T9, T10>(
        Func<T1, T2, T3, T4, T5, T6, T7, (T8, T9, T10)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }

    public IncrementalValuesProvider<T8, T9, T10> SelectMany<T8, T9, T10>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, IEnumerable<(T8, T9, T10)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValuesProvider<T8, T9, T10> SelectMany<T8, T9, T10>(
        Func<T1, T2, T3, T4, T5, T6, T7, IEnumerable<(T8, T9, T10)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Combine<T8, T9, T10>(
        IncrementalValueProvider<(T8, T9, T10)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Left.Item6, tuple.Left.Item7, tuple.Right.Item1, tuple.Right.Item2, tuple.Right.Item3));
    }

    public IncrementalValuesProvider<T8, T9, T10, T11> Select<T8, T9, T10, T11>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, (T8, T9, T10, T11)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValuesProvider<T8, T9, T10, T11> Select<T8, T9, T10, T11>(
        Func<T1, T2, T3, T4, T5, T6, T7, (T8, T9, T10, T11)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }

    public IncrementalValuesProvider<T8, T9, T10, T11> SelectMany<T8, T9, T10, T11>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, IEnumerable<(T8, T9, T10, T11)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValuesProvider<T8, T9, T10, T11> SelectMany<T8, T9, T10, T11>(
        Func<T1, T2, T3, T4, T5, T6, T7, IEnumerable<(T8, T9, T10, T11)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }
    

    public IncrementalValuesProvider<T8, T9, T10, T11, T12> Select<T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, (T8, T9, T10, T11, T12)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValuesProvider<T8, T9, T10, T11, T12> Select<T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, T7, (T8, T9, T10, T11, T12)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }

    public IncrementalValuesProvider<T8, T9, T10, T11, T12> SelectMany<T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, IEnumerable<(T8, T9, T10, T11, T12)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValuesProvider<T8, T9, T10, T11, T12> SelectMany<T8, T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, T7, IEnumerable<(T8, T9, T10, T11, T12)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }
    

    public IncrementalValuesProvider<T8, T9, T10, T11, T12, T13> Select<T8, T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, (T8, T9, T10, T11, T12, T13)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValuesProvider<T8, T9, T10, T11, T12, T13> Select<T8, T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, T7, (T8, T9, T10, T11, T12, T13)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }

    public IncrementalValuesProvider<T8, T9, T10, T11, T12, T13> SelectMany<T8, T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, IEnumerable<(T8, T9, T10, T11, T12, T13)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValuesProvider<T8, T9, T10, T11, T12, T13> SelectMany<T8, T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, T7, IEnumerable<(T8, T9, T10, T11, T12, T13)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }
    

    public IncrementalValuesProvider<T8, T9, T10, T11, T12, T13, T14> Select<T8, T9, T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, (T8, T9, T10, T11, T12, T13, T14)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValuesProvider<T8, T9, T10, T11, T12, T13, T14> Select<T8, T9, T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, T7, (T8, T9, T10, T11, T12, T13, T14)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }

    public IncrementalValuesProvider<T8, T9, T10, T11, T12, T13, T14> SelectMany<T8, T9, T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, IEnumerable<(T8, T9, T10, T11, T12, T13, T14)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValuesProvider<T8, T9, T10, T11, T12, T13, T14> SelectMany<T8, T9, T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, T7, IEnumerable<(T8, T9, T10, T11, T12, T13, T14)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }
    

    public IncrementalValuesProvider<T8, T9, T10, T11, T12, T13, T14, T15> Select<T8, T9, T10, T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, (T8, T9, T10, T11, T12, T13, T14, T15)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValuesProvider<T8, T9, T10, T11, T12, T13, T14, T15> Select<T8, T9, T10, T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, T7, (T8, T9, T10, T11, T12, T13, T14, T15)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }

    public IncrementalValuesProvider<T8, T9, T10, T11, T12, T13, T14, T15> SelectMany<T8, T9, T10, T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, IEnumerable<(T8, T9, T10, T11, T12, T13, T14, T15)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValuesProvider<T8, T9, T10, T11, T12, T13, T14, T15> SelectMany<T8, T9, T10, T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, T7, IEnumerable<(T8, T9, T10, T11, T12, T13, T14, T15)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }
    

    public IncrementalValuesProvider<T8, T9, T10, T11, T12, T13, T14, T15, T16> Select<T8, T9, T10, T11, T12, T13, T14, T15, T16>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, (T8, T9, T10, T11, T12, T13, T14, T15, T16)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValuesProvider<T8, T9, T10, T11, T12, T13, T14, T15, T16> Select<T8, T9, T10, T11, T12, T13, T14, T15, T16>(
        Func<T1, T2, T3, T4, T5, T6, T7, (T8, T9, T10, T11, T12, T13, T14, T15, T16)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }

    public IncrementalValuesProvider<T8, T9, T10, T11, T12, T13, T14, T15, T16> SelectMany<T8, T9, T10, T11, T12, T13, T14, T15, T16>(
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, IEnumerable<(T8, T9, T10, T11, T12, T13, T14, T15, T16)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    public IncrementalValuesProvider<T8, T9, T10, T11, T12, T13, T14, T15, T16> SelectMany<T8, T9, T10, T11, T12, T13, T14, T15, T16>(
        Func<T1, T2, T3, T4, T5, T6, T7, IEnumerable<(T8, T9, T10, T11, T12, T13, T14, T15, T16)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }
    
}
public readonly struct IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8>(IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8)> provider)
{
    private readonly IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8)> _provider = provider;

    public static implicit operator IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8>(IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8)> provider)
    {
        return new IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8>(provider);
    }

    public static implicit operator IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8)>(IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8> provider)
    {
        return provider._provider;
    }

    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8> WithComparer(IEqualityComparer<(T1, T2, T3, T4, T5, T6, T7, T8)> comparer)
    {
        return _provider.WithComparer(comparer);
    }

    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8> WithTrackingName(string name)
    {
        return _provider.WithTrackingName(name);
    }

    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8> Where(Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> predicate)
    {
        return IncrementalValueProviderExtensions.Where(_provider, tuple => predicate(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }
    public IncrementalValueProvider<ImmutableArray<(T1, T2, T3, T4, T5, T6, T7, T8)>> Collect()
    {
        return _provider.Collect();
    }

    public IncrementalValuesProvider<T9> Select<T9>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, T9> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValuesProvider<T9> Select<T9>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }

    public IncrementalValuesProvider<T9> SelectMany<T9>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, IEnumerable<T9>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValuesProvider<T9> SelectMany<T9>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, IEnumerable<T9>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> Combine<T9>(
        IncrementalValueProvider<T9> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Left.Item6, tuple.Left.Item7, tuple.Left.Item8, tuple.Right));
    }

    public IncrementalValuesProvider<T9, T10> Select<T9, T10>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, (T9, T10)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValuesProvider<T9, T10> Select<T9, T10>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, (T9, T10)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }

    public IncrementalValuesProvider<T9, T10> SelectMany<T9, T10>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, IEnumerable<(T9, T10)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValuesProvider<T9, T10> SelectMany<T9, T10>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, IEnumerable<(T9, T10)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Combine<T9, T10>(
        IncrementalValueProvider<(T9, T10)> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Left.Item6, tuple.Left.Item7, tuple.Left.Item8, tuple.Right.Item1, tuple.Right.Item2));
    }

    public IncrementalValuesProvider<T9, T10, T11> Select<T9, T10, T11>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, (T9, T10, T11)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValuesProvider<T9, T10, T11> Select<T9, T10, T11>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, (T9, T10, T11)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }

    public IncrementalValuesProvider<T9, T10, T11> SelectMany<T9, T10, T11>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, IEnumerable<(T9, T10, T11)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValuesProvider<T9, T10, T11> SelectMany<T9, T10, T11>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, IEnumerable<(T9, T10, T11)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }
    

    public IncrementalValuesProvider<T9, T10, T11, T12> Select<T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, (T9, T10, T11, T12)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValuesProvider<T9, T10, T11, T12> Select<T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, (T9, T10, T11, T12)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }

    public IncrementalValuesProvider<T9, T10, T11, T12> SelectMany<T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, IEnumerable<(T9, T10, T11, T12)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValuesProvider<T9, T10, T11, T12> SelectMany<T9, T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, IEnumerable<(T9, T10, T11, T12)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }
    

    public IncrementalValuesProvider<T9, T10, T11, T12, T13> Select<T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, (T9, T10, T11, T12, T13)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValuesProvider<T9, T10, T11, T12, T13> Select<T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, (T9, T10, T11, T12, T13)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }

    public IncrementalValuesProvider<T9, T10, T11, T12, T13> SelectMany<T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, IEnumerable<(T9, T10, T11, T12, T13)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValuesProvider<T9, T10, T11, T12, T13> SelectMany<T9, T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, IEnumerable<(T9, T10, T11, T12, T13)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }
    

    public IncrementalValuesProvider<T9, T10, T11, T12, T13, T14> Select<T9, T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, (T9, T10, T11, T12, T13, T14)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValuesProvider<T9, T10, T11, T12, T13, T14> Select<T9, T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, (T9, T10, T11, T12, T13, T14)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }

    public IncrementalValuesProvider<T9, T10, T11, T12, T13, T14> SelectMany<T9, T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, IEnumerable<(T9, T10, T11, T12, T13, T14)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValuesProvider<T9, T10, T11, T12, T13, T14> SelectMany<T9, T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, IEnumerable<(T9, T10, T11, T12, T13, T14)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }
    

    public IncrementalValuesProvider<T9, T10, T11, T12, T13, T14, T15> Select<T9, T10, T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, (T9, T10, T11, T12, T13, T14, T15)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValuesProvider<T9, T10, T11, T12, T13, T14, T15> Select<T9, T10, T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, (T9, T10, T11, T12, T13, T14, T15)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }

    public IncrementalValuesProvider<T9, T10, T11, T12, T13, T14, T15> SelectMany<T9, T10, T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, IEnumerable<(T9, T10, T11, T12, T13, T14, T15)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValuesProvider<T9, T10, T11, T12, T13, T14, T15> SelectMany<T9, T10, T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, IEnumerable<(T9, T10, T11, T12, T13, T14, T15)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }
    

    public IncrementalValuesProvider<T9, T10, T11, T12, T13, T14, T15, T16> Select<T9, T10, T11, T12, T13, T14, T15, T16>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, (T9, T10, T11, T12, T13, T14, T15, T16)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValuesProvider<T9, T10, T11, T12, T13, T14, T15, T16> Select<T9, T10, T11, T12, T13, T14, T15, T16>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, (T9, T10, T11, T12, T13, T14, T15, T16)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }

    public IncrementalValuesProvider<T9, T10, T11, T12, T13, T14, T15, T16> SelectMany<T9, T10, T11, T12, T13, T14, T15, T16>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, IEnumerable<(T9, T10, T11, T12, T13, T14, T15, T16)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValuesProvider<T9, T10, T11, T12, T13, T14, T15, T16> SelectMany<T9, T10, T11, T12, T13, T14, T15, T16>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, IEnumerable<(T9, T10, T11, T12, T13, T14, T15, T16)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }
    

    public IncrementalValuesProvider<T9, T10, T11, T12, T13, T14, T15, T16, T17> Select<T9, T10, T11, T12, T13, T14, T15, T16, T17>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, (T9, T10, T11, T12, T13, T14, T15, T16, T17)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValuesProvider<T9, T10, T11, T12, T13, T14, T15, T16, T17> Select<T9, T10, T11, T12, T13, T14, T15, T16, T17>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, (T9, T10, T11, T12, T13, T14, T15, T16, T17)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }

    public IncrementalValuesProvider<T9, T10, T11, T12, T13, T14, T15, T16, T17> SelectMany<T9, T10, T11, T12, T13, T14, T15, T16, T17>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, IEnumerable<(T9, T10, T11, T12, T13, T14, T15, T16, T17)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    public IncrementalValuesProvider<T9, T10, T11, T12, T13, T14, T15, T16, T17> SelectMany<T9, T10, T11, T12, T13, T14, T15, T16, T17>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, IEnumerable<(T9, T10, T11, T12, T13, T14, T15, T16, T17)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }
    
}
public readonly struct IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9>(IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> provider)
{
    private readonly IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> _provider = provider;

    public static implicit operator IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9>(IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> provider)
    {
        return new IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9>(provider);
    }

    public static implicit operator IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9)>(IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> provider)
    {
        return provider._provider;
    }

    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> WithComparer(IEqualityComparer<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> comparer)
    {
        return _provider.WithComparer(comparer);
    }

    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> WithTrackingName(string name)
    {
        return _provider.WithTrackingName(name);
    }

    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> Where(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> predicate)
    {
        return IncrementalValueProviderExtensions.Where(_provider, tuple => predicate(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }
    public IncrementalValueProvider<ImmutableArray<(T1, T2, T3, T4, T5, T6, T7, T8, T9)>> Collect()
    {
        return _provider.Collect();
    }

    public IncrementalValuesProvider<T10> Select<T10>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, T10> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValuesProvider<T10> Select<T10>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }

    public IncrementalValuesProvider<T10> SelectMany<T10>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, IEnumerable<T10>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValuesProvider<T10> SelectMany<T10>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IEnumerable<T10>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }
    
    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Combine<T10>(
        IncrementalValueProvider<T10> other)
    {
        return IncrementalValueProviderExtensions.Combine(_provider, other).Select((tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Left.Item6, tuple.Left.Item7, tuple.Left.Item8, tuple.Left.Item9, tuple.Right));
    }

    public IncrementalValuesProvider<T10, T11> Select<T10, T11>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, (T10, T11)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValuesProvider<T10, T11> Select<T10, T11>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, (T10, T11)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }

    public IncrementalValuesProvider<T10, T11> SelectMany<T10, T11>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, IEnumerable<(T10, T11)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValuesProvider<T10, T11> SelectMany<T10, T11>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IEnumerable<(T10, T11)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }
    

    public IncrementalValuesProvider<T10, T11, T12> Select<T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, (T10, T11, T12)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValuesProvider<T10, T11, T12> Select<T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, (T10, T11, T12)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }

    public IncrementalValuesProvider<T10, T11, T12> SelectMany<T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, IEnumerable<(T10, T11, T12)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValuesProvider<T10, T11, T12> SelectMany<T10, T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IEnumerable<(T10, T11, T12)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }
    

    public IncrementalValuesProvider<T10, T11, T12, T13> Select<T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, (T10, T11, T12, T13)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValuesProvider<T10, T11, T12, T13> Select<T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, (T10, T11, T12, T13)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }

    public IncrementalValuesProvider<T10, T11, T12, T13> SelectMany<T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, IEnumerable<(T10, T11, T12, T13)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValuesProvider<T10, T11, T12, T13> SelectMany<T10, T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IEnumerable<(T10, T11, T12, T13)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }
    

    public IncrementalValuesProvider<T10, T11, T12, T13, T14> Select<T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, (T10, T11, T12, T13, T14)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValuesProvider<T10, T11, T12, T13, T14> Select<T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, (T10, T11, T12, T13, T14)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }

    public IncrementalValuesProvider<T10, T11, T12, T13, T14> SelectMany<T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, IEnumerable<(T10, T11, T12, T13, T14)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValuesProvider<T10, T11, T12, T13, T14> SelectMany<T10, T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IEnumerable<(T10, T11, T12, T13, T14)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }
    

    public IncrementalValuesProvider<T10, T11, T12, T13, T14, T15> Select<T10, T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, (T10, T11, T12, T13, T14, T15)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValuesProvider<T10, T11, T12, T13, T14, T15> Select<T10, T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, (T10, T11, T12, T13, T14, T15)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }

    public IncrementalValuesProvider<T10, T11, T12, T13, T14, T15> SelectMany<T10, T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, IEnumerable<(T10, T11, T12, T13, T14, T15)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValuesProvider<T10, T11, T12, T13, T14, T15> SelectMany<T10, T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IEnumerable<(T10, T11, T12, T13, T14, T15)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }
    

    public IncrementalValuesProvider<T10, T11, T12, T13, T14, T15, T16> Select<T10, T11, T12, T13, T14, T15, T16>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, (T10, T11, T12, T13, T14, T15, T16)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValuesProvider<T10, T11, T12, T13, T14, T15, T16> Select<T10, T11, T12, T13, T14, T15, T16>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, (T10, T11, T12, T13, T14, T15, T16)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }

    public IncrementalValuesProvider<T10, T11, T12, T13, T14, T15, T16> SelectMany<T10, T11, T12, T13, T14, T15, T16>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, IEnumerable<(T10, T11, T12, T13, T14, T15, T16)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValuesProvider<T10, T11, T12, T13, T14, T15, T16> SelectMany<T10, T11, T12, T13, T14, T15, T16>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IEnumerable<(T10, T11, T12, T13, T14, T15, T16)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }
    

    public IncrementalValuesProvider<T10, T11, T12, T13, T14, T15, T16, T17> Select<T10, T11, T12, T13, T14, T15, T16, T17>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, (T10, T11, T12, T13, T14, T15, T16, T17)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValuesProvider<T10, T11, T12, T13, T14, T15, T16, T17> Select<T10, T11, T12, T13, T14, T15, T16, T17>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, (T10, T11, T12, T13, T14, T15, T16, T17)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }

    public IncrementalValuesProvider<T10, T11, T12, T13, T14, T15, T16, T17> SelectMany<T10, T11, T12, T13, T14, T15, T16, T17>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, IEnumerable<(T10, T11, T12, T13, T14, T15, T16, T17)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValuesProvider<T10, T11, T12, T13, T14, T15, T16, T17> SelectMany<T10, T11, T12, T13, T14, T15, T16, T17>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IEnumerable<(T10, T11, T12, T13, T14, T15, T16, T17)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }
    

    public IncrementalValuesProvider<T10, T11, T12, T13, T14, T15, T16, T17, T18> Select<T10, T11, T12, T13, T14, T15, T16, T17, T18>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, (T10, T11, T12, T13, T14, T15, T16, T17, T18)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValuesProvider<T10, T11, T12, T13, T14, T15, T16, T17, T18> Select<T10, T11, T12, T13, T14, T15, T16, T17, T18>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, (T10, T11, T12, T13, T14, T15, T16, T17, T18)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }

    public IncrementalValuesProvider<T10, T11, T12, T13, T14, T15, T16, T17, T18> SelectMany<T10, T11, T12, T13, T14, T15, T16, T17, T18>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, IEnumerable<(T10, T11, T12, T13, T14, T15, T16, T17, T18)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    public IncrementalValuesProvider<T10, T11, T12, T13, T14, T15, T16, T17, T18> SelectMany<T10, T11, T12, T13, T14, T15, T16, T17, T18>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IEnumerable<(T10, T11, T12, T13, T14, T15, T16, T17, T18)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }
    
}
public readonly struct IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> provider)
{
    private readonly IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> _provider = provider;

    public static implicit operator IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> provider)
    {
        return new IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(provider);
    }

    public static implicit operator IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)>(IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> provider)
    {
        return provider._provider;
    }

    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> WithComparer(IEqualityComparer<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> comparer)
    {
        return _provider.WithComparer(comparer);
    }

    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> WithTrackingName(string name)
    {
        return _provider.WithTrackingName(name);
    }

    public IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Where(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> predicate)
    {
        return IncrementalValueProviderExtensions.Where(_provider, tuple => predicate(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }
    public IncrementalValueProvider<ImmutableArray<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)>> Collect()
    {
        return _provider.Collect();
    }

    public IncrementalValuesProvider<T11> Select<T11>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, T11> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValuesProvider<T11> Select<T11>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }

    public IncrementalValuesProvider<T11> SelectMany<T11>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, IEnumerable<T11>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValuesProvider<T11> SelectMany<T11>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IEnumerable<T11>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }
    

    public IncrementalValuesProvider<T11, T12> Select<T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, (T11, T12)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValuesProvider<T11, T12> Select<T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, (T11, T12)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }

    public IncrementalValuesProvider<T11, T12> SelectMany<T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, IEnumerable<(T11, T12)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValuesProvider<T11, T12> SelectMany<T11, T12>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IEnumerable<(T11, T12)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }
    

    public IncrementalValuesProvider<T11, T12, T13> Select<T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, (T11, T12, T13)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValuesProvider<T11, T12, T13> Select<T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, (T11, T12, T13)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }

    public IncrementalValuesProvider<T11, T12, T13> SelectMany<T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, IEnumerable<(T11, T12, T13)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValuesProvider<T11, T12, T13> SelectMany<T11, T12, T13>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IEnumerable<(T11, T12, T13)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }
    

    public IncrementalValuesProvider<T11, T12, T13, T14> Select<T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, (T11, T12, T13, T14)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValuesProvider<T11, T12, T13, T14> Select<T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, (T11, T12, T13, T14)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }

    public IncrementalValuesProvider<T11, T12, T13, T14> SelectMany<T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, IEnumerable<(T11, T12, T13, T14)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValuesProvider<T11, T12, T13, T14> SelectMany<T11, T12, T13, T14>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IEnumerable<(T11, T12, T13, T14)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }
    

    public IncrementalValuesProvider<T11, T12, T13, T14, T15> Select<T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, (T11, T12, T13, T14, T15)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValuesProvider<T11, T12, T13, T14, T15> Select<T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, (T11, T12, T13, T14, T15)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }

    public IncrementalValuesProvider<T11, T12, T13, T14, T15> SelectMany<T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, IEnumerable<(T11, T12, T13, T14, T15)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValuesProvider<T11, T12, T13, T14, T15> SelectMany<T11, T12, T13, T14, T15>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IEnumerable<(T11, T12, T13, T14, T15)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }
    

    public IncrementalValuesProvider<T11, T12, T13, T14, T15, T16> Select<T11, T12, T13, T14, T15, T16>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, (T11, T12, T13, T14, T15, T16)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValuesProvider<T11, T12, T13, T14, T15, T16> Select<T11, T12, T13, T14, T15, T16>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, (T11, T12, T13, T14, T15, T16)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }

    public IncrementalValuesProvider<T11, T12, T13, T14, T15, T16> SelectMany<T11, T12, T13, T14, T15, T16>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, IEnumerable<(T11, T12, T13, T14, T15, T16)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValuesProvider<T11, T12, T13, T14, T15, T16> SelectMany<T11, T12, T13, T14, T15, T16>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IEnumerable<(T11, T12, T13, T14, T15, T16)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }
    

    public IncrementalValuesProvider<T11, T12, T13, T14, T15, T16, T17> Select<T11, T12, T13, T14, T15, T16, T17>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, (T11, T12, T13, T14, T15, T16, T17)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValuesProvider<T11, T12, T13, T14, T15, T16, T17> Select<T11, T12, T13, T14, T15, T16, T17>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, (T11, T12, T13, T14, T15, T16, T17)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }

    public IncrementalValuesProvider<T11, T12, T13, T14, T15, T16, T17> SelectMany<T11, T12, T13, T14, T15, T16, T17>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, IEnumerable<(T11, T12, T13, T14, T15, T16, T17)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValuesProvider<T11, T12, T13, T14, T15, T16, T17> SelectMany<T11, T12, T13, T14, T15, T16, T17>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IEnumerable<(T11, T12, T13, T14, T15, T16, T17)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }
    

    public IncrementalValuesProvider<T11, T12, T13, T14, T15, T16, T17, T18> Select<T11, T12, T13, T14, T15, T16, T17, T18>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, (T11, T12, T13, T14, T15, T16, T17, T18)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValuesProvider<T11, T12, T13, T14, T15, T16, T17, T18> Select<T11, T12, T13, T14, T15, T16, T17, T18>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, (T11, T12, T13, T14, T15, T16, T17, T18)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }

    public IncrementalValuesProvider<T11, T12, T13, T14, T15, T16, T17, T18> SelectMany<T11, T12, T13, T14, T15, T16, T17, T18>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, IEnumerable<(T11, T12, T13, T14, T15, T16, T17, T18)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValuesProvider<T11, T12, T13, T14, T15, T16, T17, T18> SelectMany<T11, T12, T13, T14, T15, T16, T17, T18>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IEnumerable<(T11, T12, T13, T14, T15, T16, T17, T18)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }
    

    public IncrementalValuesProvider<T11, T12, T13, T14, T15, T16, T17, T18, T19> Select<T11, T12, T13, T14, T15, T16, T17, T18, T19>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, (T11, T12, T13, T14, T15, T16, T17, T18, T19)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValuesProvider<T11, T12, T13, T14, T15, T16, T17, T18, T19> Select<T11, T12, T13, T14, T15, T16, T17, T18, T19>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, (T11, T12, T13, T14, T15, T16, T17, T18, T19)> selector)
    {
        return IncrementalValueProviderExtensions.Select(_provider, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }

    public IncrementalValuesProvider<T11, T12, T13, T14, T15, T16, T17, T18, T19> SelectMany<T11, T12, T13, T14, T15, T16, T17, T18, T19>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, IEnumerable<(T11, T12, T13, T14, T15, T16, T17, T18, T19)>> selector)
    {
        return _provider.SelectMany((tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    public IncrementalValuesProvider<T11, T12, T13, T14, T15, T16, T17, T18, T19> SelectMany<T11, T12, T13, T14, T15, T16, T17, T18, T19>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IEnumerable<(T11, T12, T13, T14, T15, T16, T17, T18, T19)>> selector)
    {
        return _provider.SelectMany((tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }
    
}
