using FluentChaining;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;
using SourceKit.Generators.Grpc.Commands;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Generators.Grpc.Builders.UsingBuilders;

public class PragmaBuilder : ILink<UsingBuildingCommand, UsingDirectiveSyntax>
{
    public UsingDirectiveSyntax Process(
        UsingBuildingCommand request,
        SynchronousContext context,
        LinkDelegate<UsingBuildingCommand, SynchronousContext, UsingDirectiveSyntax> next)
    {
        var xmlDocPragma = PragmaWarningDirectiveTrivia(
                Token(SyntaxKind.DisableKeyword),
                true)
            .AddErrorCodes(IdentifierName("CS1591"));

        request = request with
        {
            Syntax = request.Syntax.AddUsingKeywordLeadingTrivia(Trivia(xmlDocPragma)),
        };

        return next(request, context);
    }
}