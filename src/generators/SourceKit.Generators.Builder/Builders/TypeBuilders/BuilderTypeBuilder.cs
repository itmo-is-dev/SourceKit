using FluentChaining;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Generators.Builder.Commands;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Generators.Builder.Builders.TypeBuilders;

public class BuilderTypeBuilder : ILink<TypeBuildingCommand, TypeDeclarationSyntax>
{
    private readonly IChain<BuilderTypeBuildingCommand, TypeDeclarationSyntax> _chain;

    public BuilderTypeBuilder(IChain<BuilderTypeBuildingCommand, TypeDeclarationSyntax> chain)
    {
        _chain = chain;
    }

    public TypeDeclarationSyntax Process(
        TypeBuildingCommand request,
        SynchronousContext context,
        LinkDelegate<TypeBuildingCommand, SynchronousContext, TypeDeclarationSyntax> next)
    {
        SyntaxTokenList modifiers = TokenList(
            Token(SyntaxKind.PublicKeyword),
            Token(SyntaxKind.SealedKeyword),
            Token(SyntaxKind.PartialKeyword));

        TypeDeclarationSyntax builderDeclaration = ClassDeclaration("Builder").WithModifiers(modifiers);

        var command = new BuilderTypeBuildingCommand(
            request.Symbol,
            builderDeclaration,
            request.Properties,
            request.Context);

        builderDeclaration = _chain.Process(command);

        request = request with
        {
            Syntax = request.Syntax.AddMembers(builderDeclaration),
        };

        return next(request, context);
    }
}