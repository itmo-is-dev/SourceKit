using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Analyzers.Collections.Analyzers;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Analyzers.Collections.CodeFixes;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ConvertListForEachIntoForEachLoopCodeFixProvider))]
public class ConvertListForEachIntoForEachLoopCodeFixProvider : CodeFixProvider
{
    public const string Title = "Convert into foreach loop";

    public override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(ListForEachNotAllowedAnalyzer.DiagnosticId);

    public override FixAllProvider GetFixAllProvider()
        => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document
            .GetSyntaxRootAsync(context.CancellationToken)
            .ConfigureAwait(false);

        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        var invocationExpression = root?
            .FindToken(diagnosticSpan.Start)
            .Parent?.AncestorsAndSelf()
            .OfType<InvocationExpressionSyntax>()
            .First();

        if (invocationExpression is null)
        {
            return;
        }

        context.RegisterCodeFix(
            CodeAction.Create(
                equivalenceKey: ListForEachNotAllowedAnalyzer.DiagnosticId,
                title: Title,
                createChangedDocument: cancellationToken =>
                    ConvertListForEachIntoLoop(context.Document, invocationExpression, cancellationToken)),
            diagnostic);
    }

    private async Task<Document> ConvertListForEachIntoLoop(Document document,
                                                            InvocationExpressionSyntax invocationExpressionSyntax,
                                                            CancellationToken cancellationToken)
    {
        var parentOfFoundInvocationExpression = invocationExpressionSyntax.Parent;

        if (parentOfFoundInvocationExpression is null)
        {
            return document;
        }

        var root = await document.GetSyntaxRootAsync(cancellationToken);

        if (root is null)
        {
            return document;
        }

        var argumentOfInvocationExpression = invocationExpressionSyntax.ArgumentList.Arguments[0]
            .Expression;

        var foreachType =
            IdentifierName(
                Identifier(
                    TriviaList(),
                    SyntaxKind.VarKeyword,
                    "var",
                    "var",
                    TriviaList()));
        var foreachIdentifier = IdentifierName("element");
        var foreachExpression = ((MemberAccessExpressionSyntax) invocationExpressionSyntax.Expression).Expression;
        var foreachStatements = new List<StatementSyntax>();

        switch (argumentOfInvocationExpression)
        {
            case SimpleLambdaExpressionSyntax lambdaExpressionSyntax:
                if (lambdaExpressionSyntax.ExpressionBody is not null)
                {
                    foreachIdentifier = IdentifierName(lambdaExpressionSyntax.Parameter.Identifier);
                    foreachStatements.Add(ExpressionStatement(lambdaExpressionSyntax.ExpressionBody));
                }
                else if (lambdaExpressionSyntax.Block is not null)
                {
                    var statements = lambdaExpressionSyntax.Block.Statements;
                    foreachStatements.AddRange(statements);
                }
                else
                {
                    return document;
                }

                break;
            case IdentifierNameSyntax identifierNameSyntax:
                foreachStatements.Add(CreateForeachStatement(identifierNameSyntax, foreachIdentifier));
                break;
            case MemberAccessExpressionSyntax expressionSyntax:
                foreachStatements.Add(CreateForeachStatement(expressionSyntax, foreachIdentifier));
                break;
        }

        var forEachStatement = ForEachStatement(
                foreachType,
                foreachIdentifier.Identifier,
                foreachExpression,
                Block(foreachStatements))
            .WithLeadingTrivia(LineFeed);

        var newRoot = root.ReplaceNode(parentOfFoundInvocationExpression, forEachStatement);

        return document.WithSyntaxRoot(newRoot);
    }

    private ExpressionStatementSyntax CreateForeachStatement(ExpressionSyntax expressionSyntax,
                                                             IdentifierNameSyntax identifierNameSyntax)
    {
        return ExpressionStatement(
            InvocationExpression(
                    expressionSyntax)
                .WithArgumentList(
                    ArgumentList(
                        SingletonSeparatedList(
                            Argument(
                                identifierNameSyntax)))));
    }
}