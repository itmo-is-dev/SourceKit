using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Extensions;

public static class TypeSymbolExtensions
{
    public static IEnumerable<IdentifierNameSyntax> ToTypeArgumentSyntax(this IEnumerable<ITypeSymbol> symbols)
        => symbols.Select(ToTypeArgumentSyntax);

    public static IdentifierNameSyntax ToTypeArgumentSyntax(this ITypeSymbol symbol)
        => IdentifierName(symbol.GetFullyQualifiedName());

    public static bool IsAssignableTo(this ITypeSymbol source, ITypeSymbol destination)
    {
        return source is INamedTypeSymbol namedSource
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
        if (type is not INamedTypeSymbol namedTypeSymbol)
            return null;

        IEnumerable<INamedTypeSymbol> baseTypes = namedTypeSymbol.GetBaseTypesAndInterfaces();

        return baseTypes
            .FirstOrDefault(current => current.ConstructedFrom.Equals(baseType, SymbolEqualityComparer.Default));
    }

    public static INamedTypeSymbol GetAssignableTypeConstructedFrom(
        this ITypeSymbol type,
        INamedTypeSymbol baseType)
    {
        var assignableType = FindAssignableTypeConstructedFrom(type, baseType);

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

        var genericEnumerableType = compilation.GetTypeSymbol(typeof(IEnumerable<>));

        var constructedFrom = enumerableType.FindAssignableTypeConstructedFrom(genericEnumerableType);

        if (constructedFrom is null)
        {
            throw new ArgumentException($"Type {enumerableType} is not generic enumerable");
        }

        return constructedFrom.TypeArguments.Single();
    }

    public static INamedTypeSymbol? GetRequiredImplementedInterfaceByName(this ITypeSymbol symbol, string interfaceType)
    {
        return symbol.AllInterfaces.FirstOrDefault(
            x => x.ToString() == interfaceType);
    } 
}