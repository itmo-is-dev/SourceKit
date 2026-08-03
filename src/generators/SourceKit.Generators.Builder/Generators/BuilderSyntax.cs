using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Generators.Builder.Generators;

public sealed class BuilderSyntax(SyntaxToken modelIdentifier)
{
    public static readonly SyntaxToken BuilderIdentifier = Identifier("Builder");

    private readonly List<MemberDeclarationSyntax> _builderMembers = [];
    private readonly List<ParameterSyntax> _builderConstructorParameters = [];
    private readonly List<StatementSyntax> _builderConstructorStatements = [];
    private readonly List<ArgumentSyntax> _modelCreationArguments = [];

    public void AddMembers(params MemberDeclarationSyntax[] members)
    {
        _builderMembers.AddRange(members);
    }

    public void AddConstructorParameter(ParameterSyntax parameter)
    {
        _builderConstructorParameters.Add(parameter);
    }

    public void AddConstructorStatements(params StatementSyntax[] statements)
    {
        _builderConstructorStatements.AddRange(statements);
    }

    public void AddModelCreationArguments(params ArgumentSyntax[] arguments)
    {
        _modelCreationArguments.AddRange(arguments);
    }

    public TypeDeclarationSyntax AddBuilder(TypeDeclarationSyntax modelSyntax)
    {
        IEnumerable<MemberDeclarationSyntax> members = _builderMembers
            .Append(CreateBuilderConstructor())
            .Append(CreateInstanceBuildMethod())
            .OrderBy(member => member switch
            {
                FieldDeclarationSyntax => 0,
                ConstructorDeclarationSyntax => 1,
                PropertyDeclarationSyntax => 2,
                MethodDeclarationSyntax => 3,
                _ => int.MaxValue,
            });

        ClassDeclarationSyntax builderSyntax = ClassDeclaration(BuilderIdentifier)
            .AddModifiers(
                Token(SyntaxKind.PublicKeyword),
                Token(SyntaxKind.SealedKeyword),
                Token(SyntaxKind.PartialKeyword))
            .WithMembers(List(members));

        return modelSyntax.AddMembers(CreateStaticBuildMethod(), builderSyntax);
    }

    private MemberDeclarationSyntax CreateStaticBuildMethod()
    {
        GenericNameSyntax actionParameterType = GenericName(Identifier("Func"))
            .AddTypeArgumentListArguments(IdentifierName(BuilderIdentifier), IdentifierName(BuilderIdentifier));

        ParameterSyntax actionParameter = Parameter(Identifier("action")).WithType(actionParameterType);

        ObjectCreationExpressionSyntax builderCreation = ObjectCreationExpression(IdentifierName(BuilderIdentifier))
            .AddArgumentListArguments(_builderConstructorParameters
                .Select(parameter => Argument(IdentifierName(parameter.Identifier)))
                .ToArray());

        InvocationExpressionSyntax actionInvocation = InvocationExpression(IdentifierName(actionParameter.Identifier))
            .AddArgumentListArguments(Argument(builderCreation));

        MemberAccessExpressionSyntax buildMemberAccess = MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            actionInvocation,
            IdentifierName("Build"));

        AttributeSyntax methodImplAttribute = Attribute(IdentifierName("global::System.Runtime.CompilerServices.MethodImpl"))
            .AddArgumentListArguments(AttributeArgument(IdentifierName("global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining")));

        return MethodDeclaration(IdentifierName(modelIdentifier), Identifier("Build"))
            .AddAttributeLists(AttributeList().AddAttributes(methodImplAttribute))
            .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword))
            .AddParameterListParameters(_builderConstructorParameters.ToArray())
            .AddParameterListParameters(actionParameter)
            .AddBodyStatements(ReturnStatement(InvocationExpression(buildMemberAccess)));
    }

    private MemberDeclarationSyntax CreateBuilderConstructor()
    {
        return ConstructorDeclaration(BuilderIdentifier)
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .AddParameterListParameters(_builderConstructorParameters.ToArray())
            .AddBodyStatements(_builderConstructorStatements.ToArray());
    }

    private MemberDeclarationSyntax CreateInstanceBuildMethod()
    {
        ObjectCreationExpressionSyntax modelCreation = ObjectCreationExpression(IdentifierName(modelIdentifier))
            .AddArgumentListArguments(_modelCreationArguments.ToArray());

        return MethodDeclaration(IdentifierName(modelIdentifier), Identifier("Build"))
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .AddBodyStatements(ReturnStatement(modelCreation));
    }
}
