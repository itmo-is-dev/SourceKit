using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editing;
using SourceKit.Analyzers.Nullable.Analyzers;
using SourceKit.Analyzers.Nullable.CodeFixes.Tools;

namespace SourceKit.Analyzers.Nullable.CodeFixes;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(PropagateNullableCorrectlyCodeFixProvider))]
public class PropagateNullableCorrectlyCodeFixProvider : CodeFixProvider
{
    public const string Title = "Propagate nullable types correctly";

    public override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(NullForgivingOperatorNotAllowedAnalyzer.DiagnosticId);

    public override FixAllProvider GetFixAllProvider()
        => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var editor = await DocumentEditor.CreateAsync(context.Document);
        IEnumerable<Task> diagnostics =
            context.Diagnostics.Select(diagnostic => FixDiagnostic(context, editor, diagnostic));
        await Task.WhenAll(diagnostics);
    }

    private async Task FixDiagnostic(CodeFixContext context, DocumentEditor editor, Diagnostic diagnostic)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root is null)
        {
            return;
        }

        var diagnosticSpan = diagnostic.Location.SourceSpan;
        var diagnosticNode = root.FindNode(diagnosticSpan);
        var nodeToFix = FindNodeToFix(diagnosticNode);
        var fixedNode = nodeToFix.DescendantNodesAndSelf()
            .Any(node => node.IsKind(SyntaxKind.SuppressNullableWarningExpression))
            ? FixedNodeConstructor.FromNullForgivingExpression(nodeToFix)
            : nodeToFix.ChildNodes().First();

        var codeFixResult = TypeChanger.TryToChangeTypeIfNecessary(nodeToFix, fixedNode);
        editor.ReplaceNode(codeFixResult.Initial, codeFixResult.Fixed);


        context.RegisterCodeFix(
            CodeAction.Create(
                title: Title,
                createChangedDocument: c =>
                {
                    var changedDocument = editor.GetChangedDocument();
                    return Task.FromResult(changedDocument);
                },
                equivalenceKey: nameof(Title)),
            diagnostic);
    }

    private SyntaxNode FindNodeToFix(SyntaxNode currentNode)
    {
        while (true)
        {
            if (currentNode.Parent == null ||
                (!currentNode.Parent.IsKind(SyntaxKind.InvocationExpression) &&
                 !currentNode.Parent.IsKind(SyntaxKind.SimpleMemberAccessExpression) &&
                 !currentNode.Parent.IsKind(SyntaxKind.PointerMemberAccessExpression)))
            {
                return currentNode;
            }

            currentNode = currentNode.Parent;
        }
    }
}