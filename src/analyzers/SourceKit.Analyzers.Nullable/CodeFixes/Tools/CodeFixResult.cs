using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace SourceKit.Analyzers.Nullable.CodeFixes.Tools;

public class CodeFixResult
{
    public CodeFixResult(SyntaxNode initialNode, SyntaxNode fixedNode)
    {
        Initial = initialNode;
        Fixed = fixedNode;
    }

    public SyntaxNode Initial { get; }
    public SyntaxNode Fixed { get; }
}