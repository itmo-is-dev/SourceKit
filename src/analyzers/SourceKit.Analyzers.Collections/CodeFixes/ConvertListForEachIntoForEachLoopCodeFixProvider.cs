using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using SourceKit.Analyzers.Collections.Analyzers;

namespace SourceKit.Analyzers.Collections.CodeFixes;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ConvertListForEachIntoForEachLoopCodeFixProvider))]
public class ConvertListForEachIntoForEachLoopCodeFixProvider : CodeFixProvider
{
    public const string Title = "Convert into foreach loop";
    
    public override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(ListForEachNotAllowedAnalyzer.DiagnosticId);
    
    public override FixAllProvider GetFixAllProvider()
        => WellKnownFixAllProviders.BatchFixer;

    public override Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        return Task.CompletedTask;
    }
}