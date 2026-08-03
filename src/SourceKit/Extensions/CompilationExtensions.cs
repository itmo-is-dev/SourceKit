using Microsoft.CodeAnalysis;

namespace SourceKit.Extensions;

public static class CompilationExtensions
{
    public static INamedTypeSymbol? FindTypeSymbol(this Compilation compilation, string typeName)
        => compilation.GetTypeByMetadataName(typeName);

    public static INamedTypeSymbol? FindTypeSymbol(this Compilation compilation, Type type)
        => compilation.FindTypeSymbol(type.FullName ?? string.Empty);

    public static INamedTypeSymbol? FindTypeSymbol<T>(this Compilation compilation)
        => compilation.FindTypeSymbol(typeof(T));

    public static INamedTypeSymbol GetTypeSymbol(this Compilation compilation, string typeName)
        => compilation.FindTypeSymbol(typeName) ?? throw new ArgumentException($"Type {typeName} is not part of compilation");

    public static INamedTypeSymbol GetTypeSymbol(this Compilation compilation, Type type)
        => compilation.GetTypeSymbol(type.FullName ?? string.Empty);

    public static INamedTypeSymbol GetTypeSymbol<T>(this Compilation compilation)
        => compilation.GetTypeSymbol(typeof(T));
}
