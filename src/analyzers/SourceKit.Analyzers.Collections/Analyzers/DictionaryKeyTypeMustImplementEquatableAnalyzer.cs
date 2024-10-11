using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SourceKit.Extensions;

namespace SourceKit.Analyzers.Collections.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class DictionaryKeyTypeMustImplementEquatableAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "SK1500";
    public const string Title = nameof(DictionaryKeyTypeMustImplementEquatableAnalyzer);

    public const string Format = """Type argument for TKey must implement IEquatable""";

    public static readonly DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(
        DiagnosticId,
        Title,
        Format,
        "Design",
        DiagnosticSeverity.Error,
        true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [Descriptor];

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.RegisterSyntaxNodeAction(AnalyzeGeneric, SyntaxKind.GenericName);
    }

    private void AnalyzeGeneric(SyntaxNodeAnalysisContext context)
    {
        var node = (GenericNameSyntax)context.Node;

        if (context.SemanticModel.GetDeclaredSymbol(node) is not INamedTypeSymbol symbol)
            return;

        if (TryGetDictionaryKeySymbol(symbol, typeof(Dictionary<,>), context, out INamedTypeSymbol? keySymbol) is false
            & TryGetDictionaryKeySymbol(symbol, typeof(IReadOnlyDictionary<,>), context, out keySymbol) is false
            & TryGetDictionaryKeySymbol(symbol, typeof(IDictionary<,>), context, out keySymbol) is false)
        {
            return;
        }

        if (keySymbol is null || keySymbol.MetadataName is "TKey")
            return;

        if (keySymbol.TypeKind is TypeKind.Enum)
            return;

        INamedTypeSymbol equatableSymbol = context.Compilation.GetTypeSymbol(typeof(IEquatable<>));

        INamedTypeSymbol madeEquatableSymbol = equatableSymbol
            .Construct(keySymbol.WithNullableAnnotation(NullableAnnotation.None));

        IEnumerable<INamedTypeSymbol> foundEquatableSymbols = keySymbol
            .FindAssignableTypesConstructedFrom(equatableSymbol);

        bool hasCorrectEquatableImplementation = foundEquatableSymbols
            .Select(x => x.TypeArguments.First())
            .Any(x => madeEquatableSymbol.Equals(x, SymbolEqualityComparer.Default));

        if (hasCorrectEquatableImplementation is false)
            return;

        var diag = Diagnostic.Create(Descriptor, node.GetLocation());
        context.ReportDiagnostic(diag);
    }

    private static bool TryGetDictionaryKeySymbol(
        INamedTypeSymbol nameSymbol,
        Type dictionaryType,
        SyntaxNodeAnalysisContext context,
        out INamedTypeSymbol? keySymbol)
    {
        INamedTypeSymbol dictionarySymbol = context.Compilation.GetTypeSymbol(dictionaryType);
        INamedTypeSymbol? implementationSymbol = nameSymbol.FindAssignableTypeConstructedFrom(dictionarySymbol);

        keySymbol = implementationSymbol?.TypeArguments.FirstOrDefault() as INamedTypeSymbol;
        return keySymbol is not null;
    }
}