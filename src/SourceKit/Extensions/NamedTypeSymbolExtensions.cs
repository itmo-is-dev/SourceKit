using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceKit.Extensions;

public static class NamedTypeSymbolExtensions
{
    public static IEnumerable<INamedTypeSymbol> GetBaseTypes(this INamedTypeSymbol symbol)
    {
        return symbol.BaseType is null
            ? Enumerable.Empty<INamedTypeSymbol>()
            : Enumerable.Repeat(symbol.BaseType, 1).Concat(symbol.BaseType.GetBaseTypes());
    }

    public static IEnumerable<INamedTypeSymbol> GetBaseTypesAndInterfaces(this INamedTypeSymbol symbol)
    {
        return symbol.GetBaseTypes().Concat(symbol.AllInterfaces);
    }

    public static bool HasAttribute(this INamedTypeSymbol symbol, INamedTypeSymbol attribute)
    {
        return symbol
            .GetAttributes()
            .Select(x => x.AttributeClass)
            .WhereNotNull()
            .Contains(attribute, SymbolEqualityComparer.Default);
    }

    public static IEnumerable<TypeDeclarationSyntax> GetDeclarations(this INamedTypeSymbol symbol)
    {
        return symbol.Locations
            .Select(x => (location: x, x.SourceTree))
            .Where(x => x.SourceTree is not null)
            .Select(x => x.SourceTree!.GetRoot().FindNode(x.location.SourceSpan))
            .OfType<TypeDeclarationSyntax>();
    }

    public static bool IsPartial(this INamedTypeSymbol symbol)
    {
        return symbol
            .GetDeclarations()
            .SelectMany(x => x.Modifiers)
            .Any(x => x.IsKind(SyntaxKind.PartialKeyword));
    }

    public static IEnumerable<Location> GetSignatureLocations(this INamedTypeSymbol symbol)
    {
        return symbol.GetDeclarations().Select(x => x.Identifier.GetLocation());
    }
}