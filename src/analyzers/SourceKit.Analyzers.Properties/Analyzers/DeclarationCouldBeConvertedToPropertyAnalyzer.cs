using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SourceKit.Analyzers.Properties.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class DeclarationCouldBeConvertedToPropertyAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "SK1400";
    public const string Title = nameof(DeclarationCouldBeConvertedToPropertyAnalyzer);

    public const string Format = """Variable {0} could be converted to property.""";

    public static readonly DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(
        DiagnosticId,
        Title,
        Format,
        "Design",
        DiagnosticSeverity.Error,
        true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(Descriptor);

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze |
                                               GeneratedCodeAnalysisFlags.ReportDiagnostics);

        context.RegisterSyntaxNodeAction(AnalyzeClass, SyntaxKind.ClassDeclaration);
    }

    private void AnalyzeClass(SyntaxNodeAnalysisContext context)
    {
        var classDeclaration = (ClassDeclarationSyntax) context.Node;

        var fields = classDeclaration.Members.OfType<FieldDeclarationSyntax>().ToList();
        fields.ForEach(field =>
        {
            if (field.Modifiers.Any(modifier => modifier.Kind() is SyntaxKind.PublicKeyword))
            {
                AnalyzePublicVariableDeclaration(context, field.Declaration);
            }

            if (field.Modifiers.Any(modifier => modifier.Kind() is SyntaxKind.PrivateKeyword))
            {
                FindMethods(context, field.Declaration, classDeclaration);
            }
        });
    }

    private void AnalyzePublicVariableDeclaration(
        SyntaxNodeAnalysisContext context,
        VariableDeclarationSyntax variableDeclaration)
    {
        foreach (var variable in variableDeclaration.Variables)
        {
            var location = variable.GetLocation();
            var diagnostic = Diagnostic.Create(Descriptor, location, variable.Identifier.Text);
            context.ReportDiagnostic(diagnostic);
        }
    }

    private void FindMethods(
        SyntaxNodeAnalysisContext context,
        VariableDeclarationSyntax variableDeclaration,
        ClassDeclarationSyntax classDeclaration)
    {
        var getMethods = classDeclaration
            .ChildNodes()
            .OfType<MethodDeclarationSyntax>()
            .Where(method =>
                !method.ParameterList.ChildNodes().Any() &&
                method.Body?.ChildNodes().Count() == 1 &&
                method.Body.ChildNodes().First() is ReturnStatementSyntax)
            .ToDictionary(method =>
            {
                var returnStatement = (ReturnStatementSyntax) method.Body?.ChildNodes().First()!;
                return returnStatement.ChildNodes().First().ToString();
            });

        var setMethods = classDeclaration
            .ChildNodes()
            .OfType<MethodDeclarationSyntax>()
            .Where(method =>
                method.ParameterList.ChildNodes().Count() == 1 &&
                method.ParameterList.ChildNodes().First() is ParameterSyntax parameterSyntax &&
                method.Body?.ChildNodes().Count() == 1 &&
                method.Body.ChildNodes().First() is ExpressionStatementSyntax expressionStatement &&
                expressionStatement.Expression.ChildNodes().Count() == 2 &&
                expressionStatement.Expression.ChildNodes().Last().ToString() == parameterSyntax.Identifier.ToString())
            .ToDictionary(method =>
            {
                var expressionStatement = (ExpressionStatementSyntax) method.Body?.ChildNodes().First()!;
                return expressionStatement.Expression.ChildNodes().First().ToString();
            });

        foreach (var variable in variableDeclaration.Variables)
        {
            MethodDeclarationSyntax getMethod;
            var isGetMethod = getMethods.TryGetValue(variable.Identifier.ToString(), out getMethod);
            if (!isGetMethod || getMethod is null)
            {
                return;
            }

            var location = variable.GetLocation();
            var getMethodLocation = getMethod.GetLocation();


            var diagnostic = Diagnostic.Create(Descriptor, location, variable.Identifier.Text);
            context.ReportDiagnostic(diagnostic);
        }
    }
}