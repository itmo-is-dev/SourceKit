using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;

namespace SourceKit.Analyzers.Members;

[Generator]
public sealed class FieldCouldBeConverterToPropertyAnalyzer : IIncrementalGenerator
{
    public const string DiagnosticId = "SK1400";
    public const string Format = """Field '{0}' could be converted to property""";

    public static readonly DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(
        DiagnosticId,
        "Field could be converted to property",
        Format,
        "Design",
        DiagnosticSeverity.Error,
        true);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.SyntaxProvider
            .CreateSyntaxProvider(
                static (node, _) => node is MethodDeclarationSyntax { Parent: TypeDeclarationSyntax },
                static (context, _) =>
                {
                    var node = (MethodDeclarationSyntax)context.Node;
                    var parent = (TypeDeclarationSyntax)node.Parent!;

                    GetterSetterMethodCandidate candidate;

                    if (TryGetGetterMethod(node, out candidate))
                        return IncrementalResult.Success((candidate, parent));

                    if (TryGetSetterMethod(node, out candidate))
                        return IncrementalResult.Success((candidate, parent));

                    return IncrementalResult.Skip;
                })
            .Unwrap(context)
            .Select(static (candidate, parent) => IncrementalResult.Failure([..EnumerateDiagnostics(candidate, parent)]))
            .Unwrap(context);
    }

    private readonly record struct GetterSetterMethodCandidate(
        MethodDeclarationSyntax MethodDeclaration,
        SyntaxToken FieldIdentifier);

    private static bool TryGetGetterMethod(
        MethodDeclarationSyntax methodDeclaration,
        out GetterSetterMethodCandidate candidate)
    {
        candidate = default;

        if (methodDeclaration.Body is not null)
        {
            if (methodDeclaration.Body.Statements is not [ReturnStatementSyntax returnStatement])
                return false;

            if (returnStatement.Expression is not IdentifierNameSyntax identifierName)
                return false;

            candidate = new GetterSetterMethodCandidate(methodDeclaration, identifierName.Identifier);
            return true;
        }

        if (methodDeclaration.ExpressionBody is not null)
        {
            if (methodDeclaration.ExpressionBody.Expression is not IdentifierNameSyntax identifierName)
                return false;

            candidate = new GetterSetterMethodCandidate(methodDeclaration, identifierName.Identifier);
            return true;
        }

        return false;
    }

    private static bool TryGetSetterMethod(
        MethodDeclarationSyntax methodDeclaration,
        out GetterSetterMethodCandidate candidate)
    {
        candidate = default;

        if (methodDeclaration.Body is not null)
        {
            if (methodDeclaration.Body.Statements is not [ExpressionStatementSyntax expression])
                return false;

            if (expression.Expression is not AssignmentExpressionSyntax { Left: IdentifierNameSyntax target, Right: IdentifierNameSyntax })
                return false;

            candidate = new GetterSetterMethodCandidate(methodDeclaration, target.Identifier);
            return true;
        }

        if (methodDeclaration.ExpressionBody is not null)
        {
            if (methodDeclaration.ExpressionBody.Expression is not AssignmentExpressionSyntax { Left: IdentifierNameSyntax target, Right: IdentifierNameSyntax })
                return false;

            candidate = new GetterSetterMethodCandidate(methodDeclaration, target.Identifier);
            return true;
        }

        return false;
    }

    private static IEnumerable<Diagnostic> EnumerateDiagnostics(
        GetterSetterMethodCandidate candidate,
        TypeDeclarationSyntax typeDeclaration)
    {
        IEnumerable<VariableDeclaratorSyntax> fields = typeDeclaration.Members
            .OfType<FieldDeclarationSyntax>()
            .SelectMany(field => field.Declaration.Variables);

        foreach (VariableDeclaratorSyntax variable in fields)
        {
            if (variable.Identifier.Text.Equals(candidate.FieldIdentifier.Text, StringComparison.Ordinal) is false)
                continue;

            yield return Diagnostic.Create(
                descriptor: Descriptor,
                location: candidate.MethodDeclaration.GetSignatureLocation(),
                messageArgs: [candidate.FieldIdentifier],
                additionalLocations: [variable.GetLocation()]);
        }
    }
}
