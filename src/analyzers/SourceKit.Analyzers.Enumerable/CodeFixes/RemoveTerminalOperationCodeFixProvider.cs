using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using SourceKit.Analyzers.Enumerable.Analyzers;

namespace SourceKit.Analyzers.Enumerable.CodeFixes;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(RemoveTerminalOperationCodeFixProvider))]
public class RemoveTerminalOperationCodeFixProvider : CodeFixProvider
{
    public const string Title = "Remove {0}";
    
    public override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(CannotLinqChainAfterTerminalOperationAnalyzer.DiagnosticId);
    
    public override FixAllProvider GetFixAllProvider()
        => WellKnownFixAllProviders.BatchFixer;

    public override Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        return Task.CompletedTask;
    }
}