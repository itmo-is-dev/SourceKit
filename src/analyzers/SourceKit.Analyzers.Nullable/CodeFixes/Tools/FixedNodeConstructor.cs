using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceKit.Analyzers.Nullable.CodeFixes.Tools;

public static class FixedNodeConstructor
{
    public static SyntaxNode FromNullForgivingExpression(SyntaxNode expression)
    {
        var nullForgivingExpression = (ExpressionSyntax) expression
            .DescendantNodesAndSelf()
            .First(node => node.IsKind(SyntaxKind.SuppressNullableWarningExpression) &&
                           !node.DescendantNodes().Any(innerNode =>
                               innerNode.IsKind(SyntaxKind.SuppressNullableWarningExpression)));
        if (nullForgivingExpression.Parent.IsKind(SyntaxKind.PointerMemberAccessExpression) ||
            nullForgivingExpression.Parent.IsKind(SyntaxKind.SimpleMemberAccessExpression))
        {
            return SyntaxFactory.ConditionalAccessExpression(
                (ExpressionSyntax) nullForgivingExpression.ChildNodes().First(),
                ParseNullForgivingSyntax((PostfixUnaryExpressionSyntax) nullForgivingExpression)
            );
        }

        return nullForgivingExpression.ChildNodes().First();
    }

    private static SimpleNameSyntax GetMemberNameFromExpression(MemberAccessExpressionSyntax expressionSyntax)
    {
        return expressionSyntax.Name;
    }

    private static ExpressionSyntax ParseNullForgivingSyntax(
        PostfixUnaryExpressionSyntax expressionSyntax)
    {
        var currentNode = expressionSyntax.Parent;

        var memberName = GetMemberNameFromExpression((MemberAccessExpressionSyntax) currentNode);

        ExpressionSyntax whenNotNullExpression =
            SyntaxFactory.MemberBindingExpression(memberName);

        while (currentNode.Parent != null &&
               (currentNode.Parent.IsKind(SyntaxKind.InvocationExpression) ||
                currentNode.Parent.IsKind(SyntaxKind.SimpleMemberAccessExpression) ||
                currentNode.Parent.IsKind(SyntaxKind.PointerMemberAccessExpression) ||
                currentNode.Parent.IsKind(SyntaxKind.SuppressNullableWarningExpression)))
        {
            if (currentNode.Parent.IsKind(SyntaxKind.InvocationExpression))
            {
                whenNotNullExpression =
                    SyntaxFactory.InvocationExpression(whenNotNullExpression)
                        .WithArgumentList(((InvocationExpressionSyntax) currentNode.Parent).ArgumentList);
            }
            else if (currentNode.Parent.IsKind(SyntaxKind.SuppressNullableWarningExpression))
            {
                if (currentNode.Parent.Parent.IsKind(SyntaxKind.PointerMemberAccessExpression) ||
                    currentNode.Parent.Parent.IsKind(SyntaxKind.SimpleMemberAccessExpression))
                {
                    return SyntaxFactory.ConditionalAccessExpression(
                        whenNotNullExpression,
                        ParseNullForgivingSyntax((PostfixUnaryExpressionSyntax) currentNode.Parent)
                    );
                }
                else
                {
                    return whenNotNullExpression;
                }

            }
            else
            {
                memberName = GetMemberNameFromExpression((MemberAccessExpressionSyntax) currentNode.Parent);
                whenNotNullExpression = SyntaxFactory
                    .MemberAccessExpression(
                        currentNode.Parent.Kind(),
                        whenNotNullExpression,
                        memberName);
            }

            currentNode = currentNode.Parent;
        }

        return whenNotNullExpression;
    }
}