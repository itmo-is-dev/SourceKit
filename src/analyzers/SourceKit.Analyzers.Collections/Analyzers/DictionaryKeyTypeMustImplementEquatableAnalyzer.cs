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

    private string GetRequiredInterface(string keyType)
    {
        return $"System.IEquatable<{keyType}>";
    }

    private void AnalyzeGeneric(SyntaxNodeAnalysisContext context)
    {
        var node = context.Node as GenericNameSyntax;
        if (node!.Identifier.Text != "Dictionary")
            return;

        var key = node.GetFirstGenericArgument();
            
        if (key is OmittedTypeArgumentSyntax)
            return;

        var keyType = context.GetSymbolFromContext(key) as ITypeSymbol;

        var interfaceName = GetRequiredInterface(keyType!.ToString());

        var iequatable = keyType.GetRequiredImplementedInterfaceByName(interfaceName);
            
        if (iequatable is not null)
            return;
            
        var diag = Diagnostic.Create(Descriptor, key.GetLocation());
        context.ReportDiagnostic(diag);
    }
}