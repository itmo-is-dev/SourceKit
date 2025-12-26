using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SourceKit.Analyzers.MemberAccessibility.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public class PropertyCannotBePrivateAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "SK1100";
    public const string Title = nameof(PropertyCannotBePrivateAnalyzer);

    public const string Format = """Property '{0} {1}' cannot be private""";

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

        context.RegisterCompilationStartAction(compilationContext =>
        {
            compilationContext.RegisterSyntaxNodeAction(AnalyzeProperty, SyntaxKind.PropertyDeclaration);
        });
    }

    private void AnalyzeProperty(SyntaxNodeAnalysisContext context)
    {
        var propertySyntax = (PropertyDeclarationSyntax)context.Node;

        if (propertySyntax.Modifiers.All(x => x.Kind() is not SyntaxKind.PrivateKeyword))
        {
            return;
        }
        
        Location location = propertySyntax.GetLocation();
        var diagnostic = Diagnostic.Create(Descriptor, location, propertySyntax.Type,  propertySyntax.Identifier.Text);

        context.ReportDiagnostic(diagnostic);
    }
}
