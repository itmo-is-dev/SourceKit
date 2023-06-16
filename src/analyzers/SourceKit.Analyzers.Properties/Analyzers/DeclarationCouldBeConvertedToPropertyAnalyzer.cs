using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SourceKit.Analyzers.Properties.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class DeclarationCouldBeConvertedToPropertyAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "SK1400";
    public const string Title = nameof(DeclarationCouldBeConvertedToPropertyAnalyzer);

    public const string Format = """Variable {0} could be converted to property.""";

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
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze |
                                               GeneratedCodeAnalysisFlags.ReportDiagnostics);

        context.RegisterSyntaxNodeAction(AnalyzeClass, SyntaxKind.ClassDeclaration);
    }

    private void AnalyzeClass(SyntaxNodeAnalysisContext context)
    {
        var classDeclaration = (ClassDeclarationSyntax) context.Node;

        var fields = classDeclaration.Members.OfType<FieldDeclarationSyntax>().ToList();
        fields.ForEach(field =>
        {
            if (field.Modifiers.Any(modifier => modifier.Kind() is SyntaxKind.PublicKeyword))
            {
                AnalyzePublicVariableDeclaration(context, field.Declaration);
            }

            if (field.Modifiers.Any(modifier => modifier.Kind() is SyntaxKind.PrivateKeyword))
            {
                FindMethods(context, field);
            }
        });
    }

    private void AnalyzePublicVariableDeclaration(
        SyntaxNodeAnalysisContext context,
        VariableDeclarationSyntax variableDeclaration)
    {
        foreach (var variable in variableDeclaration.Variables)
        {
            var location = variable.GetLocation();
            var diagnostic = Diagnostic.Create(Descriptor, location, variable.Identifier.Text);
            context.ReportDiagnostic(diagnostic);
        }
    }

    private void FindMethods(SyntaxNodeAnalysisContext context, FieldDeclarationSyntax field)
    {
    }
}