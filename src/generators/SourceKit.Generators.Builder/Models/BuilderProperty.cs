using Microsoft.CodeAnalysis;

namespace SourceKit.Generators.Builder.Models;

public abstract record BuilderProperty
{
    private BuilderProperty(IPropertySymbol symbol)
    {
        Symbol = symbol;
    }

    public IPropertySymbol Symbol { get; }

    public record Value(IPropertySymbol Symbol, ITypeSymbol Type) : BuilderProperty(Symbol);

    public record Collection(
        IPropertySymbol Symbol,
        ITypeSymbol Type,
        CollectionKind Kind,
        ITypeSymbol ElementType) : BuilderProperty(Symbol);
}