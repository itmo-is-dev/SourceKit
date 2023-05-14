using FluentChaining;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Generators.Builder.Commands;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Generators.Builder.Builders.TypeBuilders;

public class BuildMethodTypeBuilder : ILink<TypeBuildingCommand, TypeDeclarationSyntax>
{
    public TypeDeclarationSyntax Process(
        TypeBuildingCommand request,
        SynchronousContext context,
        LinkDelegate<TypeBuildingCommand, SynchronousContext, TypeDeclarationSyntax> next)
    {
        var funcType = GenericName(Identifier("Func"))
            .AddTypeArgumentListArguments(
                IdentifierName("Builder"),
                IdentifierName("Builder"));

        var builderCreation = ObjectCreationExpression(IdentifierName("Builder")).WithArgumentList(ArgumentList());

        var builderInvocation = InvocationExpression(IdentifierName("action"))
            .AddArgumentListArguments(Argument(builderCreation));

        var buildInvocation = InvocationExpression(MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            builderInvocation,
            IdentifierName("Build")));

        var method = MethodDeclaration(IdentifierName(request.Symbol.Name), Identifier("Build"))
            .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword))
            .AddParameterListParameters(Parameter(Identifier("action")).WithType(funcType))
            .AddBodyStatements(ReturnStatement(buildInvocation));

        request = request with
        {
            Syntax = request.Syntax.AddMembers(method),
        };

        return next(request, context);
    }
}