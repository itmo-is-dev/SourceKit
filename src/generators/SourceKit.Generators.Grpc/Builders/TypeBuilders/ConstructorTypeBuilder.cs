using FluentChaining;
using Humanizer;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;
using SourceKit.Generators.Grpc.Commands;
using SourceKit.Generators.Grpc.Models;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Generators.Grpc.Builders.TypeBuilders;

public class ConstructorTypeBuilder : ILink<TypeBuildingCommand, TypeDeclarationSyntax>
{
    public TypeDeclarationSyntax Process(
        TypeBuildingCommand request,
        SynchronousContext context,
        LinkDelegate<TypeBuildingCommand, SynchronousContext, TypeDeclarationSyntax> next)
    {
        var constructor = ConstructorDeclaration(Identifier(request.Message.Type.Name))
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .AddParameterListParameters(MapParameters(request).ToArray())
            .AddBodyStatements(MapAssignments(request).ToArray());

        request = request with
        {
            Syntax = request.Syntax.AddMembers(constructor),
        };

        return next(request, context);
    }

    private static IEnumerable<ParameterSyntax> MapParameters(TypeBuildingCommand command)
    {
        var repeatedFieldType = command.Context.Compilation
            .GetTypeByMetadataName(Constants.ProtobufRepeatedFieldFullyQualifiedName);

        if (repeatedFieldType is null)
            yield break;

        foreach (var property in command.Message.Properties.OrderBy(x => x.Ordinal))
        {
            TypeSyntax typeName;

            if (property is RepeatableProtoProperty repeatableProperty)
            {
                var typeArguments = TypeArgumentList(
                    SingletonSeparatedList(repeatableProperty.ElementType.ToNameSyntax()));

                typeName = GenericName(Identifier("IEnumerable"), typeArguments);
            }
            else if (property is MapProtoProperty mapProperty)
            {
                var typeArguments = TypeArgumentList(SeparatedList(new[]
                {
                    mapProperty.Key.ToNameSyntax(),
                    mapProperty.Value.ToNameSyntax(),
                }));

                var keyValuePair = GenericName(Identifier("KeyValuePair"), typeArguments);

                typeName = GenericName(
                    Identifier("IEnumerable"),
                    TypeArgumentList(SeparatedList<TypeSyntax>(new[] { keyValuePair })));
            }
            else if (property is ValueProtoProperty valueProperty)
            {
                typeName = valueProperty.Type.ToNameSyntax();
            }
            else
            {
                continue;
            }

            var parameterName = property.Name.Camelize();

            yield return Parameter(Identifier(parameterName)).WithType(typeName);
        }
    }

    private static IEnumerable<StatementSyntax> MapAssignments(TypeBuildingCommand command)
    {
        var repeatedFieldType = command.Context.Compilation
            .GetTypeByMetadataName(Constants.ProtobufRepeatedFieldFullyQualifiedName);

        if (repeatedFieldType is null)
            yield break;

        foreach (var property in command.Message.Properties.OrderBy(x => x.Ordinal))
        {
            var parameterName = property.Name.Camelize();

            yield return property switch
            {
                RepeatableProtoProperty => CreateRepeatableAssignment(property, parameterName),
                MapProtoProperty mapProperty => CreateMapAssignment(mapProperty, property, parameterName),
                OneOfProtoProperty oneOfProperty => CreateOneOfAssignment(oneOfProperty, property, parameterName),

                _ => ExpressionStatement(AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    IdentifierName(property.Name),
                    IdentifierName(parameterName))),
            };
        }
    }

    private static StatementSyntax CreateRepeatableAssignment(ProtoProperty property, string parameterName)
    {
        var memberAccess = MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
            IdentifierName(property.Name),
            IdentifierName("Add"));

        var invocation = InvocationExpression(memberAccess)
            .AddArgumentListArguments(Argument(IdentifierName(parameterName)));

        return ExpressionStatement(invocation);
    }

    private static StatementSyntax CreateMapAssignment(
        MapProtoProperty mapProperty,
        ProtoProperty property,
        string parameterName)
    {
        const string itemName = "item";

        var typeArguments = TypeArgumentList(SeparatedList<TypeSyntax>(new[]
        {
            mapProperty.Key.ToTypeArgumentSyntax(),
            mapProperty.Value.ToTypeArgumentSyntax(),
        }));

        var keyValuePair = GenericName(Identifier("KeyValuePair"), typeArguments);

        var addMethod = InvocationExpression(MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            IdentifierName(property.Name),
            IdentifierName("Add")));

        var key = Argument(
            MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                IdentifierName(itemName),
                IdentifierName("Key")));

        var value = Argument(
            MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                IdentifierName(itemName),
                IdentifierName("Value")));

        var body = Block(SeparatedList<StatementSyntax>(new[]
        {
            ExpressionStatement(addMethod.AddArgumentListArguments(key, value)),
        }));

        return ForEachStatement(
            keyValuePair,
            Identifier(itemName),
            IdentifierName(parameterName),
            body);
    }

    private static StatementSyntax CreateOneOfAssignment(
        OneOfProtoProperty oneOfProperty,
        ProtoProperty property,
        string parameterName)
    {
        var condition = IsPatternExpression(
            IdentifierName(parameterName),
            UnaryPattern(ConstantPattern(LiteralExpression(SyntaxKind.NullLiteralExpression))));

        ExpressionSyntax value = oneOfProperty.Type.IsValueType
            ? MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                IdentifierName(parameterName),
                IdentifierName("Value"))
            : IdentifierName(parameterName);

        var statement = ExpressionStatement(AssignmentExpression(
            SyntaxKind.SimpleAssignmentExpression,
            IdentifierName(property.Name),
            value));

        return IfStatement(condition, Block(statement));
    }
}