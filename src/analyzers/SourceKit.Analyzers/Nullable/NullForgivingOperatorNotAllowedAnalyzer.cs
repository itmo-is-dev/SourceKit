using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceKit.Analyzers.Nullable;

[Generator]
public class NullForgivingOperatorNotAllowedAnalyzer : IIncrementalGenerator
{
    public const string DiagnosticId = "SK1200";
    public const string Title = nameof(NullForgivingOperatorNotAllowedAnalyzer);

    public const string Format = """Null forgiving operator is not allowed""";

    public static readonly DiagnosticDescriptor Descriptor = new(
        DiagnosticId,
        "Null forgiving operator is not allowed",
        Format,
        "Usage",
        DiagnosticSeverity.Error,
        true);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var syntaxProvider = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => node is PostfixUnaryExpressionSyntax syntax
                                    && syntax.IsKind(SyntaxKind.SuppressNullableWarningExpression)
                                    && node.Ancestors().All(static ancestor => ancestor.IsKind(SyntaxKind.SuppressNullableWarningExpression) is false),
                static (context, _) => (PostfixUnaryExpressionSyntax)context.Node);

        context.RegisterSourceOutput(
            syntaxProvider,
            static (context, node) => { context.ReportDiagnostic(Diagnostic.Create(Descriptor, node.GetLocation(), node)); });
    }
}
