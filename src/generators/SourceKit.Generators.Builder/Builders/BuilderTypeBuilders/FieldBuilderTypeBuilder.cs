using FluentChaining;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;
using SourceKit.Generators.Builder.Commands;
using SourceKit.Generators.Builder.Extensions;
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

    private MemberDeclarationSyntax ResolveDeclaration(
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

    private MemberDeclarationSyntax ResolveEnumerable(
        BuilderProperty.Collection property,
        Compilation compilation)
    {
        var listType = compilation.GetTypeSymbol(typeof(List<>));

        var constructedListType = listType.Construct(property.ElementType);
        var typeSyntax = constructedListType.ToNameSyntax();

        var name = property.Symbol.Name.ToUnderscoreCamelCase();
        var variableDeclaration = VariableDeclaration(typeSyntax, SingletonSeparatedList(VariableDeclarator(name)));

        return FieldDeclaration(variableDeclaration)
            .AddModifiers(Token(SyntaxKind.PrivateKeyword), Token(SyntaxKind.ReadOnlyKeyword));
    }

    private MemberDeclarationSyntax ResolveValue(BuilderProperty.Value property)
    {
        var name = property.Symbol.Name.ToUnderscoreCamelCase();

        var variableDeclaration = VariableDeclaration(
            property.Type.ToNameSyntax(),
            SingletonSeparatedList(VariableDeclarator(name)));

        return FieldDeclaration(variableDeclaration).AddModifiers(Token(SyntaxKind.PrivateKeyword));
    }
}