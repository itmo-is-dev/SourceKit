using Microsoft.CodeAnalysis;
using SourceKit.Generators.Builder.Extensions;

namespace SourceKit.Generators.Builder.Models;

public abstract record BuilderProperty
{
    public record Value(
        IPropertySymbol Symbol,
        ITypeSymbol Type,
        LiteralValue LiteralValue,
        bool IsBuilderConstructorParameter) : BuilderProperty(Symbol, IsBuilderConstructorParameter);

    public record Collection(
        IPropertySymbol Symbol,
        CollectionKind Kind,
        ITypeSymbol ElementType,
        bool IsBuilderConstructorParameter) : BuilderProperty(Symbol, IsBuilderConstructorParameter);

    private BuilderProperty(IPropertySymbol symbol, bool isBuilderConstructorParameter)
    {
        Symbol = symbol;
        IsBuilderConstructorParameter = isBuilderConstructorParameter;
        FieldName = symbol.Name.ToUnderscoreCamelCase();
    }

    public IPropertySymbol Symbol { get; }

    public bool IsBuilderConstructorParameter { get; }

    public string FieldName { get; }
}