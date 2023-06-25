using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using SourceKit.Analyzers.Properties.Analyzers;
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
                Identifier(GetPropertyName(variableDeclarator.Identifier.ToString())))
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

        var getterMethod = FindGetterMethodAndDelete(root, diagnostic, editor);
        var setterKind = FindSetterMethodAndDelete(root, diagnostic, editor);

        if (getterMethod is null)
        {
            return editor.OriginalDocument;
        }

        var propertyKind = getterMethod.Modifiers.First().Kind();

        var propertyDeclaration =
            PropertyDeclaration(
                    variableType,
                    Identifier(GetPropertyName(variableDeclarator.Identifier.ToString())))
                .AddAccessorListAccessors(
                    AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)))
                .NormalizeWhitespace()
                .AddModifiers(Token(propertyKind))
                .WithTrailingTrivia(CarriageReturnLineFeed);

        if (setterKind is not null)
        {
            var setterDeclaration = AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));

            if (setterKind != propertyKind)
            {
                setterDeclaration = setterDeclaration.AddModifiers(Token(setterKind.Value));
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
        var variableNameBuilder = new StringBuilder(variableName);

        if (variableNameBuilder[0] == '_' && variableNameBuilder.Length > 1)
        {
            variableNameBuilder.Remove(0, 1);
        }

        variableNameBuilder.Insert(0, char.ToUpper(variableNameBuilder[0]));
        variableNameBuilder.Remove(1, 1);
        return variableNameBuilder.ToString();
    }

    private static MethodDeclarationSyntax? FindGetterMethodAndDelete(
        SyntaxNode root,
        Diagnostic diagnostic,
        SyntaxEditor editor)
    {
        if (diagnostic.AdditionalLocations.Count < 2)
        {
            return null;
        }

        var getterMethodNode = root.FindNode(diagnostic.AdditionalLocations[1].SourceSpan);

        if (getterMethodNode is not MethodDeclarationSyntax getterMethod)
        {
            return null;
        }

        editor.RemoveNode(getterMethod);
        return getterMethod;
    }

    private static SyntaxKind? FindSetterMethodAndDelete(
        SyntaxNode root,
        Diagnostic diagnostic,
        SyntaxEditor editor)
    {
        var numberOfGetMethods = Convert.ToInt32(diagnostic.Properties["GetMethodsAmount"]);
        var numberOfSetMethods = Convert.ToInt32(diagnostic.Properties["SetMethodsAmount"]);

        if (numberOfSetMethods == 0)
        {
            return null;
        }

        var higherSyntaxKind = Accessibility.Private;
        for (var i = numberOfGetMethods + 1; i < diagnostic.AdditionalLocations.Count; i++)
        {
            var setterMethodNode = root.FindNode(diagnostic.AdditionalLocations[i].SourceSpan);
            if (setterMethodNode is not MethodDeclarationSyntax setterMethod)
            {
                return null;
            }

            if (Enum.TryParse<Accessibility>(setterMethod.Modifiers.First().Kind().ToString(), out var kind))
            {
                if (higherSyntaxKind < kind)
                {
                    higherSyntaxKind = kind;
                }
            }
            
            editor.RemoveNode(setterMethod);
        }
        
        var t = higherSyntaxKind.ToSyntaxTokenList();
        return null;
    }
}