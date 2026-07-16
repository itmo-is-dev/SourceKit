using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SourceKit.Analyzers.DependencyInjection;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class GetRequiredServiceExplicitTypeSuppressor : DiagnosticSuppressor
{
    private static readonly SuppressionDescriptor Descriptor = new(
        id: "SKSP1000",
        suppressedDiagnosticId: "IDE0008",
        justification: "Method invocation has return value type in it's generic argument");

    public override ImmutableArray<SuppressionDescriptor> SupportedSuppressions { get; } = [Descriptor];

    public override void ReportSuppressions(SuppressionAnalysisContext context)
    {
        INamedTypeSymbol? serviceProviderType = context.Compilation.GetTypeByMetadataName("System.IServiceProvider");

        if (serviceProviderType is null)
            return;

        foreach (Diagnostic diagnostic in context.ReportedDiagnostics)
        {
            SyntaxTree? syntaxTree = diagnostic.Location.SourceTree;
            SyntaxNode? node = syntaxTree?.GetRoot().FindNode(diagnostic.Location.SourceSpan);

            if (node is not IdentifierNameSyntax identifier)
                continue;

            if (identifier.Identifier.ValueText != "var")
                continue;

            if (identifier.Parent is not VariableDeclarationSyntax variableDeclaration)
                continue;

            if (variableDeclaration.Variables is not [var variableDeclarator])
                continue;

            if (variableDeclarator.Initializer?.Value is not InvocationExpressionSyntax invocation)
                continue;

            if (invocation.Expression is not MemberAccessExpressionSyntax methodMemberAccess)
                continue;

            if (methodMemberAccess.Name is not GenericNameSyntax methodGenericName)
                continue;

            if (methodGenericName.TypeArgumentList.Arguments.Count is not 1)
                continue;

            if (methodGenericName.Identifier.Text is not "GetRequiredService" and not "GetService")
                continue;

            SemanticModel semanticModel = context.GetSemanticModel(syntaxTree!);
            IOperation? targetOperation = semanticModel.GetOperation(methodMemberAccess.Expression);

            if (targetOperation?.Type?.Equals(serviceProviderType, SymbolEqualityComparer.Default) is not true)
                continue;

            context.ReportSuppression(Suppression.Create(Descriptor, diagnostic));
        }
    }
}
