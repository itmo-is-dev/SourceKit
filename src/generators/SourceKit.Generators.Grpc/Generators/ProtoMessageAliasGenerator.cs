using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;
using SourceKit.Generators.Grpc.Extensions;
using SourceKit.Tools;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Generators.Grpc.Generators;

[Generator]
public sealed class ProtoMessageAliasGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValueProvider<IncrementalResult<INamedTypeSymbol>> messageInterfaceSymbol = context.CompilationProvider
            .Select(static (compilation, _) => compilation.GetTypeByMetadataName(Constants.ProtobufMessageInterfaceFullyQualifiedName) is { } symbol
                ? IncrementalResult.Success(symbol)
                : IncrementalResult.Skip);

        IncrementalValueProvider<IncrementalResult<INamedTypeSymbol>> enumAttributeSymbol = context.CompilationProvider
            .Select(static (compilation, _) => compilation.GetTypeByMetadataName(Constants.ProtobufOriginalNameAttributeFullyQualifiedName) is { } symbol
                ? IncrementalResult.Success(symbol)
                : IncrementalResult.Skip);

        IncrementalValuesProvider<INamedTypeSymbol> definedTypes = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => node switch
                {
                    TypeDeclarationSyntax typeDeclaration => typeDeclaration.BaseList is not null,
                    EnumDeclarationSyntax => true,

                    _ => false,
                },
                static (context, _) =>
                {
                    return context.SemanticModel.GetDeclaredSymbol(context.Node) is INamedTypeSymbol symbol
                        ? IncrementalResult.Success(symbol)
                        : IncrementalResult.Skip;
                })
            .Unwrap(context)
            .Where(IsApplicableType);

        IncrementalValuesProvider<INamedTypeSymbol> referencedTypes = context.CompilationProvider
            .SelectMany(static compilation => compilation.References.Select(reference => (reference, compilation)).ToImmutableArray())
            .WithComparer(static (reference, _) => reference)
            .Select(static (reference, compilation) => compilation.GetAssemblyOrModuleSymbol(reference) is IAssemblySymbol assemblySymbol
                ? IncrementalResult.Success(assemblySymbol)
                : IncrementalResult.Skip)
            .Unwrap(context)
            .WithComparer(static assembly => assembly.Identity)
            .SelectMany(static (assembly, ct) => assembly.GlobalNamespace.EnumerateAllAvailableTypes(ct));

        IncrementalValuesProvider<INamedTypeSymbol> FilterProtoMessages(IncrementalValuesProvider<INamedTypeSymbol> provider)
        {
            return provider
                .CombineAndUnwrap(messageInterfaceSymbol, context)
                .Where(static (symbol, _) => symbol.TypeKind is TypeKind.Class)
                .Where(static (symbol, messageInterfaceSymbol) => symbol.AllInterfaces.Contains(messageInterfaceSymbol, SymbolEqualityComparer.Default))
                .Where(static (symbol, _) => symbol.ContainingType is null)
                .Select(static (symbol, _) => symbol)
                .WithComparer(static symbol => (symbol.Name, symbol.ContainingNamespace.ToDisplayString()));
        }

        IncrementalValuesProvider<INamedTypeSymbol> FilterProtoEnums(IncrementalValuesProvider<INamedTypeSymbol> provider)
        {
            return provider
                .CombineAndUnwrap(enumAttributeSymbol, context)
                .Where(static (symbol, _) => symbol.TypeKind is TypeKind.Enum)
                .Where(static (symbol, enumAttributeSymbol) => symbol
                    .GetMembers()
                    .OfType<IFieldSymbol>()
                    .All(member => member
                        .GetAttributes()
                        .Any(attr => attr.AttributeClass?.Equals(enumAttributeSymbol, SymbolEqualityComparer.Default) is true)))
                .Where(static (symbol, _) => symbol.ContainingType is null)
                .Select(static (symbol, _) => symbol)
                .WithComparer(static symbol => (symbol.Name, symbol.ContainingNamespace.ToDisplayString()));
        }

        IncrementalValueProvider<ImmutableArray<INamedTypeSymbol>> definedProtoMessageTypes = FilterProtoMessages(definedTypes).Collect();
        IncrementalValueProvider<ImmutableArray<INamedTypeSymbol>> definedProtoEnumTypes = FilterProtoEnums(definedTypes).Collect();

        IncrementalValueProvider<ImmutableArray<INamedTypeSymbol>> referencedProtoMessageTypes = FilterProtoMessages(referencedTypes).Collect();
        IncrementalValueProvider<ImmutableArray<INamedTypeSymbol>> referencedProtoEnumTypes = FilterProtoEnums(referencedTypes).Collect();

        IncrementalValueProvider<ImmutableArray<INamedTypeSymbol>> protoTypes = definedProtoMessageTypes
            .Combine(definedProtoEnumTypes)
            .Combine(referencedProtoMessageTypes)
            .Combine(referencedProtoEnumTypes)
            .Select(static (array1, array2, array3, array4) => array1.Concat(array2).Concat(array3).Concat(array4).ToImmutableArray());

        context.RegisterSourceOutput(
            protoTypes,
            static (context, protoTypes) =>
            {
                if (protoTypes is [])
                    return;

                UsingDirectiveSyntax[] directives = protoTypes
                    .Distinct(SymbolEqualityComparer.Default.Cast<ISymbol, INamedTypeSymbol>())
                    .GroupBy(x => x.Name, (k, values) => (k, values: values.ToArray()))
                    .Where(x => x.values.Length is 1)
                    .Select(x => x.values.Single())
                    .OrderBy(x => x.Name)
                    .Select(GenerateAlias)
                    .ToArray();

                if (directives is [])
                    return;

                CompilationUnitSyntax unit = CompilationUnit().AddUsings(directives).NormalizeWhitespace(eol: "\n");
                string text = unit.ToFullString();

                text = $"""
                // <auto-generated>
                //      This code was generated by a SourceKit.Generators.Grpc code generator.
                //      https://github.com/itmo-is-dev/SourceKit
                // </auto-generated>
                
                {text}
                """;

                context.AddSource("SourceKit.Generators.Grpc.ProtoAlias.cs", text);
            });
    }

    private static UsingDirectiveSyntax GenerateAlias(INamedTypeSymbol symbol)
    {
        return UsingDirective(IdentifierName(symbol.GetFullyQualifiedName()))
            .WithGlobalKeyword(Token(SyntaxKind.GlobalKeyword))
            .WithAlias(NameEquals(IdentifierName($"Proto{symbol.Name}")));
    }

    private static bool IsApplicableType(INamedTypeSymbol type)
    {
        if (type.DeclaredAccessibility is not Accessibility.Public)
            return false;

        if (type.ContainingNamespace.ToDisplayString().StartsWith("Google"))
            return false;

        return true;
    }
}
