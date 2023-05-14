using System.Collections;
using FluentChaining;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;
using SourceKit.Generators.Builder.Commands;
using SourceKit.Generators.Builder.Extensions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Generators.Builder.Builders.BuilderTypeBuilders;

public class ConstructorBuilderTypeBuilder : ILink<BuilderTypeBuildingCommand, TypeDeclarationSyntax>
{
    public TypeDeclarationSyntax Process(
        BuilderTypeBuildingCommand request,
        SynchronousContext context,
        LinkDelegate<BuilderTypeBuildingCommand, SynchronousContext, TypeDeclarationSyntax> next)
    {
        var statements = ResolveStatements(request).ToArray();

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
        var properties = request.TypeSymbol
            .GetMembers()
            .OfType<IPropertySymbol>()
            .Where(x => x.IsImplicitlyDeclared is false);

        var enumerableType = request.Context.Compilation.GetTypeSymbol<IEnumerable>();

        foreach (var property in properties)
        {
            if (property.Type is not INamedTypeSymbol type)
                continue;

            if (type.IsAssignableTo(enumerableType))
            {
                yield return ResolveEnumerableStatement(property, type, request.Context.Compilation);
            }
            else
            {
                yield return ResolveStatement(property, type, request.Context.Compilation);
            }
        }
    }

    private StatementSyntax ResolveEnumerableStatement(
        IPropertySymbol property,
        INamedTypeSymbol propertyType,
        Compilation compilation)
    {
        var listType = compilation.GetTypeSymbol(typeof(List<>));
        var genericEnumerableType = compilation.GetTypeSymbol(typeof(IEnumerable<>));

        var constructedFrom = propertyType.GetAssignableTypeConstructedFrom(genericEnumerableType);

        var elementType = constructedFrom.TypeArguments.Single();
        var constructedListType = listType.Construct(elementType);
        var typeSyntax = constructedListType.ToNameSyntax();

        var fieldName = property.Name.ToUnderscoreCamelCase();

        return ExpressionStatement(AssignmentExpression(
            SyntaxKind.SimpleAssignmentExpression,
            IdentifierName(fieldName),
            ObjectCreationExpression(typeSyntax).WithArgumentList(ArgumentList())));
    }

    private StatementSyntax ResolveStatement(
        IPropertySymbol property,
        INamedTypeSymbol propertyType,
        Compilation compilation)
    {
        var stringType = compilation.GetTypeSymbol<string>();

        ExpressionSyntax value;

        if (propertyType.Equals(stringType, SymbolEqualityComparer.IncludeNullability))
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

        var fieldName = property.Name.ToUnderscoreCamelCase();

        return ExpressionStatement(AssignmentExpression(
            SyntaxKind.SimpleAssignmentExpression,
            IdentifierName(fieldName),
            value));
    }
}