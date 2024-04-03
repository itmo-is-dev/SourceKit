using FluentChaining;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;
using SourceKit.Generators.Grpc.Commands;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Generators.Grpc.Builders.FileBuilders;

public class TypeFileBuilder : ILink<FileBuildingCommand, CompilationUnitSyntax>
{
    private readonly IChain<TypeBuildingCommand, TypeDeclarationSyntax> _typeChain;

    public TypeFileBuilder(IChain<TypeBuildingCommand, TypeDeclarationSyntax> typeChain)
    {
        _typeChain = typeChain;
    }

    public CompilationUnitSyntax Process(
        FileBuildingCommand request,
        SynchronousContext context,
        LinkDelegate<FileBuildingCommand, SynchronousContext, CompilationUnitSyntax> next)
    {
        var modifiers = TokenList
        (
            request.Message.Type.DeclaredAccessibility
                .ToSyntaxTokenList()
                .Append(Token(SyntaxKind.PartialKeyword))
        );

        var namespaceString = request.Message.Type.ContainingNamespace.IsGlobalNamespace
            ? request.Message.Type.ContainingNamespace.Name
            : request.Message.Type.ContainingNamespace.GetFullyQualifiedName();

        var namespaceIdentifier = IdentifierName(namespaceString);
        var namespaceDeclaration = NamespaceDeclaration(namespaceIdentifier);
        var declaration = request.Message.Type.ToSyntax().WithModifiers(modifiers);

        var command = new TypeBuildingCommand(request.Message, declaration, request.Context);

        declaration = _typeChain.Process(command);

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