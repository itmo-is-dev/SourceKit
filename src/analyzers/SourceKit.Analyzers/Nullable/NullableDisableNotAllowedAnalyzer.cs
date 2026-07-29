using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceKit.Analyzers.Nullable;

[Generator]
public class NullableDisableNotAllowedAnalyzer : IIncrementalGenerator
{
    public const string DiagnosticId = "SK1201";
    public const string Title = nameof(NullableDisableNotAllowedAnalyzer);

    public const string Format = """#nullable disable is not allowed""";

    public static readonly DiagnosticDescriptor Descriptor = new(
        DiagnosticId,
        Title,
        Format,
        "Usage",
        DiagnosticSeverity.Error,
        true);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var syntaxProvider = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => node is CompilationUnitSyntax,
                static (context, _) => (CompilationUnitSyntax)context.Node)
            .SelectMany((node, _) => node.DescendantTrivia())
            .Where(static node => node.IsKind(SyntaxKind.NullableDirectiveTrivia))
            .Where(static node => (node.GetStructure() as NullableDirectiveTriviaSyntax)!.SettingToken.IsKind(SyntaxKind.DisableKeyword));

        context.RegisterSourceOutput(
            syntaxProvider,
            static (context, node) =>
            {
                var diagnostic = Diagnostic.Create(Descriptor, node.GetLocation());
                context.ReportDiagnostic(diagnostic);
            });
    }
}
