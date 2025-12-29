using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Analyzers.MemberAccessibility.Analyzers;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Analyzers.MemberAccessibility.CodeFixes;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(PropertyCannotBePrivateCodeFixProvider))]
public class PropertyCannotBePrivateCodeFixProvider : CodeFixProvider
{
    public const string Title = "Make property public";

    public override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(PropertyCannotBePrivateAnalyzer.DiagnosticId);

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

        if (root?.FindNode(diagnostic.Location.SourceSpan) is not PropertyDeclarationSyntax propertySyntax)
            return;

        var action = CodeAction.Create(
            title: Title,
            priority: CodeActionPriority.High,
            equivalenceKey: nameof(PropertyCannotBePrivateCodeFixProvider),
            createChangedDocument: _ =>
            {
                SyntaxToken privateModifier = propertySyntax.Modifiers.First(x => x.IsKind(SyntaxKind.PrivateKeyword));

                SyntaxTokenList fixedModifiers = propertySyntax.Modifiers
                    .Replace(privateModifier, Token(SyntaxKind.PublicKeyword));

                PropertyDeclarationSyntax fixedSyntax = propertySyntax.WithModifiers(fixedModifiers);

                SyntaxNode newRoot = root.ReplaceNode(propertySyntax, fixedSyntax);

                Document document = context.Document.WithSyntaxRoot(newRoot);

                return Task.FromResult(document);
            });

        context.RegisterCodeFix(action, diagnostic);
    }
}
