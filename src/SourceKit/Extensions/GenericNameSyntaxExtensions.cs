using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceKit.Extensions;

public static class GenericNameSyntaxExtensions
{
    public static SyntaxNode GetFirstGenericArgument(this GenericNameSyntax node)
    {
        return node.TypeArgumentList.ChildNodes().First();
    }
}