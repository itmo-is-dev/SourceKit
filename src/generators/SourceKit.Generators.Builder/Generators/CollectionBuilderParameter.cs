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

public sealed record CollectionBuilderParameter(
    SyntaxToken ParameterIdentifier,
    TypeSyntax ElementTypeSyntax,
    ITypeSymbol ElementType,
    BuilderPropertyOptions Options)
    : IBuilderParameter
{
    private readonly SyntaxToken _fieldIdentifier = Identifier(ParameterIdentifier.Text.ToUnderscoreCamelCase());

    public bool Equals(IBuilderParameter? other) => other is CollectionBuilderParameter property && Equals(property);

    public void Configure(BuilderSyntax builder, Compilation compilation)
    {
        ConfiguredStoredMember(builder, compilation);
        ConfigureConstructor(builder);
        ConfigureUnaryWithMethod(builder);
        ConfigureCollectionWithMethod(builder);
        ConfigureModelCreation(builder);
    }

    private void ConfiguredStoredMember(BuilderSyntax builder, Compilation compilation)
    {
        INamedTypeSymbol comparableType = compilation.GetTypeSymbol<IComparable>();
        INamedTypeSymbol genericComparableType = compilation.GetTypeSymbol(typeof(IComparable<>)).Construct(ElementType);

        GenericNameSyntax fieldCollectionTypeSyntax =
            ElementType.IsAssignableTo(comparableType) is false
            && ElementType.IsAssignableTo(genericComparableType) is false
            || Options.HasFlag(BuilderPropertyOptions.AllowsDuplicates)
                ? GenericName("global::System.Collections.Generic.List")
                : GenericName("global::System.Collections.Generic.HashSet");

        TypeSyntax fieldTypeSyntax = fieldCollectionTypeSyntax.AddTypeArgumentListArguments(ElementTypeSyntax);

        if (Options.HasFlag(BuilderPropertyOptions.ConstructorParameter) is false)
        {
            fieldTypeSyntax = NullableType(fieldTypeSyntax);
        }

        VariableDeclarationSyntax variableDeclaration = VariableDeclaration(fieldTypeSyntax)
            .AddVariables(VariableDeclarator(_fieldIdentifier));

        FieldDeclarationSyntax fieldSyntax = FieldDeclaration(variableDeclaration)
            .AddModifiers(Token(SyntaxKind.PrivateKeyword));

        builder.AddMembers(fieldSyntax);

        if (Options.HasFlag(BuilderPropertyOptions.Exposed))
        {
            GenericNameSyntax propertyTypeSyntax = GenericName("global::System.Collections.Generic.IEnumerable")
                .AddTypeArgumentListArguments(ElementTypeSyntax);

            PropertyDeclarationSyntax propertySyntax = PropertyDeclaration(propertyTypeSyntax, ParameterIdentifier)
                .AddModifiers(Token(SyntaxKind.PublicKeyword))
                .WithExpressionBody(ArrowExpressionClause(BinaryExpression(
                    SyntaxKind.CoalesceExpression,
                    IdentifierName(_fieldIdentifier),
                    CollectionExpression())))
                .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));

            builder.AddMembers(propertySyntax);
        }
    }

    private void ConfigureConstructor(BuilderSyntax builder)
    {
        if (Options.HasFlag(BuilderPropertyOptions.ConstructorParameter) is false)
            return;

        SyntaxToken parameterIdentifier = Identifier(ParameterIdentifier.Text.Camelize());

        GenericNameSyntax parameterType = GenericName("global::System.Collections.Generic.IEnumerable")
            .AddTypeArgumentListArguments(ElementTypeSyntax);

        ParameterSyntax parameterSyntax = Parameter(parameterIdentifier).WithType(parameterType);

        AssignmentExpressionSyntax assignmentExpression = AssignmentExpression(
            SyntaxKind.SimpleAssignmentExpression,
            IdentifierName(_fieldIdentifier),
            CollectionExpression().AddElements(SpreadElement(IdentifierName(parameterIdentifier))));

        builder.AddConstructorParameter(parameterSyntax);
        builder.AddConstructorStatements(ExpressionStatement(assignmentExpression));
    }

    private void ConfigureUnaryWithMethod(BuilderSyntax builder)
    {
        SyntaxToken parameterIdentifier = Identifier("element");
        ParameterSyntax parameter = Parameter(parameterIdentifier).WithType(ElementTypeSyntax);

        MemberAccessExpressionSyntax addMethod = MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            IdentifierName(_fieldIdentifier),
            IdentifierName("Add"));

        MethodDeclarationSyntax methodSyntax = MethodDeclaration(IdentifierName(BuilderSyntax.BuilderIdentifier), $"With{ParameterIdentifier.Text.Singularize()}")
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .AddParameterListParameters(parameter)
            .AddBodyStatements(CreateLazyInitStatement().ToArray())
            .AddBodyStatements(
                ExpressionStatement(InvocationExpression(addMethod)
                    .AddArgumentListArguments(Argument(IdentifierName(parameterIdentifier)))),
                ReturnStatement(ThisExpression()))
            .AddAttributeLists(new InitializesPropertyAttributeBuilder(ParameterIdentifier));

        builder.AddMembers(methodSyntax);
    }

    private void ConfigureCollectionWithMethod(BuilderSyntax builder)
    {
        SyntaxToken parameterIdentifier = Identifier("elements");

        ParameterSyntax parameter = Parameter(parameterIdentifier)
            .WithType(GenericName("global::System.Collections.Generic.IEnumerable")
                .AddTypeArgumentListArguments(ElementTypeSyntax));

        MemberAccessExpressionSyntax addMethod = MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            IdentifierName(_fieldIdentifier),
            IdentifierName("Add"));

        SyntaxToken loopParameterIdentifier = Identifier("element");

        ArgumentSyntax argument = Argument(IdentifierName(loopParameterIdentifier));
        InvocationExpressionSyntax invocation = InvocationExpression(addMethod).AddArgumentListArguments(argument);

        ForEachStatementSyntax loop = ForEachStatement(
            ElementTypeSyntax,
            loopParameterIdentifier,
            IdentifierName(parameterIdentifier),
            ExpressionStatement(invocation));

        ReturnStatementSyntax returnStatement = ReturnStatement(ThisExpression());

        MethodDeclarationSyntax methodSyntax = MethodDeclaration(IdentifierName(BuilderSyntax.BuilderIdentifier), $"With{ParameterIdentifier.Text.Pluralize()}")
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .AddParameterListParameters(parameter)
            .AddBodyStatements(CreateLazyInitStatement().ToArray())
            .AddBodyStatements(loop, returnStatement)
            .AddAttributeLists(new InitializesPropertyAttributeBuilder(ParameterIdentifier));

        builder.AddMembers(methodSyntax);
    }

    private void ConfigureModelCreation(BuilderSyntax builder)
    {
        ExpressionSyntax expression = IdentifierName(_fieldIdentifier);

        if (Options.HasFlag(BuilderPropertyOptions.ConstructorParameter) is false)
        {
            expression = BinaryExpression(SyntaxKind.CoalesceExpression, expression, CollectionExpression());
        }

        // TODO: diagnostic when types are incompatible (or line pragma to propagate errors to model type source)
        if (Options.HasFlag(BuilderPropertyOptions.NotCopied) is false)
        {
            expression = CollectionExpression().AddElements(SpreadElement(expression));
        }

        builder.AddModelCreationArguments(Argument(expression).WithNameColon(NameColon(IdentifierName(ParameterIdentifier))));
    }

    private IEnumerable<StatementSyntax> CreateLazyInitStatement()
    {
        if (Options.HasFlag(BuilderPropertyOptions.ConstructorParameter))
            yield break;

        yield return ExpressionStatement(
            AssignmentExpression(
                SyntaxKind.CoalesceAssignmentExpression,
                IdentifierName(_fieldIdentifier),
                CollectionExpression()));
    }
}
