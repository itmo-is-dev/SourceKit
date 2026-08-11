using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceKit.Extensions;

public static class MemberDeclarationSyntaxExtensions
{
    public static bool HasModifiers(this MemberDeclarationSyntax syntax, params ReadOnlySpan<SyntaxKind> modifiers)
    {
        foreach (SyntaxKind expectedModifier in modifiers)
        {
            if (syntax.Modifiers.Any(modifier => modifier.IsKind(expectedModifier)) is false)
                return false;
        }

        return true;
    }

    public static bool HasAnyModifier(this MemberDeclarationSyntax syntax, params ReadOnlySpan<SyntaxKind> modifiers)
    {
        foreach (SyntaxKind expectedModifier in modifiers)
        {
            if (syntax.Modifiers.Any(modifier => modifier.IsKind(expectedModifier)))
                return true;
        }

        return false;
    }
}
