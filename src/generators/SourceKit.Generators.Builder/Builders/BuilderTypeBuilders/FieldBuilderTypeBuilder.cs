using FluentChaining;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;
using SourceKit.Generators.Builder.Commands;
using SourceKit.Generators.Builder.Extensions;
using SourceKit.Generators.Builder.Tools;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Generators.Builder.Builders.BuilderTypeBuilders;

public class FieldBuilderTypeBuilder : ILink<BuilderTypeBuildingCommand, TypeDeclarationSyntax>
{
    public TypeDeclarationSyntax Process(
        BuilderTypeBuildingCommand request,
        SynchronousContext context,
        LinkDelegate<BuilderTypeBuildingCommand, SynchronousContext, TypeDeclarationSyntax> next)
    {
        MemberDeclarationSyntax[] fields = request.TypeSymbol.GetMembers()
            .OfType<IPropertySymbol>()
            .Where(x => x.IsImplicitlyDeclared is false)
            .Select(x => ResolveDeclaration(x, request.Context.Compilation))
            .WhereNotNull()
            .ToArray();

        request = request with
        {
            BuilderSyntax = request.BuilderSyntax.AddMembers(fields),
        };

        return next(request, context);
    }

    private MemberDeclarationSyntax? ResolveDeclaration(
        IPropertySymbol symbol,
        Compilation compilation)
    {
        if (symbol.Type is not INamedTypeSymbol type)
            return null;

        var enumerableType = compilation.GetTypeByMetadataName(Constants.EnumerableFullyQualifiedName);

        if (enumerableType is null)
            return null;

        if (type.IsAssignableTo(enumerableType))
        {
            return ResolveEnumerable(symbol, type, compilation);
        }

        var name = symbol.Name.ToUnderscoreCamelCase();

        var variableDeclaration = VariableDeclaration(
            IdentifierName(symbol.Type.Name),
            SingletonSeparatedList(VariableDeclarator(name)));

        return FieldDeclaration(variableDeclaration).AddModifiers(Token(SyntaxKind.PrivateKeyword));
    }

    private MemberDeclarationSyntax? ResolveEnumerable(
        IPropertySymbol symbol,
        INamedTypeSymbol type,
        Compilation compilation)
    {
        var listType = compilation.GetTypeByMetadataName(Constants.ListFullyQualifiedName);
        var genericEnumerableType = compilation.GetTypeByMetadataName(Constants.GenericEnumerableFullyQualifiedName);

        if (listType is null || genericEnumerableType is null)
            return null;

        var constructedFrom = type.FindAssignableTypeConstructedFrom(genericEnumerableType);

        if (constructedFrom is null)
            return null;

        var typeArgument = constructedFrom.TypeArguments.Single();

        var constructedListType = listType.Construct(typeArgument);
        var typeSyntax = constructedListType.ToNameSyntax();

        var name = symbol.Name.ToUnderscoreCamelCase();
        var variableDeclaration = VariableDeclaration(typeSyntax, SingletonSeparatedList(VariableDeclarator(name)));

        return FieldDeclaration(variableDeclaration)
            .AddModifiers(Token(SyntaxKind.PrivateKeyword), Token(SyntaxKind.ReadOnlyKeyword));
    }
}