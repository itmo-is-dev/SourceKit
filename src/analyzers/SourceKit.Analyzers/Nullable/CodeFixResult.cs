using Microsoft.CodeAnalysis;

namespace SourceKit.Analyzers.Nullable;

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