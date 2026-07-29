using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;

namespace SourceKit.Analyzers.Collections;

[Generator]
public class DictionaryKeyTypeMustImplementEquatableAnalyzer : IIncrementalGenerator
{
    public const string DiagnosticId = "SK1500";
    public const string Title = nameof(DictionaryKeyTypeMustImplementEquatableAnalyzer);

    public const string Format = """Type argument for TKey must implement IEquatable""";

    public static readonly DiagnosticDescriptor Descriptor = new(
        DiagnosticId,
        Title,
        Format,
        "Design",
        DiagnosticSeverity.Error,
        true);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<GenericNameSyntax> syntaxProvider = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => node is GenericNameSyntax,
                static (context, _) => ((GenericNameSyntax)context.Node, context.SemanticModel))
            .WithComparer(static (node, _) => node.Identifier)
            .Select(static (node, semanticModel) =>
            {
                if (semanticModel.GetSymbolInfo(node).Symbol is not INamedTypeSymbol symbol)
                    return IncrementalResult.Skip;

                if (TryGetDictionaryKeySymbol(symbol, typeof(Dictionary<,>), semanticModel, out INamedTypeSymbol? keySymbol) is false
                    & TryGetDictionaryKeySymbol(symbol, typeof(IReadOnlyDictionary<,>), semanticModel, out keySymbol) is false
                    & TryGetDictionaryKeySymbol(symbol, typeof(IDictionary<,>), semanticModel, out keySymbol) is false)
                {
                    return IncrementalResult.Skip;
                }

                if (keySymbol is null || keySymbol.MetadataName is "TKey")
                    return IncrementalResult.Skip;

                if (keySymbol.TypeKind is TypeKind.Enum)
                    return IncrementalResult.Skip;

                return IncrementalResult.Success((node, semanticModel, keySymbol));
            })
            .Unwrap(context)
            .Select(static (node, semanticModel, keySymbol) =>
            {
                INamedTypeSymbol equatableSymbol = semanticModel.Compilation.GetTypeSymbol(typeof(IEquatable<>));

                IEnumerable<INamedTypeSymbol> foundEquatableSymbols = keySymbol
                    .FindAssignableTypesConstructedFrom(equatableSymbol);

                bool hasCorrectEquatableImplementation = foundEquatableSymbols
                    .Select(x => x.TypeArguments.First())
                    .Any(x => keySymbol.Equals(x, SymbolEqualityComparer.Default) || keySymbol.IsAssignableTo(x));

                if (hasCorrectEquatableImplementation)
                    return IncrementalResult.Skip;

                return IncrementalResult.Success(node);
            })
            .Unwrap(context);

        context.RegisterSourceOutput(
            syntaxProvider,
            static (context, node) => context.ReportDiagnostic(Diagnostic.Create(Descriptor, node.GetLocation())));
    }

    private static bool TryGetDictionaryKeySymbol(
        INamedTypeSymbol nameSymbol,
        Type dictionaryType,
        SemanticModel semanticModel,
        out INamedTypeSymbol? keySymbol)
    {
        INamedTypeSymbol dictionarySymbol = semanticModel.Compilation.GetTypeSymbol(dictionaryType);
        INamedTypeSymbol? implementationSymbol = nameSymbol.FindAssignableTypeConstructedFrom(dictionarySymbol);

        keySymbol = implementationSymbol?.TypeArguments.FirstOrDefault() as INamedTypeSymbol;
        return keySymbol is not null;
    }
}
