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
        Diagnostic diagnostic = context.Diagnostics.First();
        Location location = diagnostic.Location;
        SyntaxNode? root = await context.Document.GetSyntaxRootAsync();
        SyntaxNode? node = root?.FindNode(location.SourceSpan);
        
        if (node == null) return;
        
        SyntaxToken terminalOperationWithoutParamsToken = node.GetLastToken();
        string title = string.Format(Title, terminalOperationWithoutParamsToken);

        var codeFixAction = CodeAction.Create(
            title: title,
            createChangedSolution: c => RemoveToList(context.Document, node, c),
            equivalenceKey: title);
        
        context.RegisterCodeFix(codeFixAction, diagnostic);
    }

    private static async Task<Solution> RemoveToList(Document document, SyntaxNode node, CancellationToken cancellationToken)
    {
        var editor = await DocumentEditor.CreateAsync(document, cancellationToken);
        InvocationExpressionSyntax? invocationExpressions = node.AncestorsAndSelf().OfType<InvocationExpressionSyntax>().First();
        InvocationExpressionSyntax? before = invocationExpressions.DescendantNodes().OfType<InvocationExpressionSyntax>().First();
        editor.ReplaceNode(invocationExpressions, before);
        return editor.GetChangedDocument().Project.Solution;
    }
}