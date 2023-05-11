using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;
using SourceKit.Extensions;
using SourceKit.Generators.Builder.Tools;

namespace SourceKit.Generators.Builder.Receivers;

[Serializable]
public class BuilderAttributeSyntaxContextReceiver : ISyntaxContextReceiver
{
    private readonly List<INamedTypeSymbol> _typeSymbols;

    public BuilderAttributeSyntaxContextReceiver()
    {
        _typeSymbols = new List<INamedTypeSymbol>();
    }

    public IReadOnlyCollection<INamedTypeSymbol> TypeSymbols => _typeSymbols;

    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        var attributeType = context.SemanticModel.Compilation
            .GetTypeByMetadataName(Constants.GenerateBuilderAttributeFullyQualifiedName);

        if (attributeType is null)
            return;

        var symbolInfo = context.SemanticModel.GetSymbolInfo(context.Node);
        
        if (symbolInfo.Symbol is not INamedTypeSymbol namedTypeSymbol)
            return;

        if (namedTypeSymbol.HasAttribute(attributeType) is false)
            return;

        _typeSymbols.Add(namedTypeSymbol);
    }
}