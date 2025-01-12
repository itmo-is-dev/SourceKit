using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Extensions;

public static class TypeSymbolExtensions
{
    public static IEnumerable<IdentifierNameSyntax> ToTypeArgumentSyntax(
        this IEnumerable<ITypeSymbol> symbols,
        bool includeGlobal = false)
    {
        return symbols.Select(x => x.ToTypeArgumentSyntax(includeGlobal));
    }

    public static IdentifierNameSyntax ToTypeArgumentSyntax(this ITypeSymbol symbol, bool includeGlobal = false)
        => IdentifierName(symbol.GetFullyQualifiedName(includeGlobal));

    public static bool IsAssignableTo(this ITypeSymbol source, ITypeSymbol destination)
    {
        return source.Equals(destination, SymbolEqualityComparer.IncludeNullability)
               || source is INamedTypeSymbol namedSource
               && namedSource.GetBaseTypesAndInterfaces().Contains(destination, SymbolEqualityComparer.Default);
    }

    public static bool IsAssignableFrom(this ITypeSymbol destination, INamedTypeSymbol source)
    {
        return source.IsAssignableTo(destination);
    }

    public static INamedTypeSymbol? FindAssignableTypeConstructedFrom(
        this ITypeSymbol type,
        INamedTypeSymbol baseType)
    {
        IEnumerable<INamedTypeSymbol>? symbols = type.FindAssignableTypesConstructedFrom(baseType);

        return symbols
            .FirstOrDefault(current =>
                current.ConstructedFrom.Equals(baseType, SymbolEqualityComparer.Default));
    }

    public static IEnumerable<INamedTypeSymbol> FindAssignableTypesConstructedFrom(
        this ITypeSymbol type,
        INamedTypeSymbol baseType)
    {
        if (type is not INamedTypeSymbol namedTypeSymbol)
            return Enumerable.Empty<INamedTypeSymbol>();

        IEnumerable<INamedTypeSymbol> baseTypes = namedTypeSymbol.GetBaseTypesAndInterfaces();

        return baseTypes
            .Where(current =>
                current.ConstructedFrom.Equals(baseType, SymbolEqualityComparer.Default));
    }

    public static INamedTypeSymbol GetAssignableTypeConstructedFrom(
        this ITypeSymbol type,
        INamedTypeSymbol baseType)
    {
        INamedTypeSymbol? assignableType = FindAssignableTypeConstructedFrom(type, baseType);

        if (assignableType is null)
        {
            throw new ArgumentException($"Type {type} does not have assignable type constructed from {baseType}");
        }

        return assignableType;
    }

    public static ITypeSymbol GetEnumerableTypeArgument(this ITypeSymbol enumerableType, Compilation compilation)
    {
        if (enumerableType is IArrayTypeSymbol arrayTypeSymbol)
        {
            return arrayTypeSymbol.ElementType;
        }

        INamedTypeSymbol? genericEnumerableType = compilation.GetTypeSymbol(typeof(IEnumerable<>));

        INamedTypeSymbol? constructedFrom = enumerableType.FindAssignableTypeConstructedFrom(genericEnumerableType);

        if (constructedFrom is null)
        {
            throw new ArgumentException($"Type {enumerableType} is not generic enumerable");
        }

        return constructedFrom.TypeArguments.Single();
    }
}
