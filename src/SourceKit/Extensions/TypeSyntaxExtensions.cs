using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceKit.Extensions;

public static class TypeSyntaxExtensions
{
    public static bool IsSimpleNameEquals(this TypeSyntax syntax, string name)
    {
        if (syntax is SimpleNameSyntax simpleName)
        {
            return simpleName.Identifier.Text.Equals(name, StringComparison.OrdinalIgnoreCase);
        }

        if (syntax is QualifiedNameSyntax qualifiedName)
        {
            return qualifiedName.Right.IsSimpleNameEquals(name);
        }

        if (syntax is AliasQualifiedNameSyntax aliasQualifiedName)
        {
            return aliasQualifiedName.Name.IsSimpleNameEquals(name);
        }

        return false;
    }
}
