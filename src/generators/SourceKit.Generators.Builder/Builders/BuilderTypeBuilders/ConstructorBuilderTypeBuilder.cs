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

public class ConstructorBuilderTypeBuilder : ILink<BuilderTypeBuildingCommand, TypeDeclarationSyntax>
{
    public TypeDeclarationSyntax Process(
        BuilderTypeBuildingCommand request,
        SynchronousContext context,
        LinkDelegate<BuilderTypeBuildingCommand, SynchronousContext, TypeDeclarationSyntax> next)
    {
        StatementSyntax[] statements = ResolveStatements(request).ToArray();

        var constructor = ConstructorDeclaration(request.BuilderSyntax.Identifier)
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .AddBodyStatements(statements);

        request = request with
        {
            BuilderSyntax = request.BuilderSyntax.AddMembers(constructor),
        };

        return next(request, context);
    }

    private IEnumerable<StatementSyntax> ResolveStatements(BuilderTypeBuildingCommand request)
    {
        return request.Properties.Select(p => p switch
        {
            BuilderProperty.Collection collection
                => ResolveEnumerableStatement(collection, request.Context.Compilation),

            BuilderProperty.Value value
                => ResolveStatement(value, request.Context.Compilation),

            _ => throw new ArgumentOutOfRangeException(nameof(p)),
        });
    }

    private StatementSyntax ResolveEnumerableStatement(
        BuilderProperty.Collection property,
        Compilation compilation)
    {
        var listType = compilation.GetTypeSymbol(typeof(List<>));

        var constructedListType = listType.Construct(property.ElementType);
        var typeSyntax = constructedListType.ToNameSyntax();

        var fieldName = property.Symbol.Name.ToUnderscoreCamelCase();

        return ExpressionStatement(AssignmentExpression(
            SyntaxKind.SimpleAssignmentExpression,
            IdentifierName(fieldName),
            ObjectCreationExpression(typeSyntax).WithArgumentList(ArgumentList())));
    }

    private StatementSyntax ResolveStatement(
        BuilderProperty.Value property,
        Compilation compilation)
    {
        var stringType = compilation.GetTypeSymbol<string>();

        ExpressionSyntax value;

        if (property.Type.Equals(stringType, SymbolEqualityComparer.IncludeNullability))
        {
            value = MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                PredefinedType(Token(SyntaxKind.StringKeyword)),
                IdentifierName("Empty"));
        }
        else
        {
            value = LiteralExpression(SyntaxKind.DefaultLiteralExpression, Token(SyntaxKind.DefaultKeyword));
        }

        var fieldName = property.Symbol.Name.ToUnderscoreCamelCase();

        return ExpressionStatement(AssignmentExpression(
            SyntaxKind.SimpleAssignmentExpression,
            IdentifierName(fieldName),
            value));
    }
}