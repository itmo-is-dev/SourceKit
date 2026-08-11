using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;

namespace SourceKit.Analyzers.MemberAccessibility;

[Generator]
public class FieldCannotBePublicAnalyzer : IIncrementalGenerator
{
    public const string DiagnosticId = "SK1101";
    public const string Format = """Field '{0} {1}' cannot be public""";

    public static readonly DiagnosticDescriptor Descriptor = new(
        DiagnosticId,
        "Field cannot be public",
        Format,
        "Design",
        DiagnosticSeverity.Error,
        true);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<FieldDeclarationSyntax> fields = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => node is FieldDeclarationSyntax,
                static (context, _) => (FieldDeclarationSyntax)context.Node)
            .Where(static syntax => syntax.HasModifiers(SyntaxKind.PublicKeyword))
            .Where(static syntax => syntax.HasModifiers(SyntaxKind.ConstKeyword) is false)
            .Where(static syntax => syntax.HasModifiers(SyntaxKind.StaticKeyword, SyntaxKind.ReadOnlyKeyword) is false);

        context.RegisterSourceOutput(
            fields,
            static (context, fieldSyntax) =>
            {
                foreach (VariableDeclaratorSyntax variable in fieldSyntax.Declaration.Variables)
                {
                    var diagnostic = Diagnostic.Create(Descriptor,
                        variable.GetLocation(),
                        fieldSyntax.Declaration.Type,
                        variable.Identifier.Text);

                    context.ReportDiagnostic(diagnostic);
                }
            });
    }
}
