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
            return context.Document;
        }

        var document = context.Document.WithSyntaxRoot(root);

        var variableNode = root.FindNode(diagnostic.Location.SourceSpan);
        if (variableNode is not VariableDeclaratorSyntax variableDeclarator)
        {
            return document;
        }

        if (variableNode.Parent is not VariableDeclarationSyntax variableDeclaration)
        {
            return document;
        }

        if (variableDeclaration.Parent is not FieldDeclarationSyntax fieldDeclaration)
        {
            return document;
        }

        return fieldDeclaration.Modifiers.First().Kind() is SyntaxKind.PublicKeyword
            ? await ProcessPublicField(context, document, variableDeclarator, variableDeclaration)
            : await ProcessNotPublicField(context, root, diagnostic, document, variableDeclarator, variableDeclaration);
    }

    private static async Task<Document> ProcessPublicField(
        CodeFixContext context,
        Document document,
        VariableDeclaratorSyntax variableDeclarator,
        VariableDeclarationSyntax variableDeclaration)
    {
        var editor = await DocumentEditor.CreateAsync(document);

        var variableTypeNode = variableDeclaration.Type;

        if (variableDeclaration.Parent is not FieldDeclarationSyntax fieldDeclarationNode)
        {
            return editor.OriginalDocument;
        }

        var propertyDeclaration =
            SyntaxFactory.PropertyDeclaration(
                    variableTypeNode,
                    SyntaxFactory
                        .Identifier(GetPropertyName(variableDeclarator.Identifier.ToString()))
                        .NormalizeWhitespace())
                .AddAccessorListAccessors(
                    SyntaxFactory
                        .AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                        .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                    SyntaxFactory
                        .AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                        .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)))
                .NormalizeWhitespace()
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);


        if (variableDeclaration.ChildNodes().OfType<VariableDeclaratorSyntax>().Count() > 1)
        {
            editor.InsertBefore(fieldDeclarationNode, new[] { propertyDeclaration });
            editor.RemoveNode(variableDeclarator, SyntaxRemoveOptions.KeepNoTrivia);
        }
        else
        {
            editor.ReplaceNode(fieldDeclarationNode, propertyDeclaration);
        }

        var normalizedRoot = editor.GetChangedRoot().NormalizeWhitespace();

        return context.Document.WithSyntaxRoot(normalizedRoot);
    }

    private static async Task<Document> ProcessNotPublicField(
        CodeFixContext context,
        SyntaxNode root,
        Diagnostic diagnostic,
        Document document,
        VariableDeclaratorSyntax variableDeclarator,
        VariableDeclarationSyntax variableDeclaration)
    {
        var editor = await DocumentEditor.CreateAsync(document);

        var variableType = variableDeclaration.Type;

        if (variableDeclaration.Parent is not FieldDeclarationSyntax fieldDeclarationNode)
        {
            return editor.OriginalDocument;
        }

        var getterMethod = GetGetterMethodAndDelete(root, diagnostic, editor);
        var setterMethod = GetSetterMethodAndDelete(root, diagnostic, editor);

        if (getterMethod is null)
        {
            return editor.OriginalDocument;
        }

        var propertyKind = getterMethod.Modifiers.First().Kind();

        var propertyDeclaration =
            SyntaxFactory.PropertyDeclaration(
                    variableType,
                    SyntaxFactory
                        .Identifier(GetPropertyName(variableDeclarator.Identifier.ToString()))
                        .NormalizeWhitespace())
                .AddAccessorListAccessors(
                    SyntaxFactory
                        .AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                        .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)))
                .NormalizeWhitespace()
                .AddModifiers(SyntaxFactory.Token(propertyKind))
                .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);

        if (setterMethod is not null)
        {
            var setterKind = setterMethod.Modifiers.First().Kind();
            var setterDeclaration = SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));

            if (setterKind != propertyKind)
            {
                setterDeclaration = setterDeclaration.AddModifiers(SyntaxFactory.Token(setterKind));
            }

            propertyDeclaration = propertyDeclaration.AddAccessorListAccessors(setterDeclaration);
        }

        if (variableDeclaration.ChildNodes().OfType<VariableDeclaratorSyntax>().Count() > 1)
        {
            editor.InsertBefore(fieldDeclarationNode, new[] { propertyDeclaration });
            editor.RemoveNode(variableDeclarator, SyntaxRemoveOptions.KeepNoTrivia);
        }
        else
        {
            editor.ReplaceNode(fieldDeclarationNode, propertyDeclaration);
        }

        var normalizedRoot = editor.GetChangedRoot().NormalizeWhitespace();

        return context.Document.WithSyntaxRoot(normalizedRoot);
    }

    private static string GetPropertyName(string variableName)
    {
        variableName = variableName.Insert(0, char.ToUpper(variableName[0]).ToString());
        return variableName.Remove(1, 1);
    }

    private static MethodDeclarationSyntax? GetGetterMethodAndDelete(
        SyntaxNode root,
        Diagnostic diagnostic,
        SyntaxEditor editor)
    {
        if (diagnostic.AdditionalLocations.Count < 1)
        {
            return null;
        }

        var getterMethodNode = root.FindNode(diagnostic.AdditionalLocations[0].SourceSpan);

        if (getterMethodNode is not MethodDeclarationSyntax getterMethod)
        {
            return null;
        }

        editor.RemoveNode(getterMethod);
        return getterMethod;
    }

    private static MethodDeclarationSyntax? GetSetterMethodAndDelete(
        SyntaxNode root,
        Diagnostic diagnostic,
        SyntaxEditor editor)
    {
        if (diagnostic.AdditionalLocations.Count < 2)
        {
            return null;
        }

        var setterMethodNode = root.FindNode(diagnostic.AdditionalLocations[1].SourceSpan);

        if (setterMethodNode is not MethodDeclarationSyntax setterMethod)
        {
            return null;
        }

        editor.RemoveNode(setterMethod);
        return setterMethod;
    }
}