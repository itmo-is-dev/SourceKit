using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using SourceKit.Analyzers.Enumerable.Analyzers;
using Helper = SourceKit.Analyzers.Enumerable.Helpers.CannotLinqChainAfterTerminalOperationHelper;

namespace SourceKit.Analyzers.Enumerable.CodeFixes;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(RemoveTerminalOperationCodeFixProvider))]
public class RemoveTerminalOperationCodeFixProvider : CodeFixProvider
{
    public const string Title = "Remove {0}";
    
    public override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(CannotLinqChainAfterTerminalOperationAnalyzer.DiagnosticId);
    
    public override FixAllProvider GetFixAllProvider()
        => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var diagnostic = context.Diagnostics.First();
        var semantic = await context.Document.GetSemanticModelAsync();
        var location = diagnostic.Location.SourceSpan;
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
        var termNode = root?
            .FindToken(location.Start).Parent?
            .Ancestors()
            .FirstOrDefault()?
            .DescendantNodes()
            .OfType<MemberAccessExpressionSyntax>()
            .FirstOrDefault(x => Helper.TerminationMethods.Contains(x.GetLastToken().ToString()));

        var token = termNode?.GetLastToken();
        
        if (token == null) return;
        context.RegisterCodeFix(
            CodeAction.Create(
                title: string.Format(Title, token),
                createChangedSolution: c => RemoveToList(context.Document, termNode, c),
                equivalenceKey: string.Format(Title, token)), diagnostic);
    }

    private static async Task<Solution> RemoveToList(Document document, SyntaxNode? node, CancellationToken ctx)
    {

        var editor = await DocumentEditor.CreateAsync(document, ctx);
        var invocationExpressions = node?.Parent ?? throw new InvalidOperationException();
        var before = node.ChildNodes().First() ?? throw new InvalidOperationException();
        editor.ReplaceNode(invocationExpressions, before);
        return editor.GetChangedDocument().Project.Solution;
    }
}