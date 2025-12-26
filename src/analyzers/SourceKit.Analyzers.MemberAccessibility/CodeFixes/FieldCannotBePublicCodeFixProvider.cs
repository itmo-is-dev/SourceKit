using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Analyzers.MemberAccessibility.Analyzers;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Analyzers.MemberAccessibility.CodeFixes;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(FieldCannotBePublicCodeFixProvider))]
public class FieldCannotBePublicCodeFixProvider : CodeFixProvider
{
    public const string Title = "Make field private";

    public override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(FieldCannotBePublicAnalyzer.DiagnosticId);

    public override FixAllProvider GetFixAllProvider()
        => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        context.CancellationToken.ThrowIfCancellationRequested();

        IEnumerable<Task> derivativesMustBePartialDiagnostics = context.Diagnostics
            .Select(x => ProvideDerivativesMustBePartial(context, x));

        await Task.WhenAll(derivativesMustBePartialDiagnostics);
    }

    private static async Task ProvideDerivativesMustBePartial(CodeFixContext context, Diagnostic diagnostic)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);

        if (root?.FindNode(diagnostic.Location.SourceSpan) is not { Parent.Parent: FieldDeclarationSyntax fieldSyntax })
            return;

        var action = CodeAction.Create(Title,
            equivalenceKey: nameof(FieldCannotBePublicCodeFixProvider),
            createChangedDocument: _ =>
            {
                SyntaxToken privateModifier = fieldSyntax.Modifiers.First(x => x.Kind() is SyntaxKind.PublicKeyword);

                SyntaxTokenList fixedModifiers = fieldSyntax.Modifiers.Replace(privateModifier, Token(SyntaxKind.PrivateKeyword));
                FieldDeclarationSyntax fixedSyntax = fieldSyntax.WithModifiers(fixedModifiers);

                SyntaxNode newRoot = root.ReplaceNode(fieldSyntax, fixedSyntax);

                Document document = context.Document.WithSyntaxRoot(newRoot);

                return Task.FromResult(document);
            });

        context.RegisterCodeFix(action, diagnostic);
    }
}
