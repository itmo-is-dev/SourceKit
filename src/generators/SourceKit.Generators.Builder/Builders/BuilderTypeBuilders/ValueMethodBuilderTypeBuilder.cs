using FluentChaining;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;
using SourceKit.Generators.Builder.Commands;
using SourceKit.Generators.Builder.Models;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Generators.Builder.Builders.BuilderTypeBuilders;

public class ValueMethodBuilderTypeBuilder : ILink<BuilderTypeBuildingCommand, TypeDeclarationSyntax>
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
        IEnumerable<BuilderProperty.Value> valueProperties = request.Properties
            .OfType<BuilderProperty.Value>();

        foreach (BuilderProperty.Value property in valueProperties)
        {
            const string parameterName = "value";

            var name = $"With{property.Symbol.Name}";
            SyntaxToken returnType = request.BuilderSyntax.Identifier;

            ParameterSyntax parameter = Parameter(Identifier(parameterName)).WithType(property.Type.ToNameSyntax());
            ReturnStatementSyntax returnStatement = ReturnStatement(ThisExpression());

            AssignmentExpressionSyntax assignment = AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression,
                IdentifierName(property.FieldName),
                IdentifierName(parameterName));

            yield return MethodDeclaration(IdentifierName(returnType), name)
                .AddModifiers(Token(SyntaxKind.PublicKeyword))
                .AddParameterListParameters(parameter)
                .AddBodyStatements(ExpressionStatement(assignment), returnStatement)
                .AddAttributeLists(new InitializesPropertyAttributeBuilder(property.Symbol.Name));
        }
    }
}