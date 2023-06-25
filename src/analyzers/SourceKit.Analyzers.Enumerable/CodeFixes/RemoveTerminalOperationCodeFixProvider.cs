using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using SourceKit.Analyzers.Enumerable.Analyzers;

namespace SourceKit.Analyzers.Enumerable.CodeFixes;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(RemoveTerminalOperationCodeFixProvider))]
public class RemoveTerminalOperationCodeFixProvider : CodeFixProvider
{
    public const string Title = "Remove {0}";
    
    public override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(DoNotEnumerateCollectionInLinqChain.DiagnosticId);
    
    public override FixAllProvider GetFixAllProvider()
        => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var diagnostic = context.Diagnostics.First();
        var location = diagnostic.Location;
        var root = await context.Document.GetSyntaxRootAsync();
        var node = root?.FindNode(location.SourceSpan);
        if (node == null) return;
        var token = node.GetLastToken();
        var title = string.Format(Title, token);
        context.RegisterCodeFix(
            CodeAction.Create(
                title: title,
                createChangedSolution: c => RemoveToList(context.Document, node, c),
                equivalenceKey: title)
            , diagnostic);
    }

    private static async Task<Solution> RemoveToList(Document document, SyntaxNode node, CancellationToken ctx)
    {
    
        var editor = await DocumentEditor.CreateAsync(document, ctx);
        var invocationExpressions = node.AncestorsAndSelf().OfType<InvocationExpressionSyntax>().First();
        var before = invocationExpressions.DescendantNodes().OfType<InvocationExpressionSyntax>().First();
        editor.ReplaceNode(invocationExpressions, before);
        return editor.GetChangedDocument().Project.Solution;
    }
}