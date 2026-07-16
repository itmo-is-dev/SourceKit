using System.Collections.Immutable;
using System.Data.Common;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SourceKit.Analyzers.Persistence;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class DataReaderAsyncOverloadSuppressor : DiagnosticSuppressor
{
    private static readonly SuppressionDescriptor Descriptor = new(
        id: "SKSP1001",
        suppressedDiagnosticId: "CA1849",
        justification: "Row is most likely buffered, avoid extra task allocations");

    private static readonly ImmutableHashSet<string> MethodNames = typeof(DbDataReader)
        .GetMethods(BindingFlags.Public | BindingFlags.Instance)
        .Select(method => method.Name)
        .Where(name => name.StartsWith("Get") && name.Contains("Async") is false)
        .ToImmutableHashSet();

    public override ImmutableArray<SuppressionDescriptor> SupportedSuppressions { get; } = [Descriptor];

    public override void ReportSuppressions(SuppressionAnalysisContext context)
    {
        INamedTypeSymbol? readerSymbol = context.Compilation.GetTypeByMetadataName("System.Data.Common.DbDataReader");

        if (readerSymbol is null)
            return;

        foreach (Diagnostic diagnostic in context.ReportedDiagnostics)
        {
            SyntaxTree? syntaxTree = diagnostic.Location.SourceTree;
            SyntaxNode? node = syntaxTree?.GetRoot().FindNode(diagnostic.Location.SourceSpan);

            if (node is not InvocationExpressionSyntax invocationSyntax)
                continue;

            if (invocationSyntax.Expression is not MemberAccessExpressionSyntax accessSyntax)
                continue;

            if (MethodNames.Contains(accessSyntax.Name.Identifier.Text) is false)
                continue;

            SemanticModel semanticModel = context.GetSemanticModel(syntaxTree!);
            IOperation? targetOperation = semanticModel.GetOperation(accessSyntax.Expression);

            if (targetOperation?.Type?.Equals(readerSymbol, SymbolEqualityComparer.Default) is not true)
                continue;

            context.ReportSuppression(Suppression.Create(Descriptor, diagnostic));
        }
    }
}
