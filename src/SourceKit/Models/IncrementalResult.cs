using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;

// ReSharper disable once CheckNamespace
namespace SourceKit;

public readonly struct IncrementalResult
{
    internal Diagnostic[]? Diagnostics { get; private init; }

    public static IncrementalResult Skip => default;

    public static IncrementalResult Failure(params Diagnostic[] diagnostics) => new() { Diagnostics = diagnostics };

    public static IncrementalResult<T> Success<T>(T value) => new() { Value = value };
}

public readonly record struct IncrementalResult<T>
{
    internal T? Value { get; init; }
    internal Diagnostic[]? Diagnostics { get; private init; }

    [MemberNotNullWhen(true, nameof(Value))]
    internal bool IsSuccess => Value is not null && Value.Equals(default(T)) is false;

    [MemberNotNullWhen(true, nameof(Diagnostics))]
    internal bool IsFailure => Diagnostics is not null;

    internal bool IsSkip => IsSuccess is false && IsFailure is false;

    public static implicit operator IncrementalResult<T>(IncrementalResult result)
    {
        return new IncrementalResult<T> { Diagnostics = result.Diagnostics };
    }

    public IncrementalResult<T2> Map<T2>(Func<T, T2> selector)
    {
        return this switch
        {
            { IsSuccess: true } => IncrementalResult.Success(selector(Value)),
            { IsFailure: true } => IncrementalResult.Failure(Diagnostics),
            { IsSkip: true } => IncrementalResult.Skip,

            _ => throw new InvalidOperationException("Unidentifiable result"),
        };
    }
}
