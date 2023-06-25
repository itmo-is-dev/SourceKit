using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace SourceKit.Analyzers.Nullable.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class NullableDisableNotAllowedAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "SK1201";
    public const string Title = nameof(NullableDisableNotAllowedAnalyzer);

    public const string Format = """#nullable disable is not allowed""";

    public static readonly DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(
        DiagnosticId,
        Title,
        Format,
        "Usage",
        DiagnosticSeverity.Error,
        true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(Descriptor);

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.RegisterSyntaxTreeAction(AnalyzeSyntaxTree);
    }
    

    private static void AnalyzeSyntaxTree(SyntaxTreeAnalysisContext context)
    {
        var root = context.Tree.GetRoot(context.CancellationToken);
        var nullableDirectives = root.DescendantNodesAndTokens(descendIntoTrivia: true)
            .Where(nodeOrToken => nodeOrToken.IsNode && nodeOrToken.AsNode() is DirectiveTriviaSyntax)
            .Select(nodeOrToken => nodeOrToken.AsNode())
            .OfType<NullableDirectiveTriviaSyntax>()
            .Where(nullableDirective => nullableDirective.SettingToken.Kind() == SyntaxKind.DisableKeyword);

        foreach (var nullableDirective in nullableDirectives)
        {
            var diagnostic = Diagnostic.Create(Descriptor, nullableDirective.GetLocation());
            context.ReportDiagnostic(diagnostic);
        }
    }
}