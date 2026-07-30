

using System.Runtime.CompilerServices;
using System.Collections.Immutable;
using System.Collections.Generic;
using SourceKit.Tools;
using SourceKit;

// ReSharper disable once CheckNamespace
namespace Microsoft.CodeAnalysis;

public static partial class SourceKitValuesProviderExtensionsInternal
{


    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1> Select<TFrom, T1>(
        this IncrementalValuesProvider<TFrom> source,
        Func<TFrom, T1> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (from, _) => selector(from));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<T1> Select<TFrom, T1>(
        this IncrementalValueProvider<TFrom> source,
        Func<TFrom, T1> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (from, _) => selector(from));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1> SelectMany<TFrom, T1>(
        this IncrementalValuesProvider<TFrom> first, 
        Func<TFrom, IEnumerable<T1>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1> SelectMany<TFrom, T1>(
        this IncrementalValuesProvider<TFrom> first, 
        Func<TFrom, ImmutableArray<T1>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1> SelectMany<TFrom, T1>(
        this IncrementalValueProvider<TFrom> first, 
        Func<TFrom, IEnumerable<T1>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1> SelectMany<TFrom, T1>(
        this IncrementalValueProvider<TFrom> first, 
        Func<TFrom, ImmutableArray<T1>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    public static IncrementalValuesProvider<T1> WithComparer<T1, TComparand>(
        this IncrementalValuesProvider<T1> provider,
        Func<T1, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<T1, TComparand>(tuple => selector(tuple)));
    }

    public static IncrementalValueProvider<T1> WithComparer<T1, TComparand>(
        this IncrementalValueProvider<T1> provider, 
        Func<T1, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<T1, TComparand>(tuple => selector(tuple)));
    }

    public static IncrementalValuesProvider<IncrementalResult<T1>> WithResultComparer<T1, TComparand>(
        this IncrementalValuesProvider<IncrementalResult<T1>> provider,
        Func<T1, TComparand> selector)
    {
        return provider.WithComparer(new IncrementalResultComparer<T1, TComparand>(tuple => selector(tuple)));
    }

    public static IncrementalValuesProvider<T1, TAdded> CombineAndUnwrap<T1, TAdded>(
        this IncrementalValuesProvider<T1> source,
        IncrementalValueProvider<IncrementalResult<TAdded>> other,
        IncrementalGeneratorInitializationContext context)
    {
        return IncrementalValueProviderExtensions.Combine(source, other).Select(static (tuple, _) => tuple.Right.Map(value => (tuple.Left, value))).Unwrap(context);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2> Select<TFrom, T1, T2>(
        this IncrementalValuesProvider<TFrom> source,
        Func<TFrom, (T1, T2)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (from, _) => selector(from));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<T1, T2> Select<TFrom, T1, T2>(
        this IncrementalValueProvider<TFrom> source,
        Func<TFrom, (T1, T2)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (from, _) => selector(from));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2> SelectMany<TFrom, T1, T2>(
        this IncrementalValuesProvider<TFrom> first, 
        Func<TFrom, IEnumerable<(T1, T2)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2> SelectMany<TFrom, T1, T2>(
        this IncrementalValuesProvider<TFrom> first, 
        Func<TFrom, ImmutableArray<(T1, T2)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2> SelectMany<TFrom, T1, T2>(
        this IncrementalValueProvider<TFrom> first, 
        Func<TFrom, IEnumerable<(T1, T2)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2> SelectMany<TFrom, T1, T2>(
        this IncrementalValueProvider<TFrom> first, 
        Func<TFrom, ImmutableArray<(T1, T2)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    public static IncrementalValuesProvider<T1, T2> WithComparer<T1, T2, TComparand>(
        this IncrementalValuesProvider<T1, T2> provider,
        Func<T1, T2, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2), TComparand>(tuple => selector(tuple.Item1, tuple.Item2)));
    }

    public static IncrementalValueProvider<T1, T2> WithComparer<T1, T2, TComparand>(
        this IncrementalValueProvider<T1, T2> provider, 
        Func<T1, T2, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2), TComparand>(tuple => selector(tuple.Item1, tuple.Item2)));
    }

    public static IncrementalValuesProvider<IncrementalResult<(T1, T2)>> WithResultComparer<T1, T2, TComparand>(
        this IncrementalValuesProvider<IncrementalResult<(T1, T2)>> provider,
        Func<T1, T2, TComparand> selector)
    {
        return provider.WithComparer(new IncrementalResultComparer<(T1, T2), TComparand>(tuple => selector(tuple.Item1, tuple.Item2)));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, TAdded> Combine<T1, T2, TAdded>(
        this IncrementalValuesProvider<(T1, T2)> source,
        IncrementalValueProvider<TAdded> other)
    {
        return IncrementalValueProviderExtensions.Combine(source, other).Select(static (tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Right));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<T1, T2, TAdded> Combine<T1, T2, TAdded>(
        this IncrementalValueProvider<(T1, T2)> source,
        IncrementalValueProvider<TAdded> other)
    {
        return IncrementalValueProviderExtensions.Combine(source, other).Select(static (tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Right));
    }

    public static IncrementalValuesProvider<T1, T2, TAdded> CombineAndUnwrap<T1, T2, TAdded>(
        this IncrementalValuesProvider<(T1, T2)> source,
        IncrementalValueProvider<IncrementalResult<TAdded>> other,
        IncrementalGeneratorInitializationContext context)
    {
        return IncrementalValueProviderExtensions.Combine(source, other).Select(static (tuple, _) => tuple.Right.Map(value => (tuple.Left.Item1, tuple.Left.Item2, value))).Unwrap(context);
    }
    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, TTo>(
        this IncrementalValuesProvider<(T1, T2)> first, 
        Func<T1, T2, IEnumerable<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, TTo>(
        this IncrementalValuesProvider<(T1, T2)> first, 
        Func<T1, T2, ImmutableArray<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, TTo>(
        this IncrementalValueProvider<(T1, T2)> first, 
        Func<T1, T2, IEnumerable<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, TTo>(
        this IncrementalValueProvider<(T1, T2)> first, 
        Func<T1, T2, ImmutableArray<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2));
    }

    public static IncrementalValuesProvider<T1, T2> WithComparer<T1, T2, TComparand>(
        this IncrementalValuesProvider<(T1, T2)> provider,
        Func<T1, T2, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2), TComparand>(tuple => selector(tuple.Item1, tuple.Item2)));
    }

    public static IncrementalValueProvider<T1, T2> WithComparer<T1, T2, TComparand>(
        this IncrementalValueProvider<(T1, T2)> provider, 
        Func<T1, T2, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2), TComparand>(tuple => selector(tuple.Item1, tuple.Item2)));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2> Select<TFrom, T1, T2>(
        this IncrementalValuesProvider<TFrom> source,
        Func<TFrom, CancellationToken, (T1, T2)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, selector);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<T1, T2> Select<TFrom, T1, T2>(
        this IncrementalValueProvider<TFrom> source,
        Func<TFrom, CancellationToken, (T1, T2)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, selector);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> Select<T1, T2, TTo>(
        this IncrementalValuesProvider<(T1, T2)> source,
        Func<T1, T2, CancellationToken, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> Select<T1, T2, TTo>(
        this IncrementalValuesProvider<(T1, T2)> source,
        Func<T1, T2, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, _) => selector(tuple.Item1, tuple.Item2));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<TTo> Select<T1, T2, TTo>(
        this IncrementalValueProvider<(T1, T2)> source,
        Func<T1, T2, CancellationToken, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, ct) => selector(tuple.Item1, tuple.Item2, ct));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<TTo> Select<T1, T2, TTo>(
        this IncrementalValueProvider<(T1, T2)> source,
        Func<T1, T2, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, _) => selector(tuple.Item1, tuple.Item2));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2> SelectMany<TFrom, T1, T2>(
        this IncrementalValuesProvider<TFrom> first, 
        Func<TFrom, CancellationToken, IEnumerable<(T1, T2)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, selector);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2> Unwrap<T1, T2>(
        this IncrementalValuesProvider<SourceKit.IncrementalResult<(T1, T2)>> provider,
        IncrementalGeneratorInitializationContext context)
    {
        return SourceKitValuesProviderExtensions.Unwrap(provider, context);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2> Unwrap<T1, T2>(
        this IncrementalValueProvider<SourceKit.IncrementalResult<(T1, T2)>> provider,
        IncrementalGeneratorInitializationContext context)
    {
        return SourceKitValuesProviderExtensions.Unwrap(provider, context);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2> Where<T1, T2>(
        this IncrementalValuesProvider<(T1, T2)> provider,
        Func<T1, T2, bool> predicate)
    {
        return ((IncrementalValuesProvider<T1, T2>)provider).Where(predicate);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2> Where<T1, T2>(
        this IncrementalValueProvider<(T1, T2)> provider,
        Func<T1, T2, bool> predicate)
    {
        return ((IncrementalValueProvider<T1, T2>)provider).Where(predicate);
    }

    public static void RegisterSourceOutput<T1, T2>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValueProvider<T1, T2> provider,
        Action<SourceProductionContext, T1, T2> action)
    {
        context.RegisterSourceOutput(
            (IncrementalValueProvider<(T1, T2)>)provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2));
    }

    public static void RegisterSourceOutput<T1, T2>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValueProvider<(T1, T2)> provider,
        Action<SourceProductionContext, T1, T2> action)
    {
        context.RegisterSourceOutput(
            provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2));
    }

    public static void RegisterSourceOutput<T1, T2>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValuesProvider<T1, T2> provider,
        Action<SourceProductionContext, T1, T2> action)
    {
        context.RegisterSourceOutput(
            (IncrementalValuesProvider<(T1, T2)>)provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2));
    }

    public static void RegisterSourceOutput<T1, T2>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValuesProvider<(T1, T2)> provider,
        Action<SourceProductionContext, T1, T2> action)
    {
        context.RegisterSourceOutput(
            provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3> Select<TFrom, T1, T2, T3>(
        this IncrementalValuesProvider<TFrom> source,
        Func<TFrom, (T1, T2, T3)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (from, _) => selector(from));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<T1, T2, T3> Select<TFrom, T1, T2, T3>(
        this IncrementalValueProvider<TFrom> source,
        Func<TFrom, (T1, T2, T3)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (from, _) => selector(from));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3> SelectMany<TFrom, T1, T2, T3>(
        this IncrementalValuesProvider<TFrom> first, 
        Func<TFrom, IEnumerable<(T1, T2, T3)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3> SelectMany<TFrom, T1, T2, T3>(
        this IncrementalValuesProvider<TFrom> first, 
        Func<TFrom, ImmutableArray<(T1, T2, T3)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3> SelectMany<TFrom, T1, T2, T3>(
        this IncrementalValueProvider<TFrom> first, 
        Func<TFrom, IEnumerable<(T1, T2, T3)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3> SelectMany<TFrom, T1, T2, T3>(
        this IncrementalValueProvider<TFrom> first, 
        Func<TFrom, ImmutableArray<(T1, T2, T3)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    public static IncrementalValuesProvider<T1, T2, T3> WithComparer<T1, T2, T3, TComparand>(
        this IncrementalValuesProvider<T1, T2, T3> provider,
        Func<T1, T2, T3, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2, T3), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3)));
    }

    public static IncrementalValueProvider<T1, T2, T3> WithComparer<T1, T2, T3, TComparand>(
        this IncrementalValueProvider<T1, T2, T3> provider, 
        Func<T1, T2, T3, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2, T3), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3)));
    }

    public static IncrementalValuesProvider<IncrementalResult<(T1, T2, T3)>> WithResultComparer<T1, T2, T3, TComparand>(
        this IncrementalValuesProvider<IncrementalResult<(T1, T2, T3)>> provider,
        Func<T1, T2, T3, TComparand> selector)
    {
        return provider.WithComparer(new IncrementalResultComparer<(T1, T2, T3), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3)));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, TAdded> Combine<T1, T2, T3, TAdded>(
        this IncrementalValuesProvider<(T1, T2, T3)> source,
        IncrementalValueProvider<TAdded> other)
    {
        return IncrementalValueProviderExtensions.Combine(source, other).Select(static (tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Right));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<T1, T2, T3, TAdded> Combine<T1, T2, T3, TAdded>(
        this IncrementalValueProvider<(T1, T2, T3)> source,
        IncrementalValueProvider<TAdded> other)
    {
        return IncrementalValueProviderExtensions.Combine(source, other).Select(static (tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Right));
    }

    public static IncrementalValuesProvider<T1, T2, T3, TAdded> CombineAndUnwrap<T1, T2, T3, TAdded>(
        this IncrementalValuesProvider<(T1, T2, T3)> source,
        IncrementalValueProvider<IncrementalResult<TAdded>> other,
        IncrementalGeneratorInitializationContext context)
    {
        return IncrementalValueProviderExtensions.Combine(source, other).Select(static (tuple, _) => tuple.Right.Map(value => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, value))).Unwrap(context);
    }
    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, T3, TTo>(
        this IncrementalValuesProvider<(T1, T2, T3)> first, 
        Func<T1, T2, T3, IEnumerable<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, T3, TTo>(
        this IncrementalValuesProvider<(T1, T2, T3)> first, 
        Func<T1, T2, T3, ImmutableArray<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, T3, TTo>(
        this IncrementalValueProvider<(T1, T2, T3)> first, 
        Func<T1, T2, T3, IEnumerable<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, T3, TTo>(
        this IncrementalValueProvider<(T1, T2, T3)> first, 
        Func<T1, T2, T3, ImmutableArray<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }

    public static IncrementalValuesProvider<T1, T2, T3> WithComparer<T1, T2, T3, TComparand>(
        this IncrementalValuesProvider<(T1, T2, T3)> provider,
        Func<T1, T2, T3, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2, T3), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3)));
    }

    public static IncrementalValueProvider<T1, T2, T3> WithComparer<T1, T2, T3, TComparand>(
        this IncrementalValueProvider<(T1, T2, T3)> provider, 
        Func<T1, T2, T3, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2, T3), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3)));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3> Select<TFrom, T1, T2, T3>(
        this IncrementalValuesProvider<TFrom> source,
        Func<TFrom, CancellationToken, (T1, T2, T3)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, selector);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<T1, T2, T3> Select<TFrom, T1, T2, T3>(
        this IncrementalValueProvider<TFrom> source,
        Func<TFrom, CancellationToken, (T1, T2, T3)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, selector);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> Select<T1, T2, T3, TTo>(
        this IncrementalValuesProvider<(T1, T2, T3)> source,
        Func<T1, T2, T3, CancellationToken, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> Select<T1, T2, T3, TTo>(
        this IncrementalValuesProvider<(T1, T2, T3)> source,
        Func<T1, T2, T3, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<TTo> Select<T1, T2, T3, TTo>(
        this IncrementalValueProvider<(T1, T2, T3)> source,
        Func<T1, T2, T3, CancellationToken, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, ct));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<TTo> Select<T1, T2, T3, TTo>(
        this IncrementalValueProvider<(T1, T2, T3)> source,
        Func<T1, T2, T3, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3> SelectMany<TFrom, T1, T2, T3>(
        this IncrementalValuesProvider<TFrom> first, 
        Func<TFrom, CancellationToken, IEnumerable<(T1, T2, T3)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, selector);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3> Unwrap<T1, T2, T3>(
        this IncrementalValuesProvider<SourceKit.IncrementalResult<(T1, T2, T3)>> provider,
        IncrementalGeneratorInitializationContext context)
    {
        return SourceKitValuesProviderExtensions.Unwrap(provider, context);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3> Unwrap<T1, T2, T3>(
        this IncrementalValueProvider<SourceKit.IncrementalResult<(T1, T2, T3)>> provider,
        IncrementalGeneratorInitializationContext context)
    {
        return SourceKitValuesProviderExtensions.Unwrap(provider, context);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3> Where<T1, T2, T3>(
        this IncrementalValuesProvider<(T1, T2, T3)> provider,
        Func<T1, T2, T3, bool> predicate)
    {
        return ((IncrementalValuesProvider<T1, T2, T3>)provider).Where(predicate);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3> Where<T1, T2, T3>(
        this IncrementalValueProvider<(T1, T2, T3)> provider,
        Func<T1, T2, T3, bool> predicate)
    {
        return ((IncrementalValueProvider<T1, T2, T3>)provider).Where(predicate);
    }

    public static void RegisterSourceOutput<T1, T2, T3>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValueProvider<T1, T2, T3> provider,
        Action<SourceProductionContext, T1, T2, T3> action)
    {
        context.RegisterSourceOutput(
            (IncrementalValueProvider<(T1, T2, T3)>)provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2, tuple.Item3));
    }

    public static void RegisterSourceOutput<T1, T2, T3>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValueProvider<(T1, T2, T3)> provider,
        Action<SourceProductionContext, T1, T2, T3> action)
    {
        context.RegisterSourceOutput(
            provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2, tuple.Item3));
    }

    public static void RegisterSourceOutput<T1, T2, T3>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValuesProvider<T1, T2, T3> provider,
        Action<SourceProductionContext, T1, T2, T3> action)
    {
        context.RegisterSourceOutput(
            (IncrementalValuesProvider<(T1, T2, T3)>)provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2, tuple.Item3));
    }

    public static void RegisterSourceOutput<T1, T2, T3>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValuesProvider<(T1, T2, T3)> provider,
        Action<SourceProductionContext, T1, T2, T3> action)
    {
        context.RegisterSourceOutput(
            provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2, tuple.Item3));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4> Select<TFrom, T1, T2, T3, T4>(
        this IncrementalValuesProvider<TFrom> source,
        Func<TFrom, (T1, T2, T3, T4)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (from, _) => selector(from));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<T1, T2, T3, T4> Select<TFrom, T1, T2, T3, T4>(
        this IncrementalValueProvider<TFrom> source,
        Func<TFrom, (T1, T2, T3, T4)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (from, _) => selector(from));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4> SelectMany<TFrom, T1, T2, T3, T4>(
        this IncrementalValuesProvider<TFrom> first, 
        Func<TFrom, IEnumerable<(T1, T2, T3, T4)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4> SelectMany<TFrom, T1, T2, T3, T4>(
        this IncrementalValuesProvider<TFrom> first, 
        Func<TFrom, ImmutableArray<(T1, T2, T3, T4)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4> SelectMany<TFrom, T1, T2, T3, T4>(
        this IncrementalValueProvider<TFrom> first, 
        Func<TFrom, IEnumerable<(T1, T2, T3, T4)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4> SelectMany<TFrom, T1, T2, T3, T4>(
        this IncrementalValueProvider<TFrom> first, 
        Func<TFrom, ImmutableArray<(T1, T2, T3, T4)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    public static IncrementalValuesProvider<T1, T2, T3, T4> WithComparer<T1, T2, T3, T4, TComparand>(
        this IncrementalValuesProvider<T1, T2, T3, T4> provider,
        Func<T1, T2, T3, T4, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2, T3, T4), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4)));
    }

    public static IncrementalValueProvider<T1, T2, T3, T4> WithComparer<T1, T2, T3, T4, TComparand>(
        this IncrementalValueProvider<T1, T2, T3, T4> provider, 
        Func<T1, T2, T3, T4, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2, T3, T4), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4)));
    }

    public static IncrementalValuesProvider<IncrementalResult<(T1, T2, T3, T4)>> WithResultComparer<T1, T2, T3, T4, TComparand>(
        this IncrementalValuesProvider<IncrementalResult<(T1, T2, T3, T4)>> provider,
        Func<T1, T2, T3, T4, TComparand> selector)
    {
        return provider.WithComparer(new IncrementalResultComparer<(T1, T2, T3, T4), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4)));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, TAdded> Combine<T1, T2, T3, T4, TAdded>(
        this IncrementalValuesProvider<(T1, T2, T3, T4)> source,
        IncrementalValueProvider<TAdded> other)
    {
        return IncrementalValueProviderExtensions.Combine(source, other).Select(static (tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Right));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<T1, T2, T3, T4, TAdded> Combine<T1, T2, T3, T4, TAdded>(
        this IncrementalValueProvider<(T1, T2, T3, T4)> source,
        IncrementalValueProvider<TAdded> other)
    {
        return IncrementalValueProviderExtensions.Combine(source, other).Select(static (tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Right));
    }

    public static IncrementalValuesProvider<T1, T2, T3, T4, TAdded> CombineAndUnwrap<T1, T2, T3, T4, TAdded>(
        this IncrementalValuesProvider<(T1, T2, T3, T4)> source,
        IncrementalValueProvider<IncrementalResult<TAdded>> other,
        IncrementalGeneratorInitializationContext context)
    {
        return IncrementalValueProviderExtensions.Combine(source, other).Select(static (tuple, _) => tuple.Right.Map(value => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, value))).Unwrap(context);
    }
    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, T3, T4, TTo>(
        this IncrementalValuesProvider<(T1, T2, T3, T4)> first, 
        Func<T1, T2, T3, T4, IEnumerable<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, T3, T4, TTo>(
        this IncrementalValuesProvider<(T1, T2, T3, T4)> first, 
        Func<T1, T2, T3, T4, ImmutableArray<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, T3, T4, TTo>(
        this IncrementalValueProvider<(T1, T2, T3, T4)> first, 
        Func<T1, T2, T3, T4, IEnumerable<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, T3, T4, TTo>(
        this IncrementalValueProvider<(T1, T2, T3, T4)> first, 
        Func<T1, T2, T3, T4, ImmutableArray<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }

    public static IncrementalValuesProvider<T1, T2, T3, T4> WithComparer<T1, T2, T3, T4, TComparand>(
        this IncrementalValuesProvider<(T1, T2, T3, T4)> provider,
        Func<T1, T2, T3, T4, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2, T3, T4), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4)));
    }

    public static IncrementalValueProvider<T1, T2, T3, T4> WithComparer<T1, T2, T3, T4, TComparand>(
        this IncrementalValueProvider<(T1, T2, T3, T4)> provider, 
        Func<T1, T2, T3, T4, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2, T3, T4), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4)));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4> Select<TFrom, T1, T2, T3, T4>(
        this IncrementalValuesProvider<TFrom> source,
        Func<TFrom, CancellationToken, (T1, T2, T3, T4)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, selector);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<T1, T2, T3, T4> Select<TFrom, T1, T2, T3, T4>(
        this IncrementalValueProvider<TFrom> source,
        Func<TFrom, CancellationToken, (T1, T2, T3, T4)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, selector);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> Select<T1, T2, T3, T4, TTo>(
        this IncrementalValuesProvider<(T1, T2, T3, T4)> source,
        Func<T1, T2, T3, T4, CancellationToken, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> Select<T1, T2, T3, T4, TTo>(
        this IncrementalValuesProvider<(T1, T2, T3, T4)> source,
        Func<T1, T2, T3, T4, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<TTo> Select<T1, T2, T3, T4, TTo>(
        this IncrementalValueProvider<(T1, T2, T3, T4)> source,
        Func<T1, T2, T3, T4, CancellationToken, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, ct));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<TTo> Select<T1, T2, T3, T4, TTo>(
        this IncrementalValueProvider<(T1, T2, T3, T4)> source,
        Func<T1, T2, T3, T4, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4> SelectMany<TFrom, T1, T2, T3, T4>(
        this IncrementalValuesProvider<TFrom> first, 
        Func<TFrom, CancellationToken, IEnumerable<(T1, T2, T3, T4)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, selector);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4> Unwrap<T1, T2, T3, T4>(
        this IncrementalValuesProvider<SourceKit.IncrementalResult<(T1, T2, T3, T4)>> provider,
        IncrementalGeneratorInitializationContext context)
    {
        return SourceKitValuesProviderExtensions.Unwrap(provider, context);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4> Unwrap<T1, T2, T3, T4>(
        this IncrementalValueProvider<SourceKit.IncrementalResult<(T1, T2, T3, T4)>> provider,
        IncrementalGeneratorInitializationContext context)
    {
        return SourceKitValuesProviderExtensions.Unwrap(provider, context);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4> Where<T1, T2, T3, T4>(
        this IncrementalValuesProvider<(T1, T2, T3, T4)> provider,
        Func<T1, T2, T3, T4, bool> predicate)
    {
        return ((IncrementalValuesProvider<T1, T2, T3, T4>)provider).Where(predicate);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4> Where<T1, T2, T3, T4>(
        this IncrementalValueProvider<(T1, T2, T3, T4)> provider,
        Func<T1, T2, T3, T4, bool> predicate)
    {
        return ((IncrementalValueProvider<T1, T2, T3, T4>)provider).Where(predicate);
    }

    public static void RegisterSourceOutput<T1, T2, T3, T4>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValueProvider<T1, T2, T3, T4> provider,
        Action<SourceProductionContext, T1, T2, T3, T4> action)
    {
        context.RegisterSourceOutput(
            (IncrementalValueProvider<(T1, T2, T3, T4)>)provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }

    public static void RegisterSourceOutput<T1, T2, T3, T4>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValueProvider<(T1, T2, T3, T4)> provider,
        Action<SourceProductionContext, T1, T2, T3, T4> action)
    {
        context.RegisterSourceOutput(
            provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }

    public static void RegisterSourceOutput<T1, T2, T3, T4>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValuesProvider<T1, T2, T3, T4> provider,
        Action<SourceProductionContext, T1, T2, T3, T4> action)
    {
        context.RegisterSourceOutput(
            (IncrementalValuesProvider<(T1, T2, T3, T4)>)provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }

    public static void RegisterSourceOutput<T1, T2, T3, T4>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValuesProvider<(T1, T2, T3, T4)> provider,
        Action<SourceProductionContext, T1, T2, T3, T4> action)
    {
        context.RegisterSourceOutput(
            provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5> Select<TFrom, T1, T2, T3, T4, T5>(
        this IncrementalValuesProvider<TFrom> source,
        Func<TFrom, (T1, T2, T3, T4, T5)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (from, _) => selector(from));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<T1, T2, T3, T4, T5> Select<TFrom, T1, T2, T3, T4, T5>(
        this IncrementalValueProvider<TFrom> source,
        Func<TFrom, (T1, T2, T3, T4, T5)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (from, _) => selector(from));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5> SelectMany<TFrom, T1, T2, T3, T4, T5>(
        this IncrementalValuesProvider<TFrom> first, 
        Func<TFrom, IEnumerable<(T1, T2, T3, T4, T5)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5> SelectMany<TFrom, T1, T2, T3, T4, T5>(
        this IncrementalValuesProvider<TFrom> first, 
        Func<TFrom, ImmutableArray<(T1, T2, T3, T4, T5)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5> SelectMany<TFrom, T1, T2, T3, T4, T5>(
        this IncrementalValueProvider<TFrom> first, 
        Func<TFrom, IEnumerable<(T1, T2, T3, T4, T5)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5> SelectMany<TFrom, T1, T2, T3, T4, T5>(
        this IncrementalValueProvider<TFrom> first, 
        Func<TFrom, ImmutableArray<(T1, T2, T3, T4, T5)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    public static IncrementalValuesProvider<T1, T2, T3, T4, T5> WithComparer<T1, T2, T3, T4, T5, TComparand>(
        this IncrementalValuesProvider<T1, T2, T3, T4, T5> provider,
        Func<T1, T2, T3, T4, T5, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2, T3, T4, T5), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5)));
    }

    public static IncrementalValueProvider<T1, T2, T3, T4, T5> WithComparer<T1, T2, T3, T4, T5, TComparand>(
        this IncrementalValueProvider<T1, T2, T3, T4, T5> provider, 
        Func<T1, T2, T3, T4, T5, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2, T3, T4, T5), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5)));
    }

    public static IncrementalValuesProvider<IncrementalResult<(T1, T2, T3, T4, T5)>> WithResultComparer<T1, T2, T3, T4, T5, TComparand>(
        this IncrementalValuesProvider<IncrementalResult<(T1, T2, T3, T4, T5)>> provider,
        Func<T1, T2, T3, T4, T5, TComparand> selector)
    {
        return provider.WithComparer(new IncrementalResultComparer<(T1, T2, T3, T4, T5), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5)));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, TAdded> Combine<T1, T2, T3, T4, T5, TAdded>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5)> source,
        IncrementalValueProvider<TAdded> other)
    {
        return IncrementalValueProviderExtensions.Combine(source, other).Select(static (tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Right));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<T1, T2, T3, T4, T5, TAdded> Combine<T1, T2, T3, T4, T5, TAdded>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5)> source,
        IncrementalValueProvider<TAdded> other)
    {
        return IncrementalValueProviderExtensions.Combine(source, other).Select(static (tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Right));
    }

    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, TAdded> CombineAndUnwrap<T1, T2, T3, T4, T5, TAdded>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5)> source,
        IncrementalValueProvider<IncrementalResult<TAdded>> other,
        IncrementalGeneratorInitializationContext context)
    {
        return IncrementalValueProviderExtensions.Combine(source, other).Select(static (tuple, _) => tuple.Right.Map(value => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, value))).Unwrap(context);
    }
    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, T3, T4, T5, TTo>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5)> first, 
        Func<T1, T2, T3, T4, T5, IEnumerable<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, T3, T4, T5, TTo>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5)> first, 
        Func<T1, T2, T3, T4, T5, ImmutableArray<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, T3, T4, T5, TTo>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5)> first, 
        Func<T1, T2, T3, T4, T5, IEnumerable<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, T3, T4, T5, TTo>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5)> first, 
        Func<T1, T2, T3, T4, T5, ImmutableArray<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }

    public static IncrementalValuesProvider<T1, T2, T3, T4, T5> WithComparer<T1, T2, T3, T4, T5, TComparand>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5)> provider,
        Func<T1, T2, T3, T4, T5, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2, T3, T4, T5), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5)));
    }

    public static IncrementalValueProvider<T1, T2, T3, T4, T5> WithComparer<T1, T2, T3, T4, T5, TComparand>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5)> provider, 
        Func<T1, T2, T3, T4, T5, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2, T3, T4, T5), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5)));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5> Select<TFrom, T1, T2, T3, T4, T5>(
        this IncrementalValuesProvider<TFrom> source,
        Func<TFrom, CancellationToken, (T1, T2, T3, T4, T5)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, selector);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<T1, T2, T3, T4, T5> Select<TFrom, T1, T2, T3, T4, T5>(
        this IncrementalValueProvider<TFrom> source,
        Func<TFrom, CancellationToken, (T1, T2, T3, T4, T5)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, selector);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> Select<T1, T2, T3, T4, T5, TTo>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5)> source,
        Func<T1, T2, T3, T4, T5, CancellationToken, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> Select<T1, T2, T3, T4, T5, TTo>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5)> source,
        Func<T1, T2, T3, T4, T5, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<TTo> Select<T1, T2, T3, T4, T5, TTo>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5)> source,
        Func<T1, T2, T3, T4, T5, CancellationToken, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, ct));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<TTo> Select<T1, T2, T3, T4, T5, TTo>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5)> source,
        Func<T1, T2, T3, T4, T5, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5> SelectMany<TFrom, T1, T2, T3, T4, T5>(
        this IncrementalValuesProvider<TFrom> first, 
        Func<TFrom, CancellationToken, IEnumerable<(T1, T2, T3, T4, T5)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, selector);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5> Unwrap<T1, T2, T3, T4, T5>(
        this IncrementalValuesProvider<SourceKit.IncrementalResult<(T1, T2, T3, T4, T5)>> provider,
        IncrementalGeneratorInitializationContext context)
    {
        return SourceKitValuesProviderExtensions.Unwrap(provider, context);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5> Unwrap<T1, T2, T3, T4, T5>(
        this IncrementalValueProvider<SourceKit.IncrementalResult<(T1, T2, T3, T4, T5)>> provider,
        IncrementalGeneratorInitializationContext context)
    {
        return SourceKitValuesProviderExtensions.Unwrap(provider, context);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5> Where<T1, T2, T3, T4, T5>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5)> provider,
        Func<T1, T2, T3, T4, T5, bool> predicate)
    {
        return ((IncrementalValuesProvider<T1, T2, T3, T4, T5>)provider).Where(predicate);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5> Where<T1, T2, T3, T4, T5>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5)> provider,
        Func<T1, T2, T3, T4, T5, bool> predicate)
    {
        return ((IncrementalValueProvider<T1, T2, T3, T4, T5>)provider).Where(predicate);
    }

    public static void RegisterSourceOutput<T1, T2, T3, T4, T5>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValueProvider<T1, T2, T3, T4, T5> provider,
        Action<SourceProductionContext, T1, T2, T3, T4, T5> action)
    {
        context.RegisterSourceOutput(
            (IncrementalValueProvider<(T1, T2, T3, T4, T5)>)provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }

    public static void RegisterSourceOutput<T1, T2, T3, T4, T5>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValueProvider<(T1, T2, T3, T4, T5)> provider,
        Action<SourceProductionContext, T1, T2, T3, T4, T5> action)
    {
        context.RegisterSourceOutput(
            provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }

    public static void RegisterSourceOutput<T1, T2, T3, T4, T5>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValuesProvider<T1, T2, T3, T4, T5> provider,
        Action<SourceProductionContext, T1, T2, T3, T4, T5> action)
    {
        context.RegisterSourceOutput(
            (IncrementalValuesProvider<(T1, T2, T3, T4, T5)>)provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }

    public static void RegisterSourceOutput<T1, T2, T3, T4, T5>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValuesProvider<(T1, T2, T3, T4, T5)> provider,
        Action<SourceProductionContext, T1, T2, T3, T4, T5> action)
    {
        context.RegisterSourceOutput(
            provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6> Select<TFrom, T1, T2, T3, T4, T5, T6>(
        this IncrementalValuesProvider<TFrom> source,
        Func<TFrom, (T1, T2, T3, T4, T5, T6)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (from, _) => selector(from));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<T1, T2, T3, T4, T5, T6> Select<TFrom, T1, T2, T3, T4, T5, T6>(
        this IncrementalValueProvider<TFrom> source,
        Func<TFrom, (T1, T2, T3, T4, T5, T6)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (from, _) => selector(from));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6> SelectMany<TFrom, T1, T2, T3, T4, T5, T6>(
        this IncrementalValuesProvider<TFrom> first, 
        Func<TFrom, IEnumerable<(T1, T2, T3, T4, T5, T6)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6> SelectMany<TFrom, T1, T2, T3, T4, T5, T6>(
        this IncrementalValuesProvider<TFrom> first, 
        Func<TFrom, ImmutableArray<(T1, T2, T3, T4, T5, T6)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6> SelectMany<TFrom, T1, T2, T3, T4, T5, T6>(
        this IncrementalValueProvider<TFrom> first, 
        Func<TFrom, IEnumerable<(T1, T2, T3, T4, T5, T6)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6> SelectMany<TFrom, T1, T2, T3, T4, T5, T6>(
        this IncrementalValueProvider<TFrom> first, 
        Func<TFrom, ImmutableArray<(T1, T2, T3, T4, T5, T6)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6> WithComparer<T1, T2, T3, T4, T5, T6, TComparand>(
        this IncrementalValuesProvider<T1, T2, T3, T4, T5, T6> provider,
        Func<T1, T2, T3, T4, T5, T6, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2, T3, T4, T5, T6), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6)));
    }

    public static IncrementalValueProvider<T1, T2, T3, T4, T5, T6> WithComparer<T1, T2, T3, T4, T5, T6, TComparand>(
        this IncrementalValueProvider<T1, T2, T3, T4, T5, T6> provider, 
        Func<T1, T2, T3, T4, T5, T6, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2, T3, T4, T5, T6), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6)));
    }

    public static IncrementalValuesProvider<IncrementalResult<(T1, T2, T3, T4, T5, T6)>> WithResultComparer<T1, T2, T3, T4, T5, T6, TComparand>(
        this IncrementalValuesProvider<IncrementalResult<(T1, T2, T3, T4, T5, T6)>> provider,
        Func<T1, T2, T3, T4, T5, T6, TComparand> selector)
    {
        return provider.WithComparer(new IncrementalResultComparer<(T1, T2, T3, T4, T5, T6), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6)));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, TAdded> Combine<T1, T2, T3, T4, T5, T6, TAdded>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6)> source,
        IncrementalValueProvider<TAdded> other)
    {
        return IncrementalValueProviderExtensions.Combine(source, other).Select(static (tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Left.Item6, tuple.Right));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<T1, T2, T3, T4, T5, T6, TAdded> Combine<T1, T2, T3, T4, T5, T6, TAdded>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6)> source,
        IncrementalValueProvider<TAdded> other)
    {
        return IncrementalValueProviderExtensions.Combine(source, other).Select(static (tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Left.Item6, tuple.Right));
    }

    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, TAdded> CombineAndUnwrap<T1, T2, T3, T4, T5, T6, TAdded>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6)> source,
        IncrementalValueProvider<IncrementalResult<TAdded>> other,
        IncrementalGeneratorInitializationContext context)
    {
        return IncrementalValueProviderExtensions.Combine(source, other).Select(static (tuple, _) => tuple.Right.Map(value => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Left.Item6, value))).Unwrap(context);
    }
    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, T3, T4, T5, T6, TTo>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6)> first, 
        Func<T1, T2, T3, T4, T5, T6, IEnumerable<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, T3, T4, T5, T6, TTo>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6)> first, 
        Func<T1, T2, T3, T4, T5, T6, ImmutableArray<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, T3, T4, T5, T6, TTo>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6)> first, 
        Func<T1, T2, T3, T4, T5, T6, IEnumerable<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, T3, T4, T5, T6, TTo>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6)> first, 
        Func<T1, T2, T3, T4, T5, T6, ImmutableArray<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }

    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6> WithComparer<T1, T2, T3, T4, T5, T6, TComparand>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6)> provider,
        Func<T1, T2, T3, T4, T5, T6, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2, T3, T4, T5, T6), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6)));
    }

    public static IncrementalValueProvider<T1, T2, T3, T4, T5, T6> WithComparer<T1, T2, T3, T4, T5, T6, TComparand>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6)> provider, 
        Func<T1, T2, T3, T4, T5, T6, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2, T3, T4, T5, T6), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6)));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6> Select<TFrom, T1, T2, T3, T4, T5, T6>(
        this IncrementalValuesProvider<TFrom> source,
        Func<TFrom, CancellationToken, (T1, T2, T3, T4, T5, T6)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, selector);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<T1, T2, T3, T4, T5, T6> Select<TFrom, T1, T2, T3, T4, T5, T6>(
        this IncrementalValueProvider<TFrom> source,
        Func<TFrom, CancellationToken, (T1, T2, T3, T4, T5, T6)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, selector);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> Select<T1, T2, T3, T4, T5, T6, TTo>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6)> source,
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> Select<T1, T2, T3, T4, T5, T6, TTo>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6)> source,
        Func<T1, T2, T3, T4, T5, T6, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<TTo> Select<T1, T2, T3, T4, T5, T6, TTo>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6)> source,
        Func<T1, T2, T3, T4, T5, T6, CancellationToken, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, ct));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<TTo> Select<T1, T2, T3, T4, T5, T6, TTo>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6)> source,
        Func<T1, T2, T3, T4, T5, T6, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6> SelectMany<TFrom, T1, T2, T3, T4, T5, T6>(
        this IncrementalValuesProvider<TFrom> first, 
        Func<TFrom, CancellationToken, IEnumerable<(T1, T2, T3, T4, T5, T6)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, selector);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6> Unwrap<T1, T2, T3, T4, T5, T6>(
        this IncrementalValuesProvider<SourceKit.IncrementalResult<(T1, T2, T3, T4, T5, T6)>> provider,
        IncrementalGeneratorInitializationContext context)
    {
        return SourceKitValuesProviderExtensions.Unwrap(provider, context);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6> Unwrap<T1, T2, T3, T4, T5, T6>(
        this IncrementalValueProvider<SourceKit.IncrementalResult<(T1, T2, T3, T4, T5, T6)>> provider,
        IncrementalGeneratorInitializationContext context)
    {
        return SourceKitValuesProviderExtensions.Unwrap(provider, context);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6> Where<T1, T2, T3, T4, T5, T6>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6)> provider,
        Func<T1, T2, T3, T4, T5, T6, bool> predicate)
    {
        return ((IncrementalValuesProvider<T1, T2, T3, T4, T5, T6>)provider).Where(predicate);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6> Where<T1, T2, T3, T4, T5, T6>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6)> provider,
        Func<T1, T2, T3, T4, T5, T6, bool> predicate)
    {
        return ((IncrementalValueProvider<T1, T2, T3, T4, T5, T6>)provider).Where(predicate);
    }

    public static void RegisterSourceOutput<T1, T2, T3, T4, T5, T6>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValueProvider<T1, T2, T3, T4, T5, T6> provider,
        Action<SourceProductionContext, T1, T2, T3, T4, T5, T6> action)
    {
        context.RegisterSourceOutput(
            (IncrementalValueProvider<(T1, T2, T3, T4, T5, T6)>)provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }

    public static void RegisterSourceOutput<T1, T2, T3, T4, T5, T6>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValueProvider<(T1, T2, T3, T4, T5, T6)> provider,
        Action<SourceProductionContext, T1, T2, T3, T4, T5, T6> action)
    {
        context.RegisterSourceOutput(
            provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }

    public static void RegisterSourceOutput<T1, T2, T3, T4, T5, T6>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValuesProvider<T1, T2, T3, T4, T5, T6> provider,
        Action<SourceProductionContext, T1, T2, T3, T4, T5, T6> action)
    {
        context.RegisterSourceOutput(
            (IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6)>)provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }

    public static void RegisterSourceOutput<T1, T2, T3, T4, T5, T6>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6)> provider,
        Action<SourceProductionContext, T1, T2, T3, T4, T5, T6> action)
    {
        context.RegisterSourceOutput(
            provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7> Select<TFrom, T1, T2, T3, T4, T5, T6, T7>(
        this IncrementalValuesProvider<TFrom> source,
        Func<TFrom, (T1, T2, T3, T4, T5, T6, T7)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (from, _) => selector(from));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7> Select<TFrom, T1, T2, T3, T4, T5, T6, T7>(
        this IncrementalValueProvider<TFrom> source,
        Func<TFrom, (T1, T2, T3, T4, T5, T6, T7)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (from, _) => selector(from));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7> SelectMany<TFrom, T1, T2, T3, T4, T5, T6, T7>(
        this IncrementalValuesProvider<TFrom> first, 
        Func<TFrom, IEnumerable<(T1, T2, T3, T4, T5, T6, T7)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7> SelectMany<TFrom, T1, T2, T3, T4, T5, T6, T7>(
        this IncrementalValuesProvider<TFrom> first, 
        Func<TFrom, ImmutableArray<(T1, T2, T3, T4, T5, T6, T7)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7> SelectMany<TFrom, T1, T2, T3, T4, T5, T6, T7>(
        this IncrementalValueProvider<TFrom> first, 
        Func<TFrom, IEnumerable<(T1, T2, T3, T4, T5, T6, T7)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7> SelectMany<TFrom, T1, T2, T3, T4, T5, T6, T7>(
        this IncrementalValueProvider<TFrom> first, 
        Func<TFrom, ImmutableArray<(T1, T2, T3, T4, T5, T6, T7)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7> WithComparer<T1, T2, T3, T4, T5, T6, T7, TComparand>(
        this IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7> provider,
        Func<T1, T2, T3, T4, T5, T6, T7, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2, T3, T4, T5, T6, T7), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7)));
    }

    public static IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7> WithComparer<T1, T2, T3, T4, T5, T6, T7, TComparand>(
        this IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7> provider, 
        Func<T1, T2, T3, T4, T5, T6, T7, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2, T3, T4, T5, T6, T7), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7)));
    }

    public static IncrementalValuesProvider<IncrementalResult<(T1, T2, T3, T4, T5, T6, T7)>> WithResultComparer<T1, T2, T3, T4, T5, T6, T7, TComparand>(
        this IncrementalValuesProvider<IncrementalResult<(T1, T2, T3, T4, T5, T6, T7)>> provider,
        Func<T1, T2, T3, T4, T5, T6, T7, TComparand> selector)
    {
        return provider.WithComparer(new IncrementalResultComparer<(T1, T2, T3, T4, T5, T6, T7), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7)));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, TAdded> Combine<T1, T2, T3, T4, T5, T6, T7, TAdded>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7)> source,
        IncrementalValueProvider<TAdded> other)
    {
        return IncrementalValueProviderExtensions.Combine(source, other).Select(static (tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Left.Item6, tuple.Left.Item7, tuple.Right));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, TAdded> Combine<T1, T2, T3, T4, T5, T6, T7, TAdded>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7)> source,
        IncrementalValueProvider<TAdded> other)
    {
        return IncrementalValueProviderExtensions.Combine(source, other).Select(static (tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Left.Item6, tuple.Left.Item7, tuple.Right));
    }

    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, TAdded> CombineAndUnwrap<T1, T2, T3, T4, T5, T6, T7, TAdded>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7)> source,
        IncrementalValueProvider<IncrementalResult<TAdded>> other,
        IncrementalGeneratorInitializationContext context)
    {
        return IncrementalValueProviderExtensions.Combine(source, other).Select(static (tuple, _) => tuple.Right.Map(value => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Left.Item6, tuple.Left.Item7, value))).Unwrap(context);
    }
    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, T3, T4, T5, T6, T7, TTo>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7)> first, 
        Func<T1, T2, T3, T4, T5, T6, T7, IEnumerable<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, T3, T4, T5, T6, T7, TTo>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7)> first, 
        Func<T1, T2, T3, T4, T5, T6, T7, ImmutableArray<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, T3, T4, T5, T6, T7, TTo>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7)> first, 
        Func<T1, T2, T3, T4, T5, T6, T7, IEnumerable<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, T3, T4, T5, T6, T7, TTo>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7)> first, 
        Func<T1, T2, T3, T4, T5, T6, T7, ImmutableArray<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }

    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7> WithComparer<T1, T2, T3, T4, T5, T6, T7, TComparand>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7)> provider,
        Func<T1, T2, T3, T4, T5, T6, T7, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2, T3, T4, T5, T6, T7), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7)));
    }

    public static IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7> WithComparer<T1, T2, T3, T4, T5, T6, T7, TComparand>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7)> provider, 
        Func<T1, T2, T3, T4, T5, T6, T7, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2, T3, T4, T5, T6, T7), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7)));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7> Select<TFrom, T1, T2, T3, T4, T5, T6, T7>(
        this IncrementalValuesProvider<TFrom> source,
        Func<TFrom, CancellationToken, (T1, T2, T3, T4, T5, T6, T7)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, selector);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7> Select<TFrom, T1, T2, T3, T4, T5, T6, T7>(
        this IncrementalValueProvider<TFrom> source,
        Func<TFrom, CancellationToken, (T1, T2, T3, T4, T5, T6, T7)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, selector);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> Select<T1, T2, T3, T4, T5, T6, T7, TTo>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7)> source,
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> Select<T1, T2, T3, T4, T5, T6, T7, TTo>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7)> source,
        Func<T1, T2, T3, T4, T5, T6, T7, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<TTo> Select<T1, T2, T3, T4, T5, T6, T7, TTo>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7)> source,
        Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, ct));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<TTo> Select<T1, T2, T3, T4, T5, T6, T7, TTo>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7)> source,
        Func<T1, T2, T3, T4, T5, T6, T7, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7> SelectMany<TFrom, T1, T2, T3, T4, T5, T6, T7>(
        this IncrementalValuesProvider<TFrom> first, 
        Func<TFrom, CancellationToken, IEnumerable<(T1, T2, T3, T4, T5, T6, T7)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, selector);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7> Unwrap<T1, T2, T3, T4, T5, T6, T7>(
        this IncrementalValuesProvider<SourceKit.IncrementalResult<(T1, T2, T3, T4, T5, T6, T7)>> provider,
        IncrementalGeneratorInitializationContext context)
    {
        return SourceKitValuesProviderExtensions.Unwrap(provider, context);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7> Unwrap<T1, T2, T3, T4, T5, T6, T7>(
        this IncrementalValueProvider<SourceKit.IncrementalResult<(T1, T2, T3, T4, T5, T6, T7)>> provider,
        IncrementalGeneratorInitializationContext context)
    {
        return SourceKitValuesProviderExtensions.Unwrap(provider, context);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7> Where<T1, T2, T3, T4, T5, T6, T7>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7)> provider,
        Func<T1, T2, T3, T4, T5, T6, T7, bool> predicate)
    {
        return ((IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7>)provider).Where(predicate);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7> Where<T1, T2, T3, T4, T5, T6, T7>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7)> provider,
        Func<T1, T2, T3, T4, T5, T6, T7, bool> predicate)
    {
        return ((IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7>)provider).Where(predicate);
    }

    public static void RegisterSourceOutput<T1, T2, T3, T4, T5, T6, T7>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7> provider,
        Action<SourceProductionContext, T1, T2, T3, T4, T5, T6, T7> action)
    {
        context.RegisterSourceOutput(
            (IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7)>)provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }

    public static void RegisterSourceOutput<T1, T2, T3, T4, T5, T6, T7>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7)> provider,
        Action<SourceProductionContext, T1, T2, T3, T4, T5, T6, T7> action)
    {
        context.RegisterSourceOutput(
            provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }

    public static void RegisterSourceOutput<T1, T2, T3, T4, T5, T6, T7>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7> provider,
        Action<SourceProductionContext, T1, T2, T3, T4, T5, T6, T7> action)
    {
        context.RegisterSourceOutput(
            (IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7)>)provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }

    public static void RegisterSourceOutput<T1, T2, T3, T4, T5, T6, T7>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7)> provider,
        Action<SourceProductionContext, T1, T2, T3, T4, T5, T6, T7> action)
    {
        context.RegisterSourceOutput(
            provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8> Select<TFrom, T1, T2, T3, T4, T5, T6, T7, T8>(
        this IncrementalValuesProvider<TFrom> source,
        Func<TFrom, (T1, T2, T3, T4, T5, T6, T7, T8)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (from, _) => selector(from));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8> Select<TFrom, T1, T2, T3, T4, T5, T6, T7, T8>(
        this IncrementalValueProvider<TFrom> source,
        Func<TFrom, (T1, T2, T3, T4, T5, T6, T7, T8)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (from, _) => selector(from));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8> SelectMany<TFrom, T1, T2, T3, T4, T5, T6, T7, T8>(
        this IncrementalValuesProvider<TFrom> first, 
        Func<TFrom, IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8> SelectMany<TFrom, T1, T2, T3, T4, T5, T6, T7, T8>(
        this IncrementalValuesProvider<TFrom> first, 
        Func<TFrom, ImmutableArray<(T1, T2, T3, T4, T5, T6, T7, T8)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8> SelectMany<TFrom, T1, T2, T3, T4, T5, T6, T7, T8>(
        this IncrementalValueProvider<TFrom> first, 
        Func<TFrom, IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8> SelectMany<TFrom, T1, T2, T3, T4, T5, T6, T7, T8>(
        this IncrementalValueProvider<TFrom> first, 
        Func<TFrom, ImmutableArray<(T1, T2, T3, T4, T5, T6, T7, T8)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8> WithComparer<T1, T2, T3, T4, T5, T6, T7, T8, TComparand>(
        this IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8> provider,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2, T3, T4, T5, T6, T7, T8), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8)));
    }

    public static IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8> WithComparer<T1, T2, T3, T4, T5, T6, T7, T8, TComparand>(
        this IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8> provider, 
        Func<T1, T2, T3, T4, T5, T6, T7, T8, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2, T3, T4, T5, T6, T7, T8), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8)));
    }

    public static IncrementalValuesProvider<IncrementalResult<(T1, T2, T3, T4, T5, T6, T7, T8)>> WithResultComparer<T1, T2, T3, T4, T5, T6, T7, T8, TComparand>(
        this IncrementalValuesProvider<IncrementalResult<(T1, T2, T3, T4, T5, T6, T7, T8)>> provider,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, TComparand> selector)
    {
        return provider.WithComparer(new IncrementalResultComparer<(T1, T2, T3, T4, T5, T6, T7, T8), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8)));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, TAdded> Combine<T1, T2, T3, T4, T5, T6, T7, T8, TAdded>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8)> source,
        IncrementalValueProvider<TAdded> other)
    {
        return IncrementalValueProviderExtensions.Combine(source, other).Select(static (tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Left.Item6, tuple.Left.Item7, tuple.Left.Item8, tuple.Right));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, TAdded> Combine<T1, T2, T3, T4, T5, T6, T7, T8, TAdded>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8)> source,
        IncrementalValueProvider<TAdded> other)
    {
        return IncrementalValueProviderExtensions.Combine(source, other).Select(static (tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Left.Item6, tuple.Left.Item7, tuple.Left.Item8, tuple.Right));
    }

    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, TAdded> CombineAndUnwrap<T1, T2, T3, T4, T5, T6, T7, T8, TAdded>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8)> source,
        IncrementalValueProvider<IncrementalResult<TAdded>> other,
        IncrementalGeneratorInitializationContext context)
    {
        return IncrementalValueProviderExtensions.Combine(source, other).Select(static (tuple, _) => tuple.Right.Map(value => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Left.Item6, tuple.Left.Item7, tuple.Left.Item8, value))).Unwrap(context);
    }
    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, T3, T4, T5, T6, T7, T8, TTo>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8)> first, 
        Func<T1, T2, T3, T4, T5, T6, T7, T8, IEnumerable<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, T3, T4, T5, T6, T7, T8, TTo>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8)> first, 
        Func<T1, T2, T3, T4, T5, T6, T7, T8, ImmutableArray<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, T3, T4, T5, T6, T7, T8, TTo>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8)> first, 
        Func<T1, T2, T3, T4, T5, T6, T7, T8, IEnumerable<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, T3, T4, T5, T6, T7, T8, TTo>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8)> first, 
        Func<T1, T2, T3, T4, T5, T6, T7, T8, ImmutableArray<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }

    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8> WithComparer<T1, T2, T3, T4, T5, T6, T7, T8, TComparand>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8)> provider,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2, T3, T4, T5, T6, T7, T8), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8)));
    }

    public static IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8> WithComparer<T1, T2, T3, T4, T5, T6, T7, T8, TComparand>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8)> provider, 
        Func<T1, T2, T3, T4, T5, T6, T7, T8, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2, T3, T4, T5, T6, T7, T8), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8)));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8> Select<TFrom, T1, T2, T3, T4, T5, T6, T7, T8>(
        this IncrementalValuesProvider<TFrom> source,
        Func<TFrom, CancellationToken, (T1, T2, T3, T4, T5, T6, T7, T8)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, selector);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8> Select<TFrom, T1, T2, T3, T4, T5, T6, T7, T8>(
        this IncrementalValueProvider<TFrom> source,
        Func<TFrom, CancellationToken, (T1, T2, T3, T4, T5, T6, T7, T8)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, selector);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> Select<T1, T2, T3, T4, T5, T6, T7, T8, TTo>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8)> source,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> Select<T1, T2, T3, T4, T5, T6, T7, T8, TTo>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8)> source,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<TTo> Select<T1, T2, T3, T4, T5, T6, T7, T8, TTo>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8)> source,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, ct));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<TTo> Select<T1, T2, T3, T4, T5, T6, T7, T8, TTo>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8)> source,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8> SelectMany<TFrom, T1, T2, T3, T4, T5, T6, T7, T8>(
        this IncrementalValuesProvider<TFrom> first, 
        Func<TFrom, CancellationToken, IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, selector);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8> Unwrap<T1, T2, T3, T4, T5, T6, T7, T8>(
        this IncrementalValuesProvider<SourceKit.IncrementalResult<(T1, T2, T3, T4, T5, T6, T7, T8)>> provider,
        IncrementalGeneratorInitializationContext context)
    {
        return SourceKitValuesProviderExtensions.Unwrap(provider, context);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8> Unwrap<T1, T2, T3, T4, T5, T6, T7, T8>(
        this IncrementalValueProvider<SourceKit.IncrementalResult<(T1, T2, T3, T4, T5, T6, T7, T8)>> provider,
        IncrementalGeneratorInitializationContext context)
    {
        return SourceKitValuesProviderExtensions.Unwrap(provider, context);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8> Where<T1, T2, T3, T4, T5, T6, T7, T8>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8)> provider,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> predicate)
    {
        return ((IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8>)provider).Where(predicate);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8> Where<T1, T2, T3, T4, T5, T6, T7, T8>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8)> provider,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> predicate)
    {
        return ((IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8>)provider).Where(predicate);
    }

    public static void RegisterSourceOutput<T1, T2, T3, T4, T5, T6, T7, T8>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8> provider,
        Action<SourceProductionContext, T1, T2, T3, T4, T5, T6, T7, T8> action)
    {
        context.RegisterSourceOutput(
            (IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8)>)provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }

    public static void RegisterSourceOutput<T1, T2, T3, T4, T5, T6, T7, T8>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8)> provider,
        Action<SourceProductionContext, T1, T2, T3, T4, T5, T6, T7, T8> action)
    {
        context.RegisterSourceOutput(
            provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }

    public static void RegisterSourceOutput<T1, T2, T3, T4, T5, T6, T7, T8>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8> provider,
        Action<SourceProductionContext, T1, T2, T3, T4, T5, T6, T7, T8> action)
    {
        context.RegisterSourceOutput(
            (IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8)>)provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }

    public static void RegisterSourceOutput<T1, T2, T3, T4, T5, T6, T7, T8>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8)> provider,
        Action<SourceProductionContext, T1, T2, T3, T4, T5, T6, T7, T8> action)
    {
        context.RegisterSourceOutput(
            provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> Select<TFrom, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        this IncrementalValuesProvider<TFrom> source,
        Func<TFrom, (T1, T2, T3, T4, T5, T6, T7, T8, T9)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (from, _) => selector(from));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> Select<TFrom, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        this IncrementalValueProvider<TFrom> source,
        Func<TFrom, (T1, T2, T3, T4, T5, T6, T7, T8, T9)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (from, _) => selector(from));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> SelectMany<TFrom, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        this IncrementalValuesProvider<TFrom> first, 
        Func<TFrom, IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> SelectMany<TFrom, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        this IncrementalValuesProvider<TFrom> first, 
        Func<TFrom, ImmutableArray<(T1, T2, T3, T4, T5, T6, T7, T8, T9)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> SelectMany<TFrom, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        this IncrementalValueProvider<TFrom> first, 
        Func<TFrom, IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> SelectMany<TFrom, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        this IncrementalValueProvider<TFrom> first, 
        Func<TFrom, ImmutableArray<(T1, T2, T3, T4, T5, T6, T7, T8, T9)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> WithComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, TComparand>(
        this IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> provider,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2, T3, T4, T5, T6, T7, T8, T9), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9)));
    }

    public static IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> WithComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, TComparand>(
        this IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> provider, 
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2, T3, T4, T5, T6, T7, T8, T9), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9)));
    }

    public static IncrementalValuesProvider<IncrementalResult<(T1, T2, T3, T4, T5, T6, T7, T8, T9)>> WithResultComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, TComparand>(
        this IncrementalValuesProvider<IncrementalResult<(T1, T2, T3, T4, T5, T6, T7, T8, T9)>> provider,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TComparand> selector)
    {
        return provider.WithComparer(new IncrementalResultComparer<(T1, T2, T3, T4, T5, T6, T7, T8, T9), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9)));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, TAdded> Combine<T1, T2, T3, T4, T5, T6, T7, T8, T9, TAdded>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> source,
        IncrementalValueProvider<TAdded> other)
    {
        return IncrementalValueProviderExtensions.Combine(source, other).Select(static (tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Left.Item6, tuple.Left.Item7, tuple.Left.Item8, tuple.Left.Item9, tuple.Right));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, TAdded> Combine<T1, T2, T3, T4, T5, T6, T7, T8, T9, TAdded>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> source,
        IncrementalValueProvider<TAdded> other)
    {
        return IncrementalValueProviderExtensions.Combine(source, other).Select(static (tuple, _) => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Left.Item6, tuple.Left.Item7, tuple.Left.Item8, tuple.Left.Item9, tuple.Right));
    }

    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, TAdded> CombineAndUnwrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, TAdded>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> source,
        IncrementalValueProvider<IncrementalResult<TAdded>> other,
        IncrementalGeneratorInitializationContext context)
    {
        return IncrementalValueProviderExtensions.Combine(source, other).Select(static (tuple, _) => tuple.Right.Map(value => (tuple.Left.Item1, tuple.Left.Item2, tuple.Left.Item3, tuple.Left.Item4, tuple.Left.Item5, tuple.Left.Item6, tuple.Left.Item7, tuple.Left.Item8, tuple.Left.Item9, value))).Unwrap(context);
    }
    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, T3, T4, T5, T6, T7, T8, T9, TTo>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> first, 
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IEnumerable<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, T3, T4, T5, T6, T7, T8, T9, TTo>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> first, 
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, ImmutableArray<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, T3, T4, T5, T6, T7, T8, T9, TTo>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> first, 
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IEnumerable<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, T3, T4, T5, T6, T7, T8, T9, TTo>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> first, 
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, ImmutableArray<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }

    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> WithComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, TComparand>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> provider,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2, T3, T4, T5, T6, T7, T8, T9), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9)));
    }

    public static IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> WithComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, TComparand>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> provider, 
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2, T3, T4, T5, T6, T7, T8, T9), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9)));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> Select<TFrom, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        this IncrementalValuesProvider<TFrom> source,
        Func<TFrom, CancellationToken, (T1, T2, T3, T4, T5, T6, T7, T8, T9)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, selector);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> Select<TFrom, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        this IncrementalValueProvider<TFrom> source,
        Func<TFrom, CancellationToken, (T1, T2, T3, T4, T5, T6, T7, T8, T9)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, selector);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, TTo>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> source,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, TTo>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> source,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<TTo> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, TTo>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> source,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, ct));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<TTo> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, TTo>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> source,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> SelectMany<TFrom, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        this IncrementalValuesProvider<TFrom> first, 
        Func<TFrom, CancellationToken, IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, selector);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> Unwrap<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        this IncrementalValuesProvider<SourceKit.IncrementalResult<(T1, T2, T3, T4, T5, T6, T7, T8, T9)>> provider,
        IncrementalGeneratorInitializationContext context)
    {
        return SourceKitValuesProviderExtensions.Unwrap(provider, context);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> Unwrap<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        this IncrementalValueProvider<SourceKit.IncrementalResult<(T1, T2, T3, T4, T5, T6, T7, T8, T9)>> provider,
        IncrementalGeneratorInitializationContext context)
    {
        return SourceKitValuesProviderExtensions.Unwrap(provider, context);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> Where<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> provider,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> predicate)
    {
        return ((IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9>)provider).Where(predicate);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> Where<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> provider,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> predicate)
    {
        return ((IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9>)provider).Where(predicate);
    }

    public static void RegisterSourceOutput<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> provider,
        Action<SourceProductionContext, T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
    {
        context.RegisterSourceOutput(
            (IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9)>)provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }

    public static void RegisterSourceOutput<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> provider,
        Action<SourceProductionContext, T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
    {
        context.RegisterSourceOutput(
            provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }

    public static void RegisterSourceOutput<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9> provider,
        Action<SourceProductionContext, T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
    {
        context.RegisterSourceOutput(
            (IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9)>)provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }

    public static void RegisterSourceOutput<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> provider,
        Action<SourceProductionContext, T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
    {
        context.RegisterSourceOutput(
            provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Select<TFrom, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
        this IncrementalValuesProvider<TFrom> source,
        Func<TFrom, (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (from, _) => selector(from));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Select<TFrom, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
        this IncrementalValueProvider<TFrom> source,
        Func<TFrom, (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (from, _) => selector(from));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> SelectMany<TFrom, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
        this IncrementalValuesProvider<TFrom> first, 
        Func<TFrom, IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> SelectMany<TFrom, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
        this IncrementalValuesProvider<TFrom> first, 
        Func<TFrom, ImmutableArray<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> SelectMany<TFrom, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
        this IncrementalValueProvider<TFrom> first, 
        Func<TFrom, IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> SelectMany<TFrom, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
        this IncrementalValueProvider<TFrom> first, 
        Func<TFrom, ImmutableArray<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (t1, _) => selector(t1));
    }

    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> WithComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TComparand>(
        this IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> provider,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10)));
    }

    public static IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> WithComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TComparand>(
        this IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> provider, 
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10)));
    }

    public static IncrementalValuesProvider<IncrementalResult<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)>> WithResultComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TComparand>(
        this IncrementalValuesProvider<IncrementalResult<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)>> provider,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TComparand> selector)
    {
        return provider.WithComparer(new IncrementalResultComparer<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10)));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TTo>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> first, 
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IEnumerable<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TTo>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> first, 
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, ImmutableArray<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TTo>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> first, 
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IEnumerable<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> SelectMany<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TTo>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> first, 
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, ImmutableArray<TTo>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }

    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> WithComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TComparand>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> provider,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10)));
    }

    public static IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> WithComparer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TComparand>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> provider, 
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TComparand> selector)
    {
        return provider.WithComparer(EqualityComparerFactory.CreateFromSelector<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10), TComparand>(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10)));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Select<TFrom, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
        this IncrementalValuesProvider<TFrom> source,
        Func<TFrom, CancellationToken, (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, selector);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Select<TFrom, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
        this IncrementalValueProvider<TFrom> source,
        Func<TFrom, CancellationToken, (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, selector);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TTo>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> source,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<TTo> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TTo>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> source,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<TTo> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TTo>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> source,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, ct) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, ct));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValueProvider<TTo> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TTo>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> source,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TTo> selector)
    {
        return IncrementalValueProviderExtensions.Select(source, (tuple, _) => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> SelectMany<TFrom, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
        this IncrementalValuesProvider<TFrom> first, 
        Func<TFrom, CancellationToken, IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)>> selector)
    {
        return IncrementalValueProviderExtensions.SelectMany(first, selector);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Unwrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
        this IncrementalValuesProvider<SourceKit.IncrementalResult<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)>> provider,
        IncrementalGeneratorInitializationContext context)
    {
        return SourceKitValuesProviderExtensions.Unwrap(provider, context);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Unwrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
        this IncrementalValueProvider<SourceKit.IncrementalResult<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)>> provider,
        IncrementalGeneratorInitializationContext context)
    {
        return SourceKitValuesProviderExtensions.Unwrap(provider, context);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Where<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
        this IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> provider,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> predicate)
    {
        return ((IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>)provider).Where(predicate);
    }

    [OverloadResolutionPriority(2147483647)]
    public static IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Where<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
        this IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> provider,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> predicate)
    {
        return ((IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>)provider).Where(predicate);
    }

    public static void RegisterSourceOutput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValueProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> provider,
        Action<SourceProductionContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
    {
        context.RegisterSourceOutput(
            (IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)>)provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }

    public static void RegisterSourceOutput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValueProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> provider,
        Action<SourceProductionContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
    {
        context.RegisterSourceOutput(
            provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }

    public static void RegisterSourceOutput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValuesProvider<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> provider,
        Action<SourceProductionContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
    {
        context.RegisterSourceOutput(
            (IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)>)provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }

    public static void RegisterSourceOutput<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
        this IncrementalGeneratorInitializationContext context,
        IncrementalValuesProvider<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> provider,
        Action<SourceProductionContext, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
    {
        context.RegisterSourceOutput(
            provider,
            (ctx, tuple) => action(ctx, tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
    }

   
}