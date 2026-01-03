using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SourceKit.Analyzers.MemberAccessibility.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public class FieldCannotBePublicAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "SK1101";
    public const string Title = nameof(FieldCannotBePublicAnalyzer);

    public const string Format = """Field '{0} {1}' cannot be public""";

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
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.RegisterSyntaxNodeAction(AnalyzeField, SyntaxKind.FieldDeclaration);
    }

    private void AnalyzeField(SyntaxNodeAnalysisContext context)
    {
        var fieldSyntax = (FieldDeclarationSyntax)context.Node;

        if (fieldSyntax.Modifiers.All(x => x.IsKind(SyntaxKind.PublicKeyword) is false))
        {
            return;
        }

        foreach (VariableDeclaratorSyntax variable in fieldSyntax.Declaration.Variables)
        {
            Location location = variable.GetLocation();
            var diagnostic = Diagnostic.Create(Descriptor, location, fieldSyntax.Declaration.Type, variable.Identifier.Text);

            context.ReportDiagnostic(diagnostic);
        }
    }
}
