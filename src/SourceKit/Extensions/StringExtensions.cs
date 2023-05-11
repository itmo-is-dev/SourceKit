using SourceKit.Tools;

namespace SourceKit.Extensions;

public static class StringExtensions
{
    public static string ToCamelCase(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        if (char.IsLower(value[0]))
            return value;

        var builder = new StringModifier(value)
        {
            [0] = char.ToLower(value[0])
        };

        return builder;
    }

    public static string ToUnderscoreCamelCase(this string value)
    {
        return value switch
        {
            _ when string.IsNullOrEmpty(value) => value,

            ['_', var second, ..] when char.IsLower(second) => value,

            ['_', var second, ..] when char.IsLower(second) is false
                => new StringModifier(value) { [1] = char.ToLower(value[1]) },

            _ => new StringModifier(value) { [-1] = '_', [1] = char.ToLower(value[0]) },
        };
    }
}