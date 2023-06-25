using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using SourceKit.Analyzers.Properties.Analyzers;
using SourceKit.Analyzers.Properties.General;
using SourceKit.Extensions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Analyzers.Properties.CodeFixes;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ConvertDeclarationIntoPropertyCodeFixProvider))]
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

        IEnumerable<Task> diagnostics = context.Diagnostics
            .Where(x => x.Id.Equals(DeclarationCouldBeConvertedToPropertyAnalyzer.DiagnosticId))
            .Select(x => ProvideConvertDeclarationIntoPropertyCodeFix(context, x));
        await Task.WhenAll(diagnostics);
    }

    private static async Task ProvideConvertDeclarationIntoPropertyCodeFix(
        CodeFixContext context,
        Diagnostic diagnostic)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);

        if (root is null)
        {
            return;
        }

        var action = CodeAction.Create(
            Title,
            equivalenceKey: nameof(ConvertDeclarationIntoPropertyCodeFixProvider),
            createChangedDocument: async _ =>
            {
                var newDocument = await ReplaceField(root, context, diagnostic);
                return newDocument;
            });

        context.RegisterCodeFix(action, diagnostic);
    }

    private static async Task<Document> ReplaceField(
        SyntaxNode root,
        CodeFixContext context,
        Diagnostic diagnostic)
    {
        var document = context.Document.WithSyntaxRoot(root);

        var variableNode = root.FindNode(diagnostic.AdditionalLocations[0].SourceSpan);
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

        var propertyDeclaration = PropertyDeclaration(
                variableTypeNode,
                Identifier(NameProducer.GetPropertyName(variableDeclarator.Identifier.ToString())))
            .AddAccessorListAccessors(
                AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
                AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                    .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)))
            .NormalizeWhitespace()
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .WithTrailingTrivia(CarriageReturnLineFeed);


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

        var semanticModel = await context.Document.GetSemanticModelAsync();
        if (semanticModel is null)
        {
            return editor.OriginalDocument;
        }

        var classDeclarationNode = root.FindNode(diagnostic.AdditionalLocations[1].SourceSpan);
        if (classDeclarationNode is not ClassDeclarationSyntax classDeclaration)
        {
            return editor.OriginalDocument;
        }

        var fieldWithMethods = Finder.FindFieldWithMethods(semanticModel, variableDeclarator, classDeclaration);


        var getMethod = FindMostAccessibleGetMethod(fieldWithMethods);
        var setMethod = FindMostAccessibleSetMethod(fieldWithMethods);
        DeleteAllMethods(fieldWithMethods, editor);

        if (getMethod is null)
        {
            return editor.OriginalDocument;
        }

        var propertyAccessor = getMethod.Modifiers;
        
        var propertyDeclaration =
            PropertyDeclaration(
                    variableType,
                    Identifier(NameProducer.GetPropertyName(variableDeclarator.Identifier.ToString())))
                .AddAccessorListAccessors(
                    AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)))
                .NormalizeWhitespace()
                .WithModifiers(propertyAccessor)
                .WithTrailingTrivia(CarriageReturnLineFeed);

        var setMethodAccessor = setMethod?.Modifiers;
        
        if (setMethodAccessor is not null)
        {
            var setMethodDeclaration = AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));

            if (setMethodAccessor.Value.ToString() != propertyAccessor.ToString())
            {
                setMethodDeclaration = setMethodDeclaration.WithModifiers(setMethodAccessor.Value);
            }

            propertyDeclaration = propertyDeclaration.AddAccessorListAccessors(setMethodDeclaration);
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

    private static MethodDeclarationSyntax? FindMostAccessibleGetMethod(FieldWithMethods fieldWithMethods)
    {
        if (fieldWithMethods.GetMethods.Count == 0)
        {
            return null;
        }

        var higherAccessor = Accessibility.Private;
        MethodDeclarationSyntax? higherAccessorMethod = null;
        foreach (var getMethod in fieldWithMethods.GetMethods)
        {
            var accessibility = getMethod.Modifiers.ToSyntaxTokenList();
            if (higherAccessor >= accessibility) continue;
            higherAccessor = accessibility;
            higherAccessorMethod = getMethod;
        }

        return higherAccessorMethod;
    }

    private static MethodDeclarationSyntax? FindMostAccessibleSetMethod(FieldWithMethods fieldWithMethods)
    {
        if (fieldWithMethods.SetMethods.Count == 0)
        {
            return null;
        }

        var higherAccessor = Accessibility.Private;
        MethodDeclarationSyntax? higherAccessorMethod = null;
        foreach (var setMethod in fieldWithMethods.SetMethods)
        {
            var accessibility = setMethod.Modifiers.ToSyntaxTokenList();
            if (higherAccessor >= accessibility) continue;
            higherAccessor = accessibility;
            higherAccessorMethod = setMethod;
        }

        return higherAccessorMethod;
    }

    private static void DeleteAllMethods(FieldWithMethods fieldWithMethods, DocumentEditor editor)
    {
        foreach (var getMethod in fieldWithMethods.GetMethods)
        {
            editor.RemoveNode(getMethod);
        }

        foreach (var setMethod in fieldWithMethods.SetMethods)
        {
            editor.RemoveNode(setMethod);
        }
    }
}