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

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(Descriptor);

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.RegisterSyntaxNodeAction(AnalyzeGeneric , SyntaxKind.GenericName);
    }

    private SyntaxNode GetFirstGenericArgument(GenericNameSyntax node)
    {
        return node.TypeArgumentList.Arguments.First();
    }

    private void AnalyzeGeneric(SyntaxNodeAnalysisContext context)
    {
        var node = (GenericNameSyntax) context.Node;
        if (node.Identifier.Text != "Dictionary")
            return;

        var key = GetFirstGenericArgument(node);
            
        if (key is OmittedTypeArgumentSyntax) return;

        var keyType = context.GetSymbolFromContext(key) as ITypeSymbol;

        var interfaceNamedType = context.Compilation.GetTypeSymbol(typeof(IEquatable<>));

        var iequatableInterfaces = keyType.FindAssignableTypesConstructedFrom(interfaceNamedType);
        
        if (keyType.AllInterfaces.Any(c => iequatableInterfaces.Contains(c))) return;
        
        var diag = Diagnostic.Create(Descriptor, key.GetLocation());
        context.ReportDiagnostic(diag);
    }
}