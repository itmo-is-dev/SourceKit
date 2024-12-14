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

        if (messageInterfaceSymbol is null)
            return;

        ISymbol? symbolInfo = context.SemanticModel.GetDeclaredSymbol(context.Node);

        if (symbolInfo is not INamedTypeSymbol symbol)
            return;

        if (symbol.AllInterfaces.Contains(messageInterfaceSymbol, SymbolEqualityComparer.Default) is false)
            return;

        if (symbol.ContainingType is not null)
            return;

        _symbols.Add(symbol);
    }
}