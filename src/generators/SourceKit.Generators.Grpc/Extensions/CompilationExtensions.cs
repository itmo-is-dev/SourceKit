using Microsoft.CodeAnalysis;

namespace SourceKit.Generators.Grpc.Extensions;

public static class CompilationExtensions
{
    /// <summary>
    ///     Enumerates all types reachable from this compilation including the types,
    ///     that were defined outside the compiled assembly.
    /// </summary>
    public static IEnumerable<INamedTypeSymbol> EnumerateAllAvailableTypes(
        this Compilation compilation,
        CancellationToken cancellationToken)
    {
        return EnumerateNestedTypesAndSelf(compilation.GlobalNamespace, cancellationToken);
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
