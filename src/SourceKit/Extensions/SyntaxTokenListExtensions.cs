using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace SourceKit.Extensions;

public static class SyntaxTokenListExtensions
{
    public static Accessibility ToAccessibility(this SyntaxTokenList syntaxTokenList)
    {
        return syntaxTokenList.Select(t => t.Kind()).ToList() switch
        {
            [SyntaxKind.PrivateKeyword] => Accessibility.Private,
            [SyntaxKind.ProtectedKeyword, SyntaxKind.InternalKeyword] => Accessibility.ProtectedAndInternal,
            [SyntaxKind.ProtectedKeyword] => Accessibility.Protected,
            [SyntaxKind.InternalKeyword] => Accessibility.Internal,
            [SyntaxKind.PublicKeyword] => Accessibility.Public,
            _ => Accessibility.NotApplicable
        };
    }
}