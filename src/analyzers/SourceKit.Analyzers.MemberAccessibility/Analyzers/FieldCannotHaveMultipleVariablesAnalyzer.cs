using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SourceKit.Analyzers.MemberAccessibility.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public class FieldCannotHaveMultipleVariablesAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "SK1102";
    public const string Title = nameof(FieldCannotHaveMultipleVariablesAnalyzer);

    public const string Format = """Each field must have separate declaration""";

    public static readonly DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(DiagnosticId,
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
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.RegisterSyntaxNodeAction(AnalyzeField, SyntaxKind.FieldDeclaration);
    }

    private void AnalyzeField(SyntaxNodeAnalysisContext context)
    {
        var fieldSyntax = (FieldDeclarationSyntax)context.Node;

        if (fieldSyntax.Declaration.Variables.Count <= 1)
        {
            return;
        }

        Location location = fieldSyntax.GetLocation();
        var diagnostic = Diagnostic.Create(Descriptor, location);

        context.ReportDiagnostic(diagnostic);
    }
}
