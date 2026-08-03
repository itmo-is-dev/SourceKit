using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Generators.Builder.Models;

public readonly record struct LiteralValue(object? Value)
{
    public bool TryGetLiteralExpression([NotNullWhen(true)] out ExpressionSyntax? token)
    {
        token = Value switch
        {
            char c => LiteralExpression(SyntaxKind.CharacterLiteralExpression, Literal(c)),
            decimal d => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(d)),
            double d => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(d)),
            float f => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(f)),
            int i => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(i)),
            long l => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(l)),
            string s => LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(s)),

            _ => null,
        };

        return token is not null;
    }
}
