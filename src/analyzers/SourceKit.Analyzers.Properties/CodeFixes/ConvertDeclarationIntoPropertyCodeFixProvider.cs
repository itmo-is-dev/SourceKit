using System.Collections.Immutable;
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
            createChangedDocument: async token =>
            {
                var newDocument = await ReplaceFieldAsync(root, context, diagnostic, token);
                return newDocument;
            });

        context.RegisterCodeFix(action, diagnostic);
    }

    private static async Task<Document> ReplaceFieldAsync(
        SyntaxNode root,
        CodeFixContext context,
        Diagnostic diagnostic,
        CancellationToken cancellationToken)
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
            ? await ProcessPublicFieldAsync(
                document,
                variableDeclarator,
                variableDeclaration,
                cancellationToken)
            : await ProcessNonPublicFieldAsync(
                context,
                root,
                diagnostic,
                document,
                variableDeclarator,
                variableDeclaration,
                cancellationToken);
    }

    private static async Task<Document> ProcessPublicFieldAsync(
        Document document,
        VariableDeclaratorSyntax variableDeclarator,
        VariableDeclarationSyntax variableDeclaration,
        CancellationToken cancellationToken)
    {
        var editor = await DocumentEditor.CreateAsync(document, cancellationToken);

        var variableTypeNode = variableDeclaration.Type;

        if (variableDeclaration.Parent is not FieldDeclarationSyntax fieldDeclarationNode)
        {
            return editor.OriginalDocument;
        }

        var variableIdentifier = variableDeclarator.Identifier.ToString();
        var variableInitializer = variableDeclarator.Initializer;

        var propertyDeclaration =
            PropertyDeclaration(
                    variableTypeNode,
                    Identifier(NameProducer.GetPropertyName(variableIdentifier)))
                .AddAccessorListAccessors(
                    AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)))
                .AddModifiers(Token(SyntaxKind.PublicKeyword));

        if (fieldDeclarationNode.Modifiers.All(modifier => modifier.Kind() is not SyntaxKind.ReadOnlyKeyword))
        {
            var setMethodDeclaration = AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));

            propertyDeclaration = propertyDeclaration.AddAccessorListAccessors(setMethodDeclaration);
        }

        propertyDeclaration = propertyDeclaration.WithLeadingTrivia(ElasticTab);
        
        if (variableInitializer is not null)
        {
            propertyDeclaration = propertyDeclaration
                .WithInitializer(variableInitializer)
                .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
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

        return editor.GetChangedDocument();
    }

    private static async Task<Document> ProcessNonPublicFieldAsync(
        CodeFixContext context,
        SyntaxNode root,
        Diagnostic diagnostic,
        Document document,
        VariableDeclaratorSyntax variableDeclarator,
        VariableDeclarationSyntax variableDeclaration,
        CancellationToken cancellationToken)
    {
        var editor = await DocumentEditor.CreateAsync(document, cancellationToken);
        var variableType = variableDeclaration.Type;

        if (variableDeclaration.Parent is not FieldDeclarationSyntax fieldDeclarationNode)
        {
            return editor.OriginalDocument;
        }

        var semanticModel = await context.Document.GetSemanticModelAsync(cancellationToken);

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
        var variableIdentifier = variableDeclarator.Identifier.ToString();
        var variableInitializer = variableDeclarator.Initializer;

        var propertyDeclaration = PropertyDeclaration(
                variableType,
                Identifier(NameProducer.GetPropertyName(variableIdentifier)))
            .AddAccessorListAccessors(
                AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)))
            .WithModifiers(propertyAccessor);

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

        propertyDeclaration = propertyDeclaration.WithLeadingTrivia(ElasticTab);

        if (variableInitializer is not null)
        {
            propertyDeclaration = propertyDeclaration
                .WithInitializer(variableInitializer)
                .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
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

        return editor.GetChangedDocument();
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

            if (higherAccessor >= accessibility)
                continue;

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

            if (higherAccessor >= accessibility)
                continue;

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