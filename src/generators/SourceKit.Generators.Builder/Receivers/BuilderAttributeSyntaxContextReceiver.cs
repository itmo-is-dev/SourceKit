using Microsoft.CodeAnalysis;
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

        var symbolInfo = context.SemanticModel.GetDeclaredSymbol(context.Node);
        
        if (symbolInfo is not INamedTypeSymbol namedTypeSymbol)
            return;

        if (namedTypeSymbol.HasAttribute(attributeType) is false)
            return;
        
        if (namedTypeSymbol.IsPartial() is false)
            return;

        _typeSymbols.Add(namedTypeSymbol);
    }
}