using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Analyzers.MemberAccessibility.Analyzers;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Analyzers.MemberAccessibility.CodeFixes;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(PropertyCannotBePrivateCodeFixProvider))]
public class FieldCannotHaveMultipleVariablesCodeFixProvider : CodeFixProvider
{
    public const string Title = "Separate field declarations";

    public override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(FieldCannotHaveMultipleVariablesAnalyzer.DiagnosticId);

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

        if (root?.FindNode(diagnostic.Location.SourceSpan) is not FieldDeclarationSyntax fieldSyntax)
            return;

        var action = CodeAction.Create(
            title: Title,
            priority: CodeActionPriority.High,
            equivalenceKey: nameof(FieldCannotHaveMultipleVariablesCodeFixProvider),
            createChangedDocument: _ =>
            {
                IEnumerable<FieldDeclarationSyntax> fixedFields = fieldSyntax.Declaration.Variables.Select(x =>
                {
                    VariableDeclarationSyntax declaration = fieldSyntax.Declaration
                        .WithVariables(SingletonSeparatedList(x));

                    return fieldSyntax.WithDeclaration(declaration);
                });

                SyntaxNode newRoot = root.ReplaceNode(fieldSyntax, fixedFields);

                Document document = context.Document.WithSyntaxRoot(newRoot);

                return Task.FromResult(document);
            });

        context.RegisterCodeFix(action, diagnostic);
    }
}
