using Microsoft.CodeAnalysis;
using SourceKit.Extensions;

namespace SourceKit.Reflect.Extensions;

public static class TypeResolverExtensions
{
    public static Type GetType(this ITypeResolver typeResolver, ITypeSymbol? symbol)
    {
        var type = typeResolver.FindType(symbol);

        if (type is null)
            throw new ArgumentException($"Unknown type {symbol?.GetFullyQualifiedName()}", nameof(symbol));

        return type;
    }

    public static Type GetType(this ITypeResolver typeResolver, string typeName)
    {
        var type = typeResolver.FindType(typeName);

        if (type is null)
            throw new ArgumentException($"Unknown type {typeName}", nameof(typeName));

        return type;
    }

    public static Type GetDelegateType(this ITypeResolver typeResolver, IMethodSymbol symbol)
    {
        var type = typeResolver.FindDelegateType(symbol);

        if (type is null)
            throw new ArgumentException($"Unknown type {symbol.Name}", nameof(symbol));

        return type;
    }
}