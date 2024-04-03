using FluentChaining;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;
using SourceKit.Generators.Builder.Commands;
using SourceKit.Generators.Builder.Extensions;
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
        IEnumerable<BuilderProperty.Value> valueProperties = request.Properties.OfType<BuilderProperty.Value>();

        foreach (var property in valueProperties)
        {
            const string parameterName = "value";

            var name = $"With{property.Symbol.Name}";
            var returnType = request.BuilderSyntax.Identifier;
            var fieldName = property.Symbol.Name.ToUnderscoreCamelCase();

            var parameter = Parameter(Identifier(parameterName)).WithType(property.Type.ToNameSyntax());
            var returnStatement = ReturnStatement(ThisExpression());

            var assignment = AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression,
                IdentifierName(fieldName),
                IdentifierName(parameterName));

            yield return MethodDeclaration(IdentifierName(returnType), name)
                .AddModifiers(Token(SyntaxKind.PublicKeyword))
                .AddParameterListParameters(parameter)
                .AddBodyStatements(ExpressionStatement(assignment), returnStatement)
                .AddAttributeLists(new InitializesPropertyAttributeBuilder(property.Symbol.Name));
        }
    }
}