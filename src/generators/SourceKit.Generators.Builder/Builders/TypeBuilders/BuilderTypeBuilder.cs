using FluentChaining;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;
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
        var modifiers = TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.SealedKeyword));
        TypeDeclarationSyntax builderDeclaration = ClassDeclaration("Builder").WithModifiers(modifiers);

        var properties = request.Symbol
            .GetMembers()
            .OfType<IPropertySymbol>()
            .Where(x => x.IsImplicitlyDeclared is false)
            .Where(x => x.IsAutoProperty())
            .ToArray();

        var command = new BuilderTypeBuildingCommand(
            request.Symbol,
            builderDeclaration,
            properties,
            request.Context);

        builderDeclaration = _chain.Process(command);

        request = request with
        {
            Syntax = request.Syntax.AddMembers(builderDeclaration),
        };

        return next(request, context);
    }
}