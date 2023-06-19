using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceKit.Analyzers.Enumerable.Helpers;

internal static class CannotLinqChainAfterTerminalOperationHelper
{
    internal static bool IsTerminationMethod(MemberAccessExpressionSyntax syntax, SemanticModel? model)
    {
        IMethodSymbol? symbol = GetSymbol(syntax, model);
        return IsLinqEnumerable(symbol, model) && !ReturnsIEnumerable(symbol, model);
    }
    
    internal static bool IsLinqEnumerable(MemberAccessExpressionSyntax syntax, SemanticModel? model)
    {
        var symbol = GetSymbol(syntax, model) ?? throw new InvalidOperationException();
        return IsLinqEnumerable(symbol, model);
    }
    
    private static bool IsLinqEnumerable(IMethodSymbol? symbol, SemanticModel? model)
    {
        var comparer = SymbolEqualityComparer.Default;
        return comparer.Equals(symbol?.ContainingType, model?.Compilation.GetTypeByMetadataName(typeof(System.Linq.Enumerable).FullName));
    }

    private static bool ReturnsIEnumerable(IMethodSymbol? symbol, SemanticModel? model)
    {
        INamedTypeSymbol? ienumerableType =
            model?.Compilation.GetTypeByMetadataName("System.Collections.Generic.IEnumerable`1");
        var comparer = SymbolEqualityComparer.Default;
        return comparer.Equals(symbol?.ReturnType.OriginalDefinition, ienumerableType);
    }

    private static IMethodSymbol? GetSymbol(MemberAccessExpressionSyntax syntax, SemanticModel? model) 
        => model?.GetSymbolInfo(syntax).Symbol as IMethodSymbol;
    
}