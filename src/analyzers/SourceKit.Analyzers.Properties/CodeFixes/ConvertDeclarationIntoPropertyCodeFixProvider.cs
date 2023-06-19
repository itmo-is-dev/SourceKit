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

    private static async Task<Document> ReplaceField(
        CodeFixContext context,
        Diagnostic diagnostic)
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
        
        // TODO: разделить это на методы для get и set, а то first и last вернут одно и тоже, если нет set
        // TODO: протестировать с разными kind и прочее
        var getMethod = root.FindNode(diagnostic.AdditionalLocations.First().SourceSpan);
        var setMethod = root.FindNode(diagnostic.AdditionalLocations.Last().SourceSpan);

        if (getMethod is not MethodDeclarationSyntax getMethodDeclaration ||
            setMethod is not MethodDeclarationSyntax setMethodDeclaration)
        {
            return document;
        }

        var propertyKind = getMethodDeclaration.Modifiers.First().Kind();
        var setterKind = setMethodDeclaration.Modifiers.First().Kind();

        var propertyDeclaration =
            SyntaxFactory.PropertyDeclaration(
                    variableTypeNode,
                    SyntaxFactory
                        .Identifier(GetPropertyName(variableNode.ToString()))
                        .NormalizeWhitespace())
                .AddAccessorListAccessors(
                    SyntaxFactory
                        .AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                        .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)))
                .NormalizeWhitespace()
                .AddModifiers(SyntaxFactory.Token(propertyKind))
                .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);

        if (diagnostic.AdditionalLocations.Count == 2)
        {
            var setterDeclaration = SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));

            if (setterKind is not SyntaxKind.PublicKeyword)
            {
                setterDeclaration = setterDeclaration.AddModifiers(SyntaxFactory.Token(setterKind));
            }

            propertyDeclaration = propertyDeclaration.AddAccessorListAccessors(setterDeclaration);
        }

        if (variableDeclarationNode.ChildNodes().OfType<VariableDeclaratorSyntax>().Count() > 1)
        {
            editor.InsertBefore(fieldDeclarationNode, new[] { propertyDeclaration });
            editor.RemoveNode(variableNode, SyntaxRemoveOptions.KeepNoTrivia);
        }
        else
        {
            editor.ReplaceNode(fieldDeclarationNode, propertyDeclaration);
        }

        switch (diagnostic.AdditionalLocations.Count)
        {
            case 1:
                editor.RemoveNode(getMethodDeclaration);
                break;
            case 2:
                editor.RemoveNode(getMethodDeclaration);
                editor.RemoveNode(setMethodDeclaration);
                break;
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