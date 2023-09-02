using System.Collections.Immutable;
using System.Runtime.CompilerServices;
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

        var isSuppressFound = nodeToFix.DescendantNodesAndSelf()
            .Any(node => node.IsKind(SyntaxKind.SuppressNullableWarningExpression));

        SyntaxNode? fixedNode;

        if (isSuppressFound)
        {
            fixedNode = FixedNodeConstructor.FromNullForgivingExpression(nodeToFix);
        }
        else
        {
            fixedNode = nodeToFix.ChildNodes().FirstOrDefault();
        }

        if (fixedNode is null)
        {
            return;
        }

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static SyntaxNode FindNodeToFix(SyntaxNode currentNode)
    {
        return currentNode.Parent is not null && IsNodeProcessable(currentNode.Parent) 
            ? FindNodeToFix(currentNode.Parent) 
            : currentNode;
    }

    private static bool IsNodeProcessable(SyntaxNode node)
    {
        return node.Kind() switch
        {
            SyntaxKind.InvocationExpression => true,
            SyntaxKind.SimpleMemberAccessExpression => true,
            SyntaxKind.PointerMemberAccessExpression => true,
            _ => false
        };
    }
}