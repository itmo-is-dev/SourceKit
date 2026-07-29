using System.Collections.Immutable;
using SourceKit;

// ReSharper disable once CheckNamespace
namespace Microsoft.CodeAnalysis;

public static class SourceKitValuesProviderExtensions
{
    extension(IncrementalValueProviderExtensions)
    {
        public static IncrementalValuesProvider<T> Where<T>(IncrementalValueProvider<T> provider, Func<T, bool> predicate)
        {
            return provider.SelectMany(ImmutableArray<T> (value, _) => predicate(value) ? [value] : []);
        }
    }

    public static IncrementalValuesProvider<T> Unwrap<T>(
        this IncrementalValueProvider<IncrementalResult<T>> provider,
        IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(
            provider,
            static (context, result) =>
            {
                if (result.IsFailure is false)
                    return;

                foreach (Diagnostic diagnostic in result.Diagnostics)
                {
                    context.ReportDiagnostic(diagnostic);
                }
            });

        return IncrementalValueProviderExtensions
            .Where(provider, static result => result.IsSuccess)
            .Select(static (result, _) => result.Value!);
    }

    public static IncrementalValuesProvider<T> Unwrap<T>(
        this IncrementalValuesProvider<IncrementalResult<T>> provider,
        IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(
            provider,
            static (context, result) =>
            {
                if (result.IsFailure is false)
                    return;

                foreach (Diagnostic diagnostic in result.Diagnostics)
                {
                    context.ReportDiagnostic(diagnostic);
                }
            });

        return provider
            .Where(static result => result.IsSuccess)
            .Select(static (result, _) => result.Value!);
    }
}
