using Microsoft.CodeAnalysis;

namespace SourceKit.Extensions;

public static class PropertySymbolExtensions
{
    public static bool IsAutoProperty(this IPropertySymbol property)
    {
        return property.ContainingType
            .GetMembers()
            .OfType<IFieldSymbol>()
            .Select(x => x.AssociatedSymbol)
            .Contains(property, SymbolEqualityComparer.Default);
    }
}