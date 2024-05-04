using FluentChaining;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;
using SourceKit.Generators.Builder.Commands;
using SourceKit.Generators.Builder.Models;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Generators.Builder.Builders.TypeBuilders;

public class BuildMethodTypeBuilder : ILink<TypeBuildingCommand, TypeDeclarationSyntax>
{
    public TypeDeclarationSyntax Process(
        TypeBuildingCommand request,
        SynchronousContext context,
        LinkDelegate<TypeBuildingCommand, SynchronousContext, TypeDeclarationSyntax> next)
    {
        GenericNameSyntax funcType = GenericName(Identifier("Func"))
            .AddTypeArgumentListArguments(
                IdentifierName("Builder"),
                IdentifierName("Builder"));

        ParameterSyntax[] parameters = ResolveParameters(request)
            .Append(Parameter(Identifier("action")).WithType(funcType))
            .ToArray();

        ArgumentSyntax[] builderCreationArguments = request.Properties
            .Where(x => x.IsBuilderConstructorParameter)
            .Select(x => Argument(IdentifierName(x.Symbol.Name)))
            .ToArray();

        ObjectCreationExpressionSyntax builderCreation = ObjectCreationExpression(IdentifierName("Builder"))
            .AddArgumentListArguments(builderCreationArguments);

        InvocationExpressionSyntax builderInvocation = InvocationExpression(IdentifierName("action"))
            .AddArgumentListArguments(Argument(builderCreation));

        InvocationExpressionSyntax buildInvocation = InvocationExpression(
            MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                builderInvocation,
                IdentifierName("Build")));

        MethodDeclarationSyntax method = MethodDeclaration(IdentifierName(request.Symbol.Name), Identifier("Build"))
            .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword))
            .AddParameterListParameters(parameters)
            .AddBodyStatements(ReturnStatement(buildInvocation));

        request = request with
        {
            Syntax = request.Syntax.AddMembers(method),
        };

        return next(request, context);
    }

    private static IEnumerable<ParameterSyntax> ResolveParameters(TypeBuildingCommand command)
    {
        foreach (BuilderProperty property in command.Properties)
        {
            if (property.IsBuilderConstructorParameter is false)
                continue;

            if (property is BuilderProperty.Value value)
            {
                yield return Parameter(Identifier(value.Symbol.Name)).WithType(value.Type.ToNameSyntax());
            }
            else if (property is BuilderProperty.Collection collection)
            {
                GenericNameSyntax type = GenericName("IEnumerable")
                    .AddTypeArgumentListArguments(collection.ElementType.ToNameSyntax());

                yield return Parameter(Identifier(collection.Symbol.Name)).WithType(type);
            }
        }
    }
}