using FluentChaining;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Generators.Builder.Commands;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Generators.Builder.Builders.FileBuilders;

public class UsingBuilder : ILink<FileBuildingCommand, CompilationUnitSyntax>
{
    private readonly IChain<UsingBuildingCommand, UsingDirectiveSyntax> _commentChain;

    public UsingBuilder(IChain<UsingBuildingCommand, UsingDirectiveSyntax> commentChain)
    {
        _commentChain = commentChain;
    }

    public CompilationUnitSyntax Process(
        FileBuildingCommand request,
        SynchronousContext context,
        LinkDelegate<FileBuildingCommand, SynchronousContext, CompilationUnitSyntax> next)
    {
        var directive = UsingDirective(IdentifierName("System"));
        var commentBuildingCommand = new UsingBuildingCommand(directive, request.Symbol);

        directive = _commentChain.Process(commentBuildingCommand);

        var syntax = request.CompilationUnit.AddUsings(directive);

        request = request with
        {
            CompilationUnit = syntax,
        };

        return next(request, context);
    }
}