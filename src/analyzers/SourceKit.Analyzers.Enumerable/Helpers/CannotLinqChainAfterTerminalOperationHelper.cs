using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceKit.Analyzers.Enumerable.Helpers;

internal static class CannotLinqChainAfterTerminalOperationHelper
{
    internal static HashSet<string> TerminationMethods = new HashSet<string>()
    {
        nameof(System.Linq.Enumerable.ToArray),
        nameof(System.Linq.Enumerable.ToList),
        nameof(System.Linq.Enumerable.ToDictionary),
        nameof(System.Linq.Enumerable.ToLookup),
    };
    internal static bool IsLinqEnumerable(MemberAccessExpressionSyntax syntax, SemanticModel? model)
    {
        try
        {
            var symbol = GetSymbol(syntax, model) ?? throw new InvalidOperationException();
            return IsLinqEnumerable(symbol, model);
        } catch (InvalidOperationException e)
        {
            return false;
        }
    }
    
    private static bool IsLinqEnumerable(IMethodSymbol? symbol, SemanticModel? model)
    {
        var comparer = SymbolEqualityComparer.Default;
        return comparer.Equals(symbol?.ContainingType, model?.Compilation.GetTypeByMetadataName(typeof(System.Linq.Enumerable).FullName));
    }

    private static IMethodSymbol? GetSymbol(MemberAccessExpressionSyntax syntax, SemanticModel? model) 
        => model?.GetSymbolInfo(syntax).Symbol as IMethodSymbol;
    
}