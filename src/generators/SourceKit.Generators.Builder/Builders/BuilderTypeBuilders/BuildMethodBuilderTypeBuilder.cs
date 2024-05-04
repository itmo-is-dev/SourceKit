using FluentChaining;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;
using SourceKit.Generators.Builder.Commands;
using SourceKit.Generators.Builder.Models;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Generators.Builder.Builders.BuilderTypeBuilders;

public class BuildMethodBuilderTypeBuilder : ILink<BuilderTypeBuildingCommand, TypeDeclarationSyntax>
{
    public TypeDeclarationSyntax Process(
        BuilderTypeBuildingCommand request,
        SynchronousContext context,
        LinkDelegate<BuilderTypeBuildingCommand, SynchronousContext, TypeDeclarationSyntax> next)
    {
        ExpressionSyntax invocation = request.TypeSymbol.IsRecord
            ? GenerateRecordInvocation(request)
            : throw new NotSupportedException("Non record types are not supported");

        IdentifierNameSyntax returnType = IdentifierName(request.TypeSymbol.Name);

        MethodDeclarationSyntax method = MethodDeclaration(returnType, "Build")
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .AddBodyStatements(ReturnStatement(invocation));

        request = request with
        {
            BuilderSyntax = request.BuilderSyntax.AddMembers(method),
        };

        return next(request, context);
    }

    private ExpressionSyntax GenerateRecordInvocation(BuilderTypeBuildingCommand request)
    {
        ArgumentSyntax[] arguments = request.Properties
            .Select(x => ResolveArgument(x, request.Context.Compilation))
            .Select(Argument)
            .ToArray();

        return ObjectCreationExpression(IdentifierName(request.TypeSymbol.Name)).AddArgumentListArguments(arguments);
    }

    private ExpressionSyntax ResolveArgument(BuilderProperty property, Compilation compilation)
    {
        return property switch
        {
            BuilderProperty.Collection collection => ResolveCollectionArgument(collection, compilation),
            BuilderProperty.Value value => ResolveValueArgument(value),
            _ => throw new ArgumentOutOfRangeException(nameof(property)),
        };
    }

    private ExpressionSyntax ResolveCollectionArgument(BuilderProperty.Collection property, Compilation compilation)
    {
        INamedTypeSymbol comparableType = compilation.GetTypeSymbol<IComparable>();

        INamedTypeSymbol genericComparableType = compilation
            .GetTypeSymbol(typeof(IComparable<>))
            .Construct(property.ElementType);

        ExpressionSyntax expression = IdentifierName(property.FieldName);

        if (property.ElementType.IsAssignableTo(comparableType)
            || property.ElementType.IsAssignableTo(genericComparableType))
        {
            expression = InvocationExpression(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    expression,
                    IdentifierName("Distinct")));
        }

        return property.Kind switch
        {
            CollectionKind.Array => InvokeMethod(expression, IdentifierName("ToArray")),
            CollectionKind.List => InvokeMethod(expression, IdentifierName("ToList")),
            CollectionKind.HashSet => InvokeMethod(expression, IdentifierName("ToHashSet")),
            _ => throw new ArgumentOutOfRangeException(),
        };
    }

    private static ExpressionSyntax ResolveValueArgument(BuilderProperty.Value value)
    {
        return IdentifierName(value.FieldName);
    }

    private static InvocationExpressionSyntax InvokeMethod(ExpressionSyntax expression, SimpleNameSyntax name)
    {
        return InvocationExpression(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, expression, name));
    }
}