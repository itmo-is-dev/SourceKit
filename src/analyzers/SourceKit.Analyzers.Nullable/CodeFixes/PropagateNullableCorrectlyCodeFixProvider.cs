using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using SourceKit.Analyzers.Nullable.Analyzers;

namespace SourceKit.Analyzers.Nullable.CodeFixes;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(PropagateNullableCorrectlyCodeFixProvider))]
public class PropagateNullableCorrectlyCodeFixProvider : CodeFixProvider
{
    public const string Title = "Propagate nullable types correctly";
    
    public override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(NullForgivingOperatorNotAllowedAnalyzer.DiagnosticId);
    
    public override FixAllProvider GetFixAllProvider()
        => WellKnownFixAllProviders.BatchFixer;

    public override Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        return Task.CompletedTask;
    }
}