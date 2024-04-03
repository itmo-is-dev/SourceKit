using FluentChaining;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Generators.Builder.Commands;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Generators.Builder.Builders.UsingBuilders;

public class AutoGeneratedCommentBuilder : ILink<UsingBuildingCommand, UsingDirectiveSyntax>
{
    public UsingDirectiveSyntax Process(
        UsingBuildingCommand request,
        SynchronousContext context,
        LinkDelegate<UsingBuildingCommand, SynchronousContext, UsingDirectiveSyntax> next)
    {
        const string comment = """
        // <auto-generated>
        // This code was generated by a SourceKit.Generators.Builder code generator.
        // https://github.com/itmo-is-dev/SourceKit
        // </auto-generated>
        """;

        var autoGeneratedComment = TriviaList(Comment(comment));
        var autoGeneratedCommentToken = Token(autoGeneratedComment, SyntaxKind.UsingKeyword, TriviaList());

        request = request with
        {
            Syntax = request.Syntax.WithUsingKeyword(autoGeneratedCommentToken),
        };

        return next(request, context);
    }
}