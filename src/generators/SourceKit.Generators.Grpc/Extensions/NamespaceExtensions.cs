using Microsoft.CodeAnalysis;

namespace SourceKit.Generators.Grpc.Extensions;

public static class NamespaceExtensions
{
    public static IEnumerable<INamedTypeSymbol> EnumerateAllAvailableTypes(
        this INamespaceSymbol ns,
        CancellationToken cancellationToken)
    {
        return EnumerateNestedTypesAndSelf(ns, cancellationToken);
    }

    private static IEnumerable<INamedTypeSymbol> EnumerateNestedTypesAndSelf(
        INamespaceOrTypeSymbol symbol,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (symbol is INamedTypeSymbol namedTypeSymbol)
        {
            return namedTypeSymbol
                .GetTypeMembers()
                .SelectMany(type => EnumerateNestedTypesAndSelf(type, cancellationToken))
                .Prepend(namedTypeSymbol);
        }

        if (symbol is INamespaceSymbol namespaceSymbol)
        {
            IEnumerable<INamedTypeSymbol> directTypes = namespaceSymbol
                .GetTypeMembers()
                .SelectMany(type => EnumerateNestedTypesAndSelf(type, cancellationToken));

            IEnumerable<INamedTypeSymbol> nestedNamespaceTypes = namespaceSymbol
                .GetNamespaceMembers()
                .SelectMany(ns => EnumerateNestedTypesAndSelf(ns, cancellationToken));

            return directTypes.Concat(nestedNamespaceTypes);
        }

        return [];
    }
}
