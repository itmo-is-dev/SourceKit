using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SourceKit.Analyzers.Nullable.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public class NullForgivingOperatorNotAllowedAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "SK1200";
    public const string Title = nameof(NullForgivingOperatorNotAllowedAnalyzer);

    public const string Format = """Null forgiving operator is not allowed""";

    public static readonly DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(
        DiagnosticId,
        Title,
        Format,
        "Usage",
        DiagnosticSeverity.Error,
        true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(Descriptor);

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze |
                                               GeneratedCodeAnalysisFlags.ReportDiagnostics);

        context.RegisterSyntaxNodeAction(AnalyseNullForgivingOperator,
            SyntaxKind.SuppressNullableWarningExpression);
    }


    private static void AnalyseNullForgivingOperator(SyntaxNodeAnalysisContext context)
    {
        var suppressionOperator = (PostfixUnaryExpressionSyntax) context.Node;
        
        if (suppressionOperator.Ancestors().Any(node => node.IsKind(SyntaxKind.SuppressNullableWarningExpression)))
        {
            return;
        }

        var diagnostic = Diagnostic.Create(Descriptor, suppressionOperator.GetLocation(), suppressionOperator);

        context.ReportDiagnostic(diagnostic);
    }
}