using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Helper = SourceKit.Analyzers.Enumerable.Helpers.CannotLinqChainAfterTerminalOperationHelper;

namespace SourceKit.Analyzers.Enumerable.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class CannotLinqChainAfterTerminalOperationAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "SK1301";
    public const string Title = nameof(CannotLinqChainAfterTerminalOperationAnalyzer);

    public const string Format = """Cannot chain LINQ methods after terminal operation {0}""";

    public static readonly DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(
        DiagnosticId,
        Title,
        Format,
        "Performance",
        DiagnosticSeverity.Error,
        true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(Descriptor);

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);

        context.RegisterSyntaxNodeAction(c =>
        {
            var semanticModel = c.SemanticModel;
            if (c.Node is not MemberAccessExpressionSyntax node || !Helper.IsLinqEnumerable(node, semanticModel))
                return;

            var termNode = node.DescendantNodes(x =>  x is not ArgumentListSyntax)
                .OfType<MemberAccessExpressionSyntax>()
                .Where(node => Helper.IsLinqEnumerable(node, semanticModel))
                .FirstOrDefault(expressionSyntax => Helper.TerminationMethods.Contains(expressionSyntax.GetLastToken().ToString()));

            if (termNode == null) return;

            var linqToken = node.GetLastToken();
            c.ReportDiagnostic(Diagnostic.Create(Descriptor, linqToken.GetLocation(), termNode.Name));
        }, SyntaxKind.SimpleMemberAccessExpression);
    }
}