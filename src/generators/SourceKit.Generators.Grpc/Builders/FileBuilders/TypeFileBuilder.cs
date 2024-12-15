using FluentChaining;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;
using SourceKit.Generators.Grpc.Commands;
using SourceKit.Generators.Grpc.Models;
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
        string namespaceString = request.Message.Type.ContainingNamespace.IsGlobalNamespace
            ? request.Message.Type.ContainingNamespace.Name
            : request.Message.Type.ContainingNamespace.GetFullyQualifiedName();

        IdentifierNameSyntax namespaceIdentifier = IdentifierName(namespaceString);
        NamespaceDeclarationSyntax namespaceDeclaration = NamespaceDeclaration(namespaceIdentifier);

        TypeDeclarationSyntax declaration = GenerateMessageType(request.Message, request.Context);
        declaration = ProcessNestedTypes(declaration, request.Message, request.Context);

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

    private TypeDeclarationSyntax GenerateMessageType(ProtoMessage message, GeneratorExecutionContext context)
    {
        SyntaxTokenList modifiers = TokenList
        (
            message.Type.DeclaredAccessibility
                .ToSyntaxTokenList()
                .Append(Token(SyntaxKind.PartialKeyword))
        );

        TypeDeclarationSyntax nestedDeclaration = message.Type.ToSyntax().WithModifiers(modifiers);
        var command = new TypeBuildingCommand(message, nestedDeclaration, context);

        return _typeChain.Process(command);
    }

    private TypeDeclarationSyntax ProcessNestedTypes(
        TypeDeclarationSyntax declaration,
        ProtoMessage message,
        GeneratorExecutionContext context)
    {
        if (message.NestedMessages is [])
            return declaration;

        ClassDeclarationSyntax typesDeclaration = ClassDeclaration("Types")
            .AddModifiers(
                Token(SyntaxKind.PublicKeyword),
                Token(SyntaxKind.StaticKeyword),
                Token(SyntaxKind.PartialKeyword));

        foreach (ProtoMessage nestedMessage in message.NestedMessages)
        {
            TypeDeclarationSyntax nestedDeclaration = GenerateMessageType(nestedMessage, context);
            nestedDeclaration = ProcessNestedTypes(nestedDeclaration, nestedMessage, context);

            typesDeclaration = typesDeclaration.AddMembers(nestedDeclaration);
        }

        return declaration.AddMembers(typesDeclaration);
    }
}