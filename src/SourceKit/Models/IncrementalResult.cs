using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

// ReSharper disable once CheckNamespace
namespace SourceKit;

internal enum IncrementalResultKind
{
    Skip,
    Failure,
    Success,
}

public readonly struct IncrementalResult
{
    internal IncrementalResultKind Kind { get; }

    internal ImmutableArray<Diagnostic>? Diagnostics { get; private init; }

    internal IncrementalResult(IncrementalResultKind kind)
    {
        Kind = kind;
    }

    public static IncrementalResult Skip => new(IncrementalResultKind.Skip);

    public static IncrementalResult SkipWithMetadata(Diagnostic metadata) => new(IncrementalResultKind.Skip)
    {
        Diagnostics = [metadata],
    };

    public static IncrementalResult Failure(params ImmutableArray<Diagnostic> diagnostics) => new(IncrementalResultKind.Failure)
    {
        Diagnostics = diagnostics,
    };

    public static IncrementalResult<T> Success<T>(T value) => new(IncrementalResultKind.Success)
    {
        Value = value,
    };

    public static IncrementalResult<T> Success<T>(T value, Diagnostic metadata) => new(IncrementalResultKind.Success)
    {
        Value = value,
        Diagnostics = [metadata],
    };
}

public readonly record struct IncrementalResult<T>
{
    internal IncrementalResultKind Kind { get; }

    internal T? Value { get; init; }
    internal ImmutableArray<Diagnostic>? Diagnostics { get; init; }

    internal IncrementalResult(IncrementalResultKind kind)
    {
        Kind = kind;
    }

    public static implicit operator IncrementalResult<T>(IncrementalResult result)
    {
        return new IncrementalResult<T>(result.Kind)
        {
            Diagnostics = result.Diagnostics,
        };
    }

    public IncrementalResult<T2> Map<T2>(Func<T, T2> selector)
    {
        return Kind switch
        {
            IncrementalResultKind.Success => IncrementalResult.Success(selector(Value!)),
            IncrementalResultKind.Failure => IncrementalResult.Failure(Diagnostics!.Value),
            IncrementalResultKind.Skip => IncrementalResult.Skip,

            _ => throw new InvalidOperationException("Unidentifiable result"),
        };
    }
}
