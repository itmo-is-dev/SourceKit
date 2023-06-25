using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SourceKit.Analyzers.Collections.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class ListForEachNotAllowedAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "SK1501";
    public const string Title = nameof(DictionaryKeyTypeMustImplementEquatableAnalyzer);

    public const string Format = """Using ForEach method is not allowed""";

    public static readonly DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(
        DiagnosticId,
        Title,
        Format,
        "Design",
        DiagnosticSeverity.Warning,
        true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(Descriptor);

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze |
                                               GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.SimpleMemberAccessExpression);
    }

    private static void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context)
    {
        var node = (MemberAccessExpressionSyntax) context.Node;

        if (node.Expression is not IdentifierNameSyntax identifierNameSyntax)
        {
            return;
        }

        if (node.Name is not IdentifierNameSyntax expressionName)
        {
            return;
        }

        var semanticModel = context.SemanticModel;

        var invocationTargetTypeSymbol = semanticModel
            .GetTypeInfo(identifierNameSyntax)
            .ConvertedType;

        if (invocationTargetTypeSymbol is null)
        {
            return;
        }

        if (invocationTargetTypeSymbol
                .ToString()
                .Contains("System.Collections.Generic.List") &&
            expressionName.ToString() == "ForEach")
        {
            context.ReportDiagnostic(
                Diagnostic.Create(
                    Descriptor,
                    context.Node.GetLocation()));
        }
    }
}