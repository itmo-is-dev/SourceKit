using Microsoft.CodeAnalysis;

namespace SourceKit.Generators.Grpc.Receivers;

public class ProtoMessageAliasReceiver : ISyntaxContextReceiver
{
    private readonly List<INamedTypeSymbol> _symbols = [];

    public IReadOnlyCollection<INamedTypeSymbol> Symbols => _symbols;

    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        INamedTypeSymbol? messageInterfaceSymbol = context.SemanticModel.Compilation
            .GetTypeByMetadataName(Constants.ProtobufMessageInterfaceFullyQualifiedName);

        INamedTypeSymbol? enumAttributeSymbol = context.SemanticModel.Compilation
            .GetTypeByMetadataName(Constants.ProtobufOriginalNameAttributeFullyQualifiedName);

        if (messageInterfaceSymbol is null || enumAttributeSymbol is null)
            return;

        ISymbol? symbolInfo = context.SemanticModel.GetDeclaredSymbol(context.Node);

        if (symbolInfo is not INamedTypeSymbol symbol)
            return;

        if (IsProtoClass(symbol) is false && IsProtoEnum(symbol) is false)
            return;

        if (symbol.ContainingType is not null)
            return;

        _symbols.Add(symbol);

        bool IsProtoClass(INamedTypeSymbol type)
        {
            return type.TypeKind is TypeKind.Class
                   && type.AllInterfaces.Contains(messageInterfaceSymbol, SymbolEqualityComparer.Default);
        }

        bool IsProtoEnum(INamedTypeSymbol type)
        {
            if (type.TypeKind is not TypeKind.Enum)
                return false;

            return type
                .GetMembers()
                .All(member => member.GetAttributes()
                    .Any(attr => attr
                        .AttributeClass?.Equals(enumAttributeSymbol, SymbolEqualityComparer.Default) is true));
        }
    }
}
