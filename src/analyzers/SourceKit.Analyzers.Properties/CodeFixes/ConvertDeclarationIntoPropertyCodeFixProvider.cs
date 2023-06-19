using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
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

    private static async Task ProvideConvertDeclarationIntoPropertyCodeFix(
        CodeFixContext context,
        Diagnostic diagnostic)
    {
        var action = CodeAction.Create(
            Title,
            equivalenceKey: nameof(Title),
            createChangedDocument: async _ =>
            {
                var newDocument = await ReplaceField(context, diagnostic);
                return newDocument;
            });

        context.RegisterCodeFix(action, diagnostic);
    }

    private static async Task<Document> ReplaceField(CodeFixContext context, Diagnostic diagnostic)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
        if (root is null)
        {
            return null;
        }

        var document = context.Document.WithSyntaxRoot(root);
        var editor = await DocumentEditor.CreateAsync(document);


        var variableNode = root.FindNode(diagnostic.Location.SourceSpan);
        if (variableNode.Parent is not VariableDeclarationSyntax variableDeclarationNode)
        {
            return document;
        }

        var variableTypeNode = variableDeclarationNode.Type;

        if (variableDeclarationNode.Parent is not FieldDeclarationSyntax fieldDeclarationNode)
        {
            return document;
        }

        var isPublic = fieldDeclarationNode.Modifiers.Any(modifier => modifier.Kind() is SyntaxKind.PublicKeyword);
        if (isPublic)
        {
            var propertyDeclaration =
                SyntaxFactory.PropertyDeclaration(
                        variableTypeNode,
                        SyntaxFactory
                            .Identifier(GetPropertyName(variableNode.ToString()))
                            .NormalizeWhitespace())
                    .AddAccessorListAccessors(
                        SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                            .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                        SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                            .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)))
                    .NormalizeWhitespace()
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);

            if (variableDeclarationNode.ChildNodes().OfType<VariableDeclaratorSyntax>().Count() > 1)
            {
                editor.InsertBefore(fieldDeclarationNode, new[] { propertyDeclaration });
                editor.RemoveNode(variableNode, SyntaxRemoveOptions.KeepNoTrivia);
            }
            else
            {
                editor.ReplaceNode(fieldDeclarationNode, propertyDeclaration);
            }
        }

        var normalizedRoot = editor.GetChangedRoot().NormalizeWhitespace();

        return context.Document.WithSyntaxRoot(normalizedRoot);
    }

    private static string GetPropertyName(string variableName)
    {
        variableName = variableName.Insert(0, char.ToUpper(variableName[0]).ToString());
        return variableName.Remove(1, 1);
    }
}