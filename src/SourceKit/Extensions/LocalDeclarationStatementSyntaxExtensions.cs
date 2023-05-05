using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceKit.Extensions;

public static class LocalDeclarationStatementSyntaxExtensions
{
    public static ExpressionSyntax? FindValue(this LocalDeclarationStatementSyntax syntax)
    {
        var declaration = syntax.Declaration;
        var variable = declaration.Variables.SingleOrDefault();
        var initializer = variable?.Initializer;

        return initializer?.Value;
    }
}