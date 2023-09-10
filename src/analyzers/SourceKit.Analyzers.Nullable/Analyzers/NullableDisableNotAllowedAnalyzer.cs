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
        context.RegisterSyntaxNodeAction(AnalyzeNullableDirectiveTrivia, SyntaxKind.NullableDirectiveTrivia);
    }
    
    private static void AnalyzeNullableDirectiveTrivia(SyntaxNodeAnalysisContext context)
    {
        var nullableDirectiveTrivia = (NullableDirectiveTriviaSyntax)context.Node;

        if (nullableDirectiveTrivia.SettingToken.Kind() == SyntaxKind.DisableKeyword)
        {
            var diagnostic = Diagnostic.Create(Descriptor, nullableDirectiveTrivia.GetLocation());
            context.ReportDiagnostic(diagnostic);
        }
    }
}