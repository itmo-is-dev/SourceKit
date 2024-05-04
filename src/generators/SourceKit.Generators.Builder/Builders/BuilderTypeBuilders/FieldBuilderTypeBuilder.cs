using FluentChaining;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;
using SourceKit.Generators.Builder.Commands;
using SourceKit.Generators.Builder.Models;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Generators.Builder.Builders.BuilderTypeBuilders;

public class FieldBuilderTypeBuilder : ILink<BuilderTypeBuildingCommand, TypeDeclarationSyntax>
{
    public TypeDeclarationSyntax Process(
        BuilderTypeBuildingCommand request,
        SynchronousContext context,
        LinkDelegate<BuilderTypeBuildingCommand, SynchronousContext, TypeDeclarationSyntax> next)
    {
        MemberDeclarationSyntax[] fields = request.Properties
            .Select(x => ResolveDeclaration(x, request.Context.Compilation))
            .WhereNotNull()
            .ToArray();

        request = request with
        {
            BuilderSyntax = request.BuilderSyntax.AddMembers(fields),
        };

        return next(request, context);
    }

    private static MemberDeclarationSyntax ResolveDeclaration(
        BuilderProperty property,
        Compilation compilation)
    {
        return property switch
        {
            BuilderProperty.Collection collection => ResolveEnumerable(collection, compilation),
            BuilderProperty.Value value => ResolveValue(value),
            _ => throw new ArgumentOutOfRangeException(nameof(property)),
        };
    }

    private static MemberDeclarationSyntax ResolveEnumerable(
        BuilderProperty.Collection property,
        Compilation compilation)
    {
        INamedTypeSymbol listType = compilation.GetTypeSymbol(typeof(List<>));

        INamedTypeSymbol constructedListType = listType.Construct(property.ElementType);
        TypeSyntax typeSyntax = constructedListType.ToNameSyntax();

        VariableDeclarationSyntax variableDeclaration = VariableDeclaration(
            typeSyntax,
            SingletonSeparatedList(VariableDeclarator(property.FieldName)));


        return FieldDeclaration(variableDeclaration)
            .AddModifiers(Token(SyntaxKind.PrivateKeyword), Token(SyntaxKind.ReadOnlyKeyword));
    }

    private static MemberDeclarationSyntax ResolveValue(BuilderProperty.Value property)
    {
        VariableDeclarationSyntax variableDeclaration = VariableDeclaration(
            property.Type.ToNameSyntax(),
            SingletonSeparatedList(VariableDeclarator(property.FieldName)));

        return FieldDeclaration(variableDeclaration).AddModifiers(Token(SyntaxKind.PrivateKeyword));
    }
}