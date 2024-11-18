using FluentChaining;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;
using SourceKit.Generators.Builder.Commands;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Generators.Builder.Builders.FileBuilders;

public class TypeBuilder : ILink<FileBuildingCommand, CompilationUnitSyntax>
{
    private readonly IChain<TypeBuildingCommand, TypeDeclarationSyntax> _chain;

    public TypeBuilder(IChain<TypeBuildingCommand, TypeDeclarationSyntax> chain)
    {
        _chain = chain;
    }

    public CompilationUnitSyntax Process(
        FileBuildingCommand request,
        SynchronousContext context,
        LinkDelegate<FileBuildingCommand, SynchronousContext, CompilationUnitSyntax> next)
    {
        var modifiers = TokenList
        (
            request.Symbol
                .DeclaredAccessibility
                .ToSyntaxTokenList()
                .Append(Token(SyntaxKind.PartialKeyword))
        );

        var namespaceString = request.Symbol.ContainingNamespace.IsGlobalNamespace
            ? request.Symbol.ContainingNamespace.Name
            : request.Symbol.ContainingNamespace.GetFullyQualifiedName();

        var namespaceIdentifier = IdentifierName(namespaceString);
        var namespaceDeclaration = NamespaceDeclaration(namespaceIdentifier);
        var declaration = request.Symbol.ToSyntax().WithModifiers(modifiers);

        var command = new TypeBuildingCommand(
            request.Context,
            declaration,
            request.Symbol,
            request.Properties);

        declaration = _chain.Process(command);
        namespaceDeclaration = namespaceDeclaration.AddMembers(declaration);

        namespaceDeclaration = namespaceDeclaration.WithNamespaceKeyword(Token(
            TriviaList(Trivia(NullableDirectiveTrivia(Token(SyntaxKind.EnableKeyword), true))),
            SyntaxKind.NamespaceKeyword,
            SyntaxTriviaList.Empty));

        request = request with
        {
            CompilationUnit = request.CompilationUnit.AddMembers(namespaceDeclaration),
        };

        return next(request, context);
    }
}