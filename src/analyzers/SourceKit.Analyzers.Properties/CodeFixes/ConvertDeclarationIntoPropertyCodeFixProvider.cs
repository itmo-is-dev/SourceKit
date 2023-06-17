using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Analyzers.Properties.Analyzers;

namespace SourceKit.Analyzers.Properties.CodeFixes;

public class ConvertDeclarationIntoPropertyCodeFixProvider : CodeFixProvider
{
    public const string Title = "Convert into auto-property";

    public override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(DeclarationCouldBeConvertedToPropertyAnalyzer.DiagnosticId);

    public override FixAllProvider GetFixAllProvider()
        => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        context.CancellationToken.ThrowIfCancellationRequested();

        IEnumerable<Task> declarationCouldBeConvertedToPropertyDiagnostics = context.Diagnostics
            .Where(x => x.Id.Equals(DeclarationCouldBeConvertedToPropertyAnalyzer.DiagnosticId))
            .Select(x => ProvideConvertDeclarationIntoPropertyCodeFix(context, x));

        await Task.WhenAll(declarationCouldBeConvertedToPropertyDiagnostics);
    }

    private static async Task ProvideConvertDeclarationIntoPropertyCodeFix(CodeFixContext context,
        Diagnostic diagnostic)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);

        if (root is null)
        {
            return;
        }

        var action = CodeAction.Create(
            Title,
            equivalenceKey: nameof(Title),
            createChangedDocument: _ =>
            {
                var newRoot = ReplaceField(root, diagnostic);
                var document = context.Document.WithSyntaxRoot(newRoot);
                return Task.FromResult(document);
            });

        context.RegisterCodeFix(action, diagnostic);
    }

    private static SyntaxNode ReplaceField(SyntaxNode root, Diagnostic diagnostic)
    {
        var variableNode = root.FindNode(diagnostic.Location.SourceSpan);
        if (variableNode.Parent is not VariableDeclarationSyntax variableDeclarationNode)
        {
            return root;
        }

        var variableTypeNode = variableDeclarationNode.Type;

        if (variableDeclarationNode.Parent is not FieldDeclarationSyntax fieldDeclarationNode)
        {
            return root;
        }

        var isPublic = fieldDeclarationNode.Modifiers.Any(modifier => modifier.Kind() is SyntaxKind.PublicKeyword);

        var newRoot = root;
        if (isPublic)
        {
            if (variableDeclarationNode.ChildNodes().OfType<VariableDeclaratorSyntax>().Count() > 1)
            {
                var propertyDeclaration =
                    SyntaxFactory.PropertyDeclaration(
                            variableTypeNode,
                            SyntaxFactory.Identifier(variableNode.ToString())
                                .NormalizeWhitespace())
                        .AddAccessorListAccessors(
                            SyntaxFactory.AccessorDeclaration(
                                    SyntaxKind.GetAccessorDeclaration)
                                .WithSemicolonToken(
                                    SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                            SyntaxFactory.AccessorDeclaration(
                                    SyntaxKind.SetAccessorDeclaration)
                                .WithSemicolonToken(
                                    SyntaxFactory.Token(SyntaxKind.SemicolonToken)))
                        .NormalizeWhitespace()
                        .AddModifiers(
                            SyntaxFactory.Token(
                                    SyntaxFactory.TriviaList(SyntaxFactory.Tab),
                                    SyntaxKind.PublicKeyword,
                                    SyntaxFactory.TriviaList())
                                .WithTrailingTrivia(SyntaxFactory.Space))
                        .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);
                
                // TODO: ѕосле вставки новой ноды, стара€ нода не удал€етс€.
                newRoot = root
                    .InsertNodesAfter(fieldDeclarationNode, new[] { propertyDeclaration })
                    .RemoveNode(variableNode, SyntaxRemoveOptions.KeepNoTrivia);
            }
            else
            {
                // TODO: «аменить на Replace и замен€ть на свойство
                newRoot = root.RemoveNode(fieldDeclarationNode, SyntaxRemoveOptions.KeepNoTrivia);
            }
        }


        return newRoot ?? root;
    }
}