using FluentChaining;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;
using SourceKit.Generators.Builder.Commands;
using SourceKit.Generators.Builder.Extensions;
using SourceKit.Generators.Builder.Tools;
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
        var enumerableType = request.Context.Compilation.GetTypeByMetadataName(Constants.EnumerableFullyQualifiedName);

        if (enumerableType is null)
            yield break;

        foreach (IPropertySymbol property in request.Properties)
        {
            if (property.Type is not INamedTypeSymbol type)
                continue;

            if (type.IsAssignableTo(enumerableType))
                continue;

            const string parameterName = "value";

            var name = $"With{property.Name}";
            var returnType = request.BuilderSyntax.Identifier;
            var fieldName = property.Name.ToUnderscoreCamelCase();

            var parameter = Parameter(Identifier(parameterName)).WithType(property.Type.ToNameSyntax());
            var returnStatement = ReturnStatement(ThisExpression());

            var assignment = AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression,
                IdentifierName(fieldName),
                IdentifierName(parameterName));

            yield return MethodDeclaration(IdentifierName(returnType), name)
                .AddModifiers(Token(SyntaxKind.PublicKeyword))
                .AddParameterListParameters(parameter)
                .AddBodyStatements(ExpressionStatement(assignment), returnStatement);
        }
    }
}