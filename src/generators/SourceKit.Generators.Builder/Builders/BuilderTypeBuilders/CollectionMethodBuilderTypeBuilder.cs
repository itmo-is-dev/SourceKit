using FluentChaining;
using Humanizer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;
using SourceKit.Generators.Builder.Commands;
using SourceKit.Generators.Builder.Extensions;
using SourceKit.Generators.Builder.Tools;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Generators.Builder.Builders.BuilderTypeBuilders;

public class CollectionMethodBuilderTypeBuilder : ILink<BuilderTypeBuildingCommand, TypeDeclarationSyntax>
{
    public TypeDeclarationSyntax Process(
        BuilderTypeBuildingCommand request,
        SynchronousContext context,
        LinkDelegate<BuilderTypeBuildingCommand, SynchronousContext, TypeDeclarationSyntax> next)
    {
        MemberDeclarationSyntax[] methods = GenerateMethods(request).ToArray();

        request = request with
        {
            BuilderSyntax = request.BuilderSyntax.AddMembers(methods),
        };

        return next(request, context);
    }

    private IEnumerable<MemberDeclarationSyntax> GenerateMethods(BuilderTypeBuildingCommand request)
    { 
        var enumerableType = request.Context.Compilation.GetTypeByMetadataName(Constants.EnumerableFullyQualifiedName);

        var genericEnumerableType = request.Context.Compilation
            .GetTypeByMetadataName(Constants.GenericEnumerableFullyQualifiedName);

        if (enumerableType is null || genericEnumerableType is null)
            yield break;

        foreach (IPropertySymbol property in request.Properties)
        {
            if (property.Type is not INamedTypeSymbol type)
                continue;

            if (type.IsAssignableTo(enumerableType) is false)
                continue;

            var constructedFrom = type.FindAssignableTypeConstructedFrom(genericEnumerableType);

            if (constructedFrom is null)
                continue;

            var elementType = constructedFrom.TypeArguments.Single();

            yield return GenerateAddSingleMethod(request.BuilderSyntax, property, elementType);
            yield return GenerateAddRangeMethod(request.BuilderSyntax, property, constructedFrom);
        }
    }

    private MemberDeclarationSyntax GenerateAddSingleMethod(
        TypeDeclarationSyntax builder,
        IPropertySymbol property,
        ITypeSymbol elementType)
    {
        const string parameterName = "element";

        var name = $"With{property.Name.Singularize()}";
        var returnType = builder.Identifier;
        var fieldName = property.Name.ToUnderscoreCamelCase();

        var parameter = Parameter(Identifier(parameterName)).WithType(elementType.ToNameSyntax());

        var addMethod = MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            IdentifierName(fieldName),
            IdentifierName("Add"));

        var argument = Argument(IdentifierName(parameterName));
        var invocation = InvocationExpression(addMethod).AddArgumentListArguments(argument);
        var returnStatement = ReturnStatement(ThisExpression());

        return MethodDeclaration(IdentifierName(returnType), name)
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .AddParameterListParameters(parameter)
            .AddBodyStatements(ExpressionStatement(invocation), returnStatement);
    }

    private MemberDeclarationSyntax GenerateAddRangeMethod(
        TypeDeclarationSyntax builder,
        IPropertySymbol property,
        INamedTypeSymbol enumerableType)
    {
        const string parameterName = "elements";

        var name = $"With{property.Name.Pluralize()}";
        var returnType = builder.Identifier;
        var fieldName = property.Name.ToUnderscoreCamelCase();

        var parameter = Parameter(Identifier(parameterName)).WithType(enumerableType.ToNameSyntax());

        var addRangeMethod = MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            IdentifierName(fieldName),
            IdentifierName("AddRange"));

        var argument = Argument(IdentifierName(parameterName));
        var invocation = InvocationExpression(addRangeMethod).AddArgumentListArguments(argument);
        var returnStatement = ReturnStatement(ThisExpression());

        return MethodDeclaration(IdentifierName(returnType), name)
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .AddParameterListParameters(parameter)
            .AddBodyStatements(ExpressionStatement(invocation), returnStatement);
    }
}