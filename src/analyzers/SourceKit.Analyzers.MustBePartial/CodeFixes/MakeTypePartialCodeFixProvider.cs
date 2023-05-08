using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Analyzers.MustBePartial.Analyzers;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Analyzers.MustBePartial.CodeFixes;

[Shared]
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(MakeTypePartialCodeFixProvider))]
public class MakeTypePartialCodeFixProvider : CodeFixProvider
{
    public const string Title = "Make type partial";

    public override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(DerivativesMustBePartialAnalyzer.DiagnosticId);

    public override FixAllProvider GetFixAllProvider()
        => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        context.CancellationToken.ThrowIfCancellationRequested();

        IEnumerable<Task> derivativesMustBePartialDiagnostics = context.Diagnostics
            .Where(x => x.Id.Equals(DerivativesMustBePartialAnalyzer.DiagnosticId))
            .Select(x => ProvideDerivativesMustBePartial(context, x));

        await Task.WhenAll(derivativesMustBePartialDiagnostics);
    }

    private static async Task ProvideDerivativesMustBePartial(CodeFixContext context, Diagnostic diagnostic)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);

        if (root?.FindNode(context.Span) is not TypeDeclarationSyntax syntax)
            return;

        var action = CodeAction.Create(
            Title,
            equivalenceKey: nameof(Title),
            createChangedDocument: _ =>
            {
                var newSyntax = syntax.AddModifiers(Token(SyntaxKind.PartialKeyword));
                var newRoot = root.ReplaceNode(syntax, newSyntax);

                var document = context.Document.WithSyntaxRoot(newRoot);

                return Task.FromResult(document);
            });

        context.RegisterCodeFix(action, diagnostic);
    }
}