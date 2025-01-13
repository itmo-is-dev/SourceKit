using FluentChaining;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;
using SourceKit.Generators.Builder.Commands;
using SourceKit.Generators.Builder.Models;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Generators.Builder.Builders.BuilderTypeBuilders;

public class ConstructorBuilderTypeBuilder : ILink<BuilderTypeBuildingCommand, TypeDeclarationSyntax>
{
    public TypeDeclarationSyntax Process(
        BuilderTypeBuildingCommand request,
        SynchronousContext context,
        LinkDelegate<BuilderTypeBuildingCommand, SynchronousContext, TypeDeclarationSyntax> next)
    {
        ParameterSyntax[] parameters = ResolveParameters(request).ToArray();
        StatementSyntax[] statements = ResolveStatements(request).ToArray();

        ConstructorDeclarationSyntax constructor = ConstructorDeclaration(request.BuilderSyntax.Identifier)
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .AddParameterListParameters(parameters)
            .AddBodyStatements(statements);

        request = request with
        {
            BuilderSyntax = request.BuilderSyntax.AddMembers(constructor),
        };

        return next(request, context);
    }

    private static IEnumerable<ParameterSyntax> ResolveParameters(BuilderTypeBuildingCommand request)
    {
        foreach (BuilderProperty property in request.Properties)
        {
            if (property.IsBuilderConstructorParameter is false)
                continue;

            yield return property switch
            {
                BuilderProperty.Collection collection => ResolveEnumerableParameter(collection),
                BuilderProperty.Value value => ResolveValueParameter(value),
                _ => throw new ArgumentOutOfRangeException(nameof(property)),
            };
        }
    }

    private static ParameterSyntax ResolveEnumerableParameter(BuilderProperty.Collection collection)
    {
        GenericNameSyntax parameterType = GenericName("IEnumerable")
            .AddTypeArgumentListArguments(collection.ElementType.ToNameSyntax(includeGlobal: true));

        return Parameter(Identifier(collection.Symbol.Name)).WithType(parameterType);
    }

    private static ParameterSyntax ResolveValueParameter(BuilderProperty.Value value)
    {
        ParameterSyntax parameter = Parameter(Identifier(value.Symbol.Name))
            .WithType(value.Symbol.Type.ToNameSyntax(includeGlobal: true));

        if (value.LiteralValue.TryGetLiteralExpression(out ExpressionSyntax? literal))
        {
            parameter = parameter.WithDefault(EqualsValueClause(literal));
        }

        return parameter;
    }

    private static IEnumerable<StatementSyntax> ResolveStatements(BuilderTypeBuildingCommand request)
    {
        return request.Properties.Select(
            p => p switch
            {
                BuilderProperty.Collection collection
                    => ResolveEnumerableStatement(collection, request.Context.Compilation),

                BuilderProperty.Value value
                    => ResolveValueStatement(value, request.Context.Compilation),

                _ => throw new ArgumentOutOfRangeException(nameof(p)),
            });
    }

    private static StatementSyntax ResolveEnumerableStatement(
        BuilderProperty.Collection property,
        Compilation compilation)
    {
        INamedTypeSymbol listType = compilation.GetTypeSymbol(typeof(List<>));

        INamedTypeSymbol constructedListType = listType.Construct(property.ElementType);
        TypeSyntax typeSyntax = constructedListType.ToNameSyntax(includeGlobal: true);

        ArgumentListSyntax arguments = property.IsBuilderConstructorParameter
            ? ArgumentList().AddArguments(Argument(IdentifierName(property.Symbol.Name)))
            : ArgumentList();

        return ExpressionStatement(
            AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression,
                IdentifierName(property.FieldName),
                ObjectCreationExpression(typeSyntax).WithArgumentList(arguments)));
    }

    private static StatementSyntax ResolveValueStatement(
        BuilderProperty.Value property,
        Compilation compilation)
    {
        if (property.IsBuilderConstructorParameter)
        {
            return ExpressionStatement(
                AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    IdentifierName(property.FieldName),
                    IdentifierName(property.Symbol.Name)));
        }

        ExpressionSyntax value = property.LiteralValue.TryGetLiteralExpression(out ExpressionSyntax? literal)
            ? literal
            : ResolveDefaultValue(property.Type, compilation);

        return ExpressionStatement(
            AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression,
                IdentifierName(property.FieldName),
                value));
    }

    private static ExpressionSyntax ResolveDefaultValue(ITypeSymbol type, Compilation compilation)
    {
        INamedTypeSymbol stringType = compilation.GetTypeSymbol<string>();

        if (type.Equals(stringType, SymbolEqualityComparer.Default)
            && type.NullableAnnotation is not NullableAnnotation.Annotated)
        {
            return MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                PredefinedType(Token(SyntaxKind.StringKeyword)),
                IdentifierName("Empty"));
        }

        return LiteralExpression(SyntaxKind.DefaultLiteralExpression, Token(SyntaxKind.DefaultKeyword));
    }
}