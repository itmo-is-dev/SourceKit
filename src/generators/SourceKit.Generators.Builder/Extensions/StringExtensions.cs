using Humanizer;

namespace SourceKit.Generators.Builder.Extensions;

public static class StringExtensions
{
    public static string ToUnderscoreCamelCase(this string value)
    {
        return $"_{value.Camelize()}";
    }
}