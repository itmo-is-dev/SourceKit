using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using SourceKit.Analyzers.MemberAccessibility.Analyzers;

namespace SourceKit.Analyzers.MemberAccessibility.CodeFixes;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(FieldAccessibilityCodeFixProvider))]
public class FieldAccessibilityCodeFixProvider : CodeFixProvider
{
    public const string Title = "Make field private";
    
    public override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(FieldCannotBePublicAnalyzer.DiagnosticId);
    
    public override FixAllProvider GetFixAllProvider()
        => WellKnownFixAllProviders.BatchFixer;

    public override Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        return Task.CompletedTask;
    }
}