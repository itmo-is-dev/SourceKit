using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using SourceKit.Extensions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Generators.Grpc.Generators;

[Generator]
public sealed class ProtoMessageAliasGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValueProvider<IncrementalResult<INamedTypeSymbol>> messageInterfaceSymbol = context.CompilationProvider
            .Select(static compilation => compilation.GetTypeByMetadataName(Constants.ProtobufMessageInterfaceFullyQualifiedName) is { } symbol
                ? IncrementalResult.Success(symbol)
                : IncrementalResult.Skip);

        IncrementalValueProvider<IncrementalResult<INamedTypeSymbol>> enumAttributeSymbol = context.CompilationProvider
            .Select(static compilation => compilation.GetTypeByMetadataName(Constants.ProtobufOriginalNameAttributeFullyQualifiedName) is { } symbol
                ? IncrementalResult.Success(symbol)
                : IncrementalResult.Skip);

        IncrementalValueProvider<IncrementalResult<INamedTypeSymbol>> exportedAliasAttributeSymbol = context.CompilationProvider
            .Select(static compilation => compilation.GetTypeByMetadataName(Constants.ExportProtoAliasAttributeMetadataName) is { } symbol
                ? IncrementalResult.Success(symbol)
                : IncrementalResult.Skip);

        IncrementalValuesProvider<INamedTypeSymbol> definedTypes = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => node switch
                {
                    TypeDeclarationSyntax typeDeclaration =>
                        typeDeclaration.BaseList is { } baseList
                        && baseList.Types.Any(type => type.Type.IsSimpleNameEquals("IMessage")),

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

        IncrementalValueProvider<ImmutableArray<MessageAliasDefinition>> definedProtoMessageTypes = definedTypes
            .CombineAndUnwrap(messageInterfaceSymbol, context)
            .Where(static (symbol, _) => symbol.TypeKind is TypeKind.Class)
            .Where(static (symbol, messageInterfaceSymbol) => symbol.AllInterfaces.Contains(messageInterfaceSymbol, SymbolEqualityComparer.Default))
            .Where(static (symbol, _) => symbol.ContainingType is null)
            .Select(static (symbol, _) => symbol)
            .WithComparer(static symbol => (symbol.Name, symbol.ContainingNamespace.ToDisplayString()))
            .Select(MessageAliasDefinition.Create)
            .Collect();

        IncrementalValueProvider<ImmutableArray<MessageAliasDefinition>> definedProtoEnumTypes = definedTypes
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
            .WithComparer(static symbol => (symbol.Name, symbol.ContainingNamespace.ToDisplayString()))
            .Select(MessageAliasDefinition.Create)
            .Collect();

        IncrementalValueProvider<ImmutableArray<MessageAliasDefinition>> exportedAliasDefinitions = context.CompilationProvider
            .SelectMany(static compilation => compilation
                .References
                .Select(compilation.GetAssemblyOrModuleSymbol)
                .OfType<IAssemblySymbol>()
                .ToImmutableArray())
            .WithComparer(static assembly => assembly.Identity)
            .CombineAndUnwrap(exportedAliasAttributeSymbol, context)
            .WithComparer(static (assembly, _) => assembly.Identity)
            .SelectMany(static (assembly, exportAttributeSymbol) => assembly
                .GetAttributes()
                .Where(attribute => SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, exportAttributeSymbol))
                .Where(attribute => attribute.ConstructorArguments is [{ Value: string }, { Value: string }])
                .Select(attribute => new MessageAliasDefinition(
                    Alias: (string)attribute.ConstructorArguments[0].Value!,
                    TypeName: (string)attribute.ConstructorArguments[1].Value!))
                .ToImmutableArray())
            .Collect();

        context.RegisterSourceOutput(definedProtoMessageTypes, CreateExportAliasAttributeWriter("DefinedMessages"));
        context.RegisterSourceOutput(definedProtoEnumTypes, CreateExportAliasAttributeWriter("DefinedEnums"));

        context.RegisterSourceOutput(definedProtoMessageTypes, CreateAliasWriter("DefinedMessages"));
        context.RegisterSourceOutput(definedProtoEnumTypes, CreateAliasWriter("DefinedEnums"));
        context.RegisterSourceOutput(exportedAliasDefinitions, CreateAliasWriter("Exported"));
    }

    static Action<SourceProductionContext, ImmutableArray<MessageAliasDefinition>> CreateExportAliasAttributeWriter(string filePostfix)
    {
        return (context, types) =>
        {
            if (types is [])
                return;

            AttributeListSyntax[] attributes = types
                .Distinct()
                .GroupBy(alias => alias.Alias, (_, values) => values.ToArray())
                .Where(definitions => definitions.Length is 1)
                .Select(definitions => definitions.Single())
                .OrderBy(alias => alias.Alias)
                .Select(alias => AttributeList()
                    .AddAttributes(Attribute(IdentifierName("global::SourceKit.Generators.Grpc.Annotations.ExportProtoAlias"))
                        .AddArgumentListArguments(
                            AttributeArgument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(alias.Alias))),
                            AttributeArgument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(alias.TypeName)))))
                    .WithTarget(AttributeTargetSpecifier(Token(SyntaxKind.AssemblyKeyword))))
                .ToArray();

            if (attributes is [])
                return;

            CompilationUnitSyntax unit = CompilationUnit().AddAttributeLists(attributes).NormalizeWhitespace(eol: "\n");
            string text = unit.ToFullString();

            text = $"""
            // <auto-generated>
            //      This code was generated by a SourceKit.Generators.Grpc code generator.
            //      https://github.com/itmo-is-dev/SourceKit
            // </auto-generated>
            
            {text}
            """;

            context.AddSource($"SourceKit.Generators.Grpc.ExportProtoAlias.{filePostfix}.g.cs", SourceText.From(text, Encoding.UTF8));
        };
    }

    static Action<SourceProductionContext, ImmutableArray<MessageAliasDefinition>> CreateAliasWriter(string filePostfix)
    {
        return (context, types) =>
        {
            if (types is [])
                return;

            UsingDirectiveSyntax[] directives = types
                .Distinct()
                .GroupBy(alias => alias.Alias, (_, values) => values.ToArray())
                .Where(definitions => definitions.Length is 1)
                .Select(definitions => definitions.Single())
                .OrderBy(alias => alias.Alias)
                .Select(alias => UsingDirective(IdentifierName(alias.TypeName))
                    .WithGlobalKeyword(Token(SyntaxKind.GlobalKeyword))
                    .WithAlias(NameEquals(IdentifierName(alias.Alias))))
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

            context.AddSource($"SourceKit.Generators.Grpc.ProtoAlias.{filePostfix}.g.cs", SourceText.From(text, Encoding.UTF8));
        };
    }

    private static bool IsApplicableType(INamedTypeSymbol type)
    {
        if (type.DeclaredAccessibility is not Accessibility.Public)
            return false;

        if (type.ContainingNamespace.ToDisplayString().StartsWith("Google"))
            return false;

        return true;
    }

    public readonly record struct MessageAliasDefinition(string Alias, string TypeName)
    {
        public static MessageAliasDefinition Create(INamedTypeSymbol symbol) => new(
            Alias: $"Proto{symbol.Name}",
            TypeName: symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
    }
}
