using FluentChaining;
using Humanizer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;
using SourceKit.Generators.Builder.Commands;
using SourceKit.Generators.Builder.Extensions;
using SourceKit.Generators.Builder.Models;
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
        IEnumerable<BuilderProperty.Collection> collectionProperties = request.Properties
            .OfType<BuilderProperty.Collection>();

        var genericEnumerableType = request.Context.Compilation.GetTypeSymbol(typeof(IEnumerable<>));

        foreach (var property in collectionProperties)
        {
            var constructedFrom = genericEnumerableType.Construct(property.ElementType);

            yield return GenerateAddSingleMethod(request.BuilderSyntax, property);
            yield return GenerateAddRangeMethod(request.BuilderSyntax, property.Symbol, constructedFrom);
        }
    }

    private MemberDeclarationSyntax GenerateAddSingleMethod(
        TypeDeclarationSyntax builder,
        BuilderProperty.Collection property)
    {
        const string parameterName = "element";

        var name = $"With{property.Symbol.Name.Singularize()}";
        var returnType = builder.Identifier;
        var fieldName = property.Symbol.Name.ToUnderscoreCamelCase();

        var parameter = Parameter(Identifier(parameterName)).WithType(property.ElementType.ToNameSyntax());

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