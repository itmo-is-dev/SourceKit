using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Extensions;

public static class UsingDirectiveSyntaxExtensions
{
    public static UsingDirectiveSyntax AddUsingKeywordLeadingTrivia(
        this UsingDirectiveSyntax syntax,
        SyntaxTrivia trivia)
    {
        var triviaList = TriviaList(syntax.UsingKeyword.LeadingTrivia.Append(trivia));
        var token = Token(triviaList, SyntaxKind.UsingKeyword, syntax.UsingKeyword.TrailingTrivia);

        return syntax.WithUsingKeyword(token);
    }
}