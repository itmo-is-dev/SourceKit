using FluentChaining;
using Humanizer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;
using SourceKit.Generators.Builder.Commands;
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

        INamedTypeSymbol genericEnumerableType = request.Context.Compilation.GetTypeSymbol(typeof(IEnumerable<>));

        foreach (BuilderProperty.Collection property in collectionProperties)
        {
            INamedTypeSymbol constructedFrom = genericEnumerableType.Construct(property.ElementType);

            yield return GenerateAddSingleMethod(request.BuilderSyntax, property);
            yield return GenerateAddRangeMethod(request.BuilderSyntax, property, constructedFrom);
        }
    }

    private MemberDeclarationSyntax GenerateAddSingleMethod(
        TypeDeclarationSyntax builder,
        BuilderProperty.Collection property)
    {
        const string parameterName = "element";

        var name = $"With{property.Symbol.Name.Singularize()}";
        SyntaxToken returnType = builder.Identifier;

        ParameterSyntax parameter = Parameter(Identifier(parameterName)).WithType(property.ElementType.ToNameSyntax());

        MemberAccessExpressionSyntax addMethod = MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            IdentifierName(property.FieldName),
            IdentifierName("Add"));

        ArgumentSyntax argument = Argument(IdentifierName(parameterName));
        InvocationExpressionSyntax invocation = InvocationExpression(addMethod).AddArgumentListArguments(argument);
        ReturnStatementSyntax returnStatement = ReturnStatement(ThisExpression());

        return MethodDeclaration(IdentifierName(returnType), name)
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .AddParameterListParameters(parameter)
            .AddBodyStatements(ExpressionStatement(invocation), returnStatement)
            .AddAttributeLists(new InitializesPropertyAttributeBuilder(property.Symbol.Name));
    }

    private MemberDeclarationSyntax GenerateAddRangeMethod(
        TypeDeclarationSyntax builder,
        BuilderProperty.Collection property,
        INamedTypeSymbol enumerableType)
    {
        const string parameterName = "elements";

        var name = $"With{property.Symbol.Name.Pluralize()}";
        SyntaxToken returnType = builder.Identifier;

        ParameterSyntax parameter = Parameter(Identifier(parameterName)).WithType(enumerableType.ToNameSyntax());

        MemberAccessExpressionSyntax addRangeMethod = MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            IdentifierName(property.FieldName),
            IdentifierName("AddRange"));

        ArgumentSyntax argument = Argument(IdentifierName(parameterName));
        InvocationExpressionSyntax invocation = InvocationExpression(addRangeMethod).AddArgumentListArguments(argument);
        ReturnStatementSyntax returnStatement = ReturnStatement(ThisExpression());

        return MethodDeclaration(IdentifierName(returnType), name)
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .AddParameterListParameters(parameter)
            .AddBodyStatements(ExpressionStatement(invocation), returnStatement)
            .AddAttributeLists(new InitializesPropertyAttributeBuilder(property.Symbol.Name));
    }
}