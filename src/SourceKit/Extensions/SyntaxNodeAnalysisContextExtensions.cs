using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SourceKit.Extensions;

public static class SyntaxNodeAnalysisContextExtensions
{
    public static ISymbol GetSymbolFromContext(this SyntaxNodeAnalysisContext context, SyntaxNode node)
    {
        var model = context.SemanticModel;
        
        var symbolInfo = model.GetSymbolInfo(node);

        var symbol = symbolInfo.Symbol;

        if (symbol is null)
        {
            throw new ArgumentException("Required symbol is not found");
        }
        
        return symbol;
    }
}