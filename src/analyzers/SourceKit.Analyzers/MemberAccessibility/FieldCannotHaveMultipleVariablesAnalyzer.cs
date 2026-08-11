using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceKit.Analyzers.MemberAccessibility;

[Generator]
public class FieldCannotHaveMultipleVariablesAnalyzer : IIncrementalGenerator
{
    public const string DiagnosticId = "SK1102";
    public const string Format = """Each field must have separate declaration""";

    public static readonly DiagnosticDescriptor Descriptor = new(
        DiagnosticId,
        "Field cannot have multiple variables",
        Format,
        "Design",
        DiagnosticSeverity.Error,
        true);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<FieldDeclarationSyntax> syntaxProvider = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => node is FieldDeclarationSyntax { Declaration.Variables.Count: > 1 },
                static (context, _) => (FieldDeclarationSyntax)context.Node);

        context.RegisterSourceOutput(
            syntaxProvider,
            static (context, fieldSyntax) =>
            {
                context.ReportDiagnostic(Diagnostic.Create(Descriptor, fieldSyntax.GetLocation()));
            });
    }
}
