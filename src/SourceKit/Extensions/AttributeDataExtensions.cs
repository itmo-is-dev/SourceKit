using Microsoft.CodeAnalysis;

namespace SourceKit.Extensions;

public static class AttributeDataExtensions
{
    public static bool IsAttribute(this AttributeData data, INamedTypeSymbol attribute)
        => data.AttributeClass?.Equals(attribute, SymbolEqualityComparer.Default) is true;

    public static bool HasAttribute(this IEnumerable<AttributeData> attributes, INamedTypeSymbol attribute)
        => attributes.Any(x => x.IsAttribute(attribute));
}