using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceKit.Extensions;

public static class LocalDeclarationStatementSyntaxExtensions
{
    public static ExpressionSyntax? FindValue(this LocalDeclarationStatementSyntax syntax)
    {
        VariableDeclarationSyntax? declaration = syntax.Declaration;
        VariableDeclaratorSyntax? variable = declaration.Variables.SingleOrDefault();
        EqualsValueClauseSyntax? initializer = variable?.Initializer;

        return initializer?.Value;
    }
}