using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceKit.Analyzers.Nullable.CodeFixes.Tools;

public class TypeChanger
{
    public static CodeFixResult TryToChangeTypeIfNecessary(SyntaxNode initialNode, SyntaxNode fixedNode)
    {
        if (!initialNode.Parent.IsKind(SyntaxKind.ArrowExpressionClause) &&
            !initialNode.Parent.IsKind(SyntaxKind.EqualsValueClause) &&
            !initialNode.Parent.IsKind(SyntaxKind.ReturnStatement))
        {
            return new CodeFixResult(initialNode, fixedNode);
        }

        var currentNode = initialNode.Parent;
        while (currentNode is not null && !currentNode.IsKind(SyntaxKind.VariableDeclaration) &&
               !currentNode.IsKind(SyntaxKind.PropertyDeclaration))
        {
            if (currentNode.IsKind(SyntaxKind.Argument))
            {
                return new CodeFixResult(initialNode, fixedNode);
            }

            currentNode = currentNode.Parent;
        }

        if (currentNode is null)
        {
            return new CodeFixResult(initialNode, fixedNode);
        }

        if (currentNode.IsKind(SyntaxKind.VariableDeclaration))
        {
            var variableSyntax = (VariableDeclarationSyntax) currentNode.ReplaceNode(initialNode, fixedNode);
            return variableSyntax.Type.IsVar
                ? new CodeFixResult(currentNode, variableSyntax)
                : new CodeFixResult(
                    currentNode,
                    variableSyntax.WithType(SyntaxFactory.NullableType(variableSyntax.Type))
                );
        }

        var propertySyntax = (PropertyDeclarationSyntax) currentNode.ReplaceNode(initialNode, fixedNode);
        return propertySyntax.Type.IsVar
            ? new CodeFixResult(currentNode, propertySyntax)
            : new CodeFixResult(
                currentNode,
                propertySyntax.WithType(SyntaxFactory.NullableType(propertySyntax.Type))
            );
    }
}