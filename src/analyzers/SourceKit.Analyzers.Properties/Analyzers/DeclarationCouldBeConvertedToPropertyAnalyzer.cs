using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SourceKit.Analyzers.Properties.General;

namespace SourceKit.Analyzers.Properties.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class DeclarationCouldBeConvertedToPropertyAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "SK1400";
    public const string Title = nameof(DeclarationCouldBeConvertedToPropertyAnalyzer);

    public const string Format = """Field '{0}' could be converted to property""";

    public static readonly DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(
        DiagnosticId,
        Title,
        Format,
        "Design",
        DiagnosticSeverity.Warning,
        true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(Descriptor);

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.RegisterSyntaxNodeAction(AnalyzeClass, SyntaxKind.ClassDeclaration);
    }

    private void AnalyzeClass(SyntaxNodeAnalysisContext context)
    {
        var classDeclaration = (ClassDeclarationSyntax) context.Node;

        foreach (var field in classDeclaration.Members.OfType<FieldDeclarationSyntax>())
        {
            if (field.Modifiers.Any(modifier => modifier.Kind() is SyntaxKind.PublicKeyword))
            {
                AnalyzePublicVariableDeclaration(context, field.Declaration);
            }
            else
            {
                AnalyzeFieldsMethods(context, field.Declaration, classDeclaration);
            }
        }
    }

    private void AnalyzePublicVariableDeclaration(
        SyntaxNodeAnalysisContext context,
        VariableDeclarationSyntax variableDeclaration)
    {
        foreach (var variable in variableDeclaration.Variables)
        {
            var location = variable.Identifier.GetLocation();
            var diagnostic = Diagnostic.Create(Descriptor, location, new[] { location }, variable.Identifier.Text);
            context.ReportDiagnostic(diagnostic);
        }
    }

    private void AnalyzeFieldsMethods(
        SyntaxNodeAnalysisContext context,
        VariableDeclarationSyntax variableDeclaration,
        ClassDeclarationSyntax classDeclaration)
    {
        var semanticModel = context.SemanticModel;

        foreach (var variable in variableDeclaration.Variables)
        {
            var fieldWithMethods = Finder.FindFieldWithMethods(semanticModel, variable, classDeclaration);

            if (fieldWithMethods.GetMethods.Count == 0)
            {
                return;
            }

            var variableLocation = variable.Identifier.GetLocation();
            var classLocation = classDeclaration.GetLocation();
            var additionalLocations = new List<Location> { variableLocation, classLocation };

            context.ReportDiagnostic(Diagnostic.Create(
                Descriptor,
                variableLocation,
                additionalLocations,
                variable.Identifier.Text));

            foreach (var getMethod in fieldWithMethods.GetMethods)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    Descriptor,
                    getMethod.Identifier.GetLocation(),
                    additionalLocations,
                    variable.Identifier.Text));
            }

            foreach (var setMethod in fieldWithMethods.SetMethods)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    Descriptor,
                    setMethod.Identifier.GetLocation(),
                    additionalLocations,
                    variable.Identifier.Text));
            }
        }
    }
}