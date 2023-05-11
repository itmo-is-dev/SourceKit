using FluentChaining;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;
using SourceKit.Generators.Builder.Builders.FileBuilders;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Generators.Builder.Builders.BuilderTypeBuilders;

public class FieldBuilderTypeBuilder : ILink<BuilderTypeBuildingCommand, TypeDeclarationSyntax>
{
    public TypeDeclarationSyntax Process(
        BuilderTypeBuildingCommand request,
        SynchronousContext context,
        LinkDelegate<BuilderTypeBuildingCommand, SynchronousContext, TypeDeclarationSyntax> next)
    {
        var properties = request.TypeSyntax.Members
            .OfType<PropertyDeclarationSyntax>()
            .Select(x => (name: x.Identifier.ToString(), type: x.Type));

        var fieldNames = properties
            .Select(x => x with { name = x.name.ToUnderscoreCamelCase() });

        var fields = fieldNames
            .Select(x => VariableDeclaration(x.type, SingletonSeparatedList(VariableDeclarator(x.name))))
            .Select(x => (MemberDeclarationSyntax)FieldDeclaration(x))
            .ToArray();

        request = request with
        {
            BuilderSyntax = request.BuilderSyntax.AddMembers(fields)
        };

        return next(request, context);
    }

    private TypeSyntax ResolveType(
        TypeSyntax typeSyntax,
        INamedTypeSymbol enumerableTypeSymbol,
        SemanticModel semanticModel)
    {
        
    }
}