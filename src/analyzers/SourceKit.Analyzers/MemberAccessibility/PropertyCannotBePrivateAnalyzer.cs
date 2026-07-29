using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;

namespace SourceKit.Analyzers.MemberAccessibility;

[Generator]
public class PropertyCannotBePrivateAnalyzer : IIncrementalGenerator
{
    public const string DiagnosticId = "SK1100";
    public const string Title = nameof(PropertyCannotBePrivateAnalyzer);

    public const string Format = """Property '{0} {1}' cannot be private""";

    public static readonly DiagnosticDescriptor Descriptor = new(
        DiagnosticId,
        Title,
        Format,
        "Design",
        DiagnosticSeverity.Error,
        true);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<PropertyDeclarationSyntax> properties = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => node is PropertyDeclarationSyntax,
                static (context, _) => (PropertyDeclarationSyntax)context.Node)
            .Where(static syntax => syntax.HasModifiers(SyntaxKind.PrivateKeyword));

        context.RegisterSourceOutput(
            properties,
            static (context, syntax) =>
            {
                var diagnostic = Diagnostic.Create(
                    Descriptor,
                    syntax.GetLocation(),
                    syntax.Type,
                    syntax.Identifier.Text);

                context.ReportDiagnostic(diagnostic);
            });
    }
}
