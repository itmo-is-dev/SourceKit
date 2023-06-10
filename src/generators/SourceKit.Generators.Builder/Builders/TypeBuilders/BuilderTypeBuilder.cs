using System.Collections;
using FluentChaining;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;
using SourceKit.Generators.Builder.Commands;
using SourceKit.Generators.Builder.Models;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Generators.Builder.Builders.TypeBuilders;

public class BuilderTypeBuilder : ILink<TypeBuildingCommand, TypeDeclarationSyntax>
{
    private readonly IChain<BuilderTypeBuildingCommand, TypeDeclarationSyntax> _chain;

    public BuilderTypeBuilder(IChain<BuilderTypeBuildingCommand, TypeDeclarationSyntax> chain)
    {
        _chain = chain;
    }

    public TypeDeclarationSyntax Process(
        TypeBuildingCommand request,
        SynchronousContext context,
        LinkDelegate<TypeBuildingCommand, SynchronousContext, TypeDeclarationSyntax> next)
    {
        var modifiers = TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.SealedKeyword));
        TypeDeclarationSyntax builderDeclaration = ClassDeclaration("Builder").WithModifiers(modifiers);

        var properties = request.Symbol
            .GetMembers()
            .OfType<IPropertySymbol>()
            .Where(x => x.IsImplicitlyDeclared is false)
            .Where(x => x.IsAutoProperty())
            .Select(x => MapToBuilderProperty(x, request.Context.Compilation))
            .ToArray();

        var command = new BuilderTypeBuildingCommand(
            request.Symbol,
            builderDeclaration,
            properties,
            request.Context);

        builderDeclaration = _chain.Process(command);

        request = request with
        {
            Syntax = request.Syntax.AddMembers(builderDeclaration),
        };

        return next(request, context);
    }

    private BuilderProperty MapToBuilderProperty(IPropertySymbol propertySymbol, Compilation compilation)
    {
        var type = propertySymbol.Type;
        var enumerableType = compilation.GetTypeSymbol<IEnumerable>();

        if (type is not IArrayTypeSymbol && type.IsAssignableTo(enumerableType) is false)
            return new BuilderProperty.Value(propertySymbol, type);

        var elementType = type.GetEnumerableTypeArgument(compilation);

        if (type is IArrayTypeSymbol)
            return new BuilderProperty.Collection(propertySymbol, type, CollectionKind.Array, elementType);

        var listType = compilation.GetTypeSymbol(typeof(List<>)).Construct(elementType);

        if (type.Equals(listType, SymbolEqualityComparer.Default))
            return new BuilderProperty.Collection(propertySymbol, type, CollectionKind.List, elementType);

        var setType = compilation.GetTypeSymbol(typeof(HashSet<>)).Construct(elementType);

        if (type.Equals(setType, SymbolEqualityComparer.Default))
            return new BuilderProperty.Collection(propertySymbol, type, CollectionKind.HashSet, elementType);

        return new BuilderProperty.Collection(propertySymbol, type, CollectionKind.Array, elementType);
    }
}