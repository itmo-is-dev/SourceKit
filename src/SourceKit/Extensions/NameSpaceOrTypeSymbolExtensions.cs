using System.Text;
using Microsoft.CodeAnalysis;

namespace SourceKit.Extensions;

public static class NameSpaceOrTypeSymbolExtensions
{
    public static string GetFullyQualifiedName(this INamespaceOrTypeSymbol symbol)
    {
        var builder = new StringBuilder(symbol.Name);

        if (symbol is INamedTypeSymbol { IsGenericType: true } namedTypeSymbol)
        {
            builder.Append('`');
            builder.Append(namedTypeSymbol.TypeParameters.Length);
        }

        symbol = symbol.ContainingType ?? (INamespaceOrTypeSymbol)symbol.ContainingNamespace;

        while (symbol is not null and not INamespaceSymbol { IsGlobalNamespace: true })
        {
            builder.Insert(0, '.');
            builder.Insert(0, symbol.Name);
            symbol = symbol.ContainingType ?? (INamespaceOrTypeSymbol)symbol.ContainingNamespace;
        }

        return builder.ToString();
    }
}