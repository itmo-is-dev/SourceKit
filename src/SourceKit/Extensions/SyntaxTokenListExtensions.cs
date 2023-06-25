using Microsoft.CodeAnalysis;

namespace SourceKit.Extensions;

public static class SyntaxTokenListExtensions
{
    public static Accessibility ToSyntaxTokenList(this SyntaxTokenList syntaxTokenList)
    {
        switch (syntaxTokenList.ToString())
        {
            case "private":
                return Accessibility.Private;
            case "protected internal":
                return Accessibility.ProtectedAndInternal;
            case "protected":
                return Accessibility.Protected;
            case "internal":
                return Accessibility.Internal;
            case "public":
                return Accessibility.Public;
            default:
                return Accessibility.NotApplicable;
        }
    }
}