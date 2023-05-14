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

public class BuildMethodBuilderTypeBuilder : ILink<BuilderTypeBuildingCommand, TypeDeclarationSyntax>
{
    public TypeDeclarationSyntax Process(
        BuilderTypeBuildingCommand request,
        SynchronousContext context,
        LinkDelegate<BuilderTypeBuildingCommand, SynchronousContext, TypeDeclarationSyntax> next)
    {
        var invocation = request.TypeSymbol.IsRecord
            ? GenerateRecordInvocation(request)
            : GenerateInvocation(request);

        var returnType = IdentifierName(request.TypeSymbol.Name);

        var method = MethodDeclaration(returnType, "Build")
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
        ArgumentSyntax[] arguments = request.TypeSymbol.GetMembers()
            .OfType<IPropertySymbol>()
            .Where(x => x.IsImplicitlyDeclared is false)
            .Select(x => ResolveArgument(x, request.Context.Compilation))
            .Select(Argument)
            .ToArray();

        return ObjectCreationExpression(IdentifierName(request.TypeSymbol.Name)).AddArgumentListArguments(arguments);
    }

    private ExpressionSyntax GenerateInvocation(BuilderTypeBuildingCommand request)
    {
        throw new NotSupportedException("Non record types are not supported");
    }

    private ExpressionSyntax ResolveArgument(IPropertySymbol property, Compilation compilation)
    {
        var enumerableType = compilation.GetTypeSymbol<IEnumerable>();

        ExpressionSyntax expression = IdentifierName(property.Name.ToUnderscoreCamelCase());

        if (property.Type.IsAssignableTo(enumerableType) is false)
            return expression;

        var genericEnumerableType = compilation.GetTypeSymbol(typeof(IEnumerable<>));
        var constructedFrom = property.Type.GetAssignableTypeConstructedFrom(genericEnumerableType);

        var elementType = constructedFrom.TypeArguments.Single();

        var comparableType = compilation.GetTypeSymbol<IComparable>();
        var genericComparableType = compilation.GetTypeSymbol(typeof(IComparable<>)).Construct(elementType);

        if (elementType.IsAssignableTo(comparableType) || elementType.IsAssignableTo(genericComparableType))
        {
            expression = InvocationExpression(MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                expression,
                IdentifierName("Distinct")));
        }

        var listType = compilation.GetTypeSymbol(typeof(List<>)).Construct(elementType);

        if (property.Type.Equals(listType, SymbolEqualityComparer.Default))
        {
            return InvokeMethod(expression, IdentifierName("ToList"));
        }

        var setType = compilation.GetTypeSymbol(typeof(HashSet<>)).Construct(elementType);

        if (property.Type.Equals(setType, SymbolEqualityComparer.Default))
        {
            return InvokeMethod(expression, IdentifierName("ToHashSet"));
        }

        return InvocationExpression(MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            expression,
            IdentifierName("ToArray")));
    }

    private InvocationExpressionSyntax InvokeMethod(ExpressionSyntax expression, SimpleNameSyntax name)
    {
        return InvocationExpression(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, expression, name));
    }
}