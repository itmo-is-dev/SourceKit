using Humanizer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;
using SourceKit.Generators.Builder.Annotations;
using SourceKit.Generators.Builder.Extensions;
using SourceKit.Generators.Builder.Models;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Generators.Builder.Generators;

public sealed record ValueBuilderParameter(
    SyntaxToken ParameterIdentifier,
    TypeSyntax ParameterTypeSyntax,
    bool IsReferenceType,
    bool IsNullableReferenceType,
    LiteralValue LiteralValue,
    BuilderPropertyOptions Options)
    : IBuilderParameter
{
    public bool Equals(IBuilderParameter other) => other is ValueBuilderParameter property && Equals(property);

    public void Configure(BuilderSyntax builder, Compilation compilation)
    {
        SyntaxToken storedMemberIdentifier = ConfigureStoredMember(builder);
        ConfigureBuilderConstructor(storedMemberIdentifier, builder);
        ConfigureWithMethod(builder, storedMemberIdentifier);
        ConfigureModelCreation(builder, storedMemberIdentifier);
    }

    private SyntaxToken ConfigureStoredMember(BuilderSyntax builder)
    {
        TypeSyntax valueTypeSyntax = ParameterTypeSyntax;

        if (IsNullableReferenceType && Options.HasFlag(BuilderPropertyOptions.ConstructorParameter) is false)
        {
            valueTypeSyntax = NullableType(valueTypeSyntax);
        }

        if (Options.HasFlag(BuilderPropertyOptions.Exposed))
        {
            SyntaxToken identifier = Identifier(ParameterIdentifier.Text.Pascalize());

            PropertyDeclarationSyntax propertySyntax = PropertyDeclaration(valueTypeSyntax, identifier)
                .AddModifiers(Token(SyntaxKind.PublicKeyword))
                .AddAccessorListAccessors(
                    AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
                    AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                        .AddModifiers(Token(SyntaxKind.PrivateKeyword))
                        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)));

            builder.AddMembers(propertySyntax);

            return identifier;
        }
        else
        {
            SyntaxToken identifier = Identifier(ParameterIdentifier.Text.ToUnderscoreCamelCase());

            VariableDeclarationSyntax variableSyntax = VariableDeclaration(valueTypeSyntax).AddVariables(VariableDeclarator(identifier));
            FieldDeclarationSyntax fieldSyntax = FieldDeclaration(variableSyntax).AddModifiers(Token(SyntaxKind.PrivateKeyword));

            builder.AddMembers(fieldSyntax);

            return identifier;
        }
    }

    private void ConfigureBuilderConstructor(
        SyntaxToken storedMemberIdentifier,
        BuilderSyntax builder)
    {
        if (Options.HasFlag(BuilderPropertyOptions.ConstructorParameter))
        {
            SyntaxToken parameterIdentifier = Identifier(ParameterIdentifier.Text.Camelize());

            ParameterSyntax parameter = Parameter(parameterIdentifier).WithType(ParameterTypeSyntax);

            if (LiteralValue.TryGetLiteralExpression(out ExpressionSyntax? literal))
            {
                parameter = parameter.WithDefault(EqualsValueClause(literal));
            }

            AssignmentExpressionSyntax assignmentExpression = AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression,
                IdentifierName(storedMemberIdentifier),
                IdentifierName(parameterIdentifier));

            builder.AddConstructorParameter(parameter);
            builder.AddConstructorStatements(ExpressionStatement(assignmentExpression));
        }
        else
        {
            ExpressionSyntax value = LiteralValue.TryGetLiteralExpression(out ExpressionSyntax? literal)
                ? literal
                : LiteralExpression(SyntaxKind.DefaultLiteralExpression, Token(SyntaxKind.DefaultKeyword));

            AssignmentExpressionSyntax assignmentExpression = AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression,
                IdentifierName(storedMemberIdentifier),
                value);

            builder.AddConstructorStatements(ExpressionStatement(assignmentExpression));
        }
    }

    private void ConfigureWithMethod(BuilderSyntax builder, SyntaxToken storedMemberIdentifier)
    {
        SyntaxToken withMethodIdentifier = Identifier($"With{ParameterIdentifier.Text}");
        SyntaxToken parameterIdentifier = Identifier("value");

        MethodDeclarationSyntax methodSyntax = MethodDeclaration(IdentifierName(BuilderSyntax.BuilderIdentifier), withMethodIdentifier)
            .AddAttributeLists(new InitializesPropertyAttributeBuilder(ParameterIdentifier))
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .AddParameterListParameters(Parameter(parameterIdentifier).WithType(ParameterTypeSyntax))
            .AddBodyStatements(
                ExpressionStatement(AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    IdentifierName(storedMemberIdentifier),
                    IdentifierName(parameterIdentifier))),
                ReturnStatement(ThisExpression()));

        builder.AddMembers(methodSyntax);
    }

    private void ConfigureModelCreation(BuilderSyntax builder, SyntaxToken storedMemberIdentifier)
    {
        ExpressionSyntax argumentExpression;

        if (IsReferenceType is false || IsNullableReferenceType)
        {
            argumentExpression = IdentifierName(storedMemberIdentifier);
        }
        else
        {
            ObjectCreationExpressionSyntax exception = ObjectCreationExpression(IdentifierName("global::System.ArgumentNullException"))
                .AddArgumentListArguments(Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(storedMemberIdentifier.Text))));

            argumentExpression = BinaryExpression(
                SyntaxKind.CoalesceExpression,
                IdentifierName(storedMemberIdentifier),
                ThrowExpression(exception));
        }

        ArgumentSyntax argumentSyntax = Argument(argumentExpression).WithNameColon(NameColon(IdentifierName(ParameterIdentifier)));

        builder.AddModelCreationArguments(argumentSyntax);
    }
}
