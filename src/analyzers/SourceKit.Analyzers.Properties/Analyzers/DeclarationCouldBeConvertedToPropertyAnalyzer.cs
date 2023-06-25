using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using SourceKit.Analyzers.Properties.General;

namespace SourceKit.Analyzers.Properties.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class DeclarationCouldBeConvertedToPropertyAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "SK1400";
    public const string Title = nameof(DeclarationCouldBeConvertedToPropertyAnalyzer);

    public const string Format = """Field '{0}' could be converted to property.""";

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

        context.RegisterSyntaxNodeAction(AnalyzeClass, SyntaxKind.ClassDeclaration);
    }

    private void AnalyzeClass(SyntaxNodeAnalysisContext context)
    {
        var classDeclaration = (ClassDeclarationSyntax)context.Node;

        foreach (var field in classDeclaration.Members.OfType<FieldDeclarationSyntax>())
        {
            if (field.Modifiers.Any(modifier => modifier.Kind() is SyntaxKind.PublicKeyword))
            {
                AnalyzePublicVariableDeclaration(context, field.Declaration);
            }
            else
            {
                AnalyzeFieldsMethods(context, field.Declaration, classDeclaration);
            }
        }
    }

    private void AnalyzePublicVariableDeclaration(
        SyntaxNodeAnalysisContext context,
        VariableDeclarationSyntax variableDeclaration)
    {
        foreach (var variable in variableDeclaration.Variables)
        {
            var location = variable.GetLocation();
            var diagnostic = Diagnostic.Create(Descriptor, location, new[] { location }, variable.Identifier.Text);
            context.ReportDiagnostic(diagnostic);
        }
    }

    private void AnalyzeFieldsMethods(
        SyntaxNodeAnalysisContext context,
        VariableDeclarationSyntax variableDeclaration,
        ClassDeclarationSyntax classDeclaration)
    {
        var semanticModel = context.SemanticModel;

        var getMethods = FindGetMethods(semanticModel, classDeclaration);
        var setMethods = FindSetMethods(semanticModel, classDeclaration);

        foreach (var variable in variableDeclaration.Variables)
        {
            var fieldWithMethods = Finder.FindFieldWithMethods(context, variable, getMethods, setMethods);

            if (fieldWithMethods.GetMethods.Count == 0)
            {
                return;
            }
            
            var variableLocation = variable.Identifier.GetLocation();
            var additionalLocations = new List<Location> { variableLocation };
            
            context.ReportDiagnostic(Diagnostic.Create(
                Descriptor,
                variableLocation,
                additionalLocations,
                variable.Identifier.Text));
            
            foreach (var getMethod in fieldWithMethods.GetMethods)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    Descriptor,
                    getMethod.Identifier.GetLocation(),
                    additionalLocations,
                    variable.Identifier.Text));
            }

            foreach (var setMethod in fieldWithMethods.SetMethods)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    Descriptor,
                    setMethod.Identifier.GetLocation(),
                    additionalLocations,
                    variable.Identifier.Text));
            }
        }
    }

    private ILookup<ISymbol?, MethodDeclarationSyntax> FindGetMethods(
        SemanticModel semanticModel,
        ClassDeclarationSyntax classDeclaration)
    {
        return classDeclaration
            .ChildNodes()
            .OfType<MethodDeclarationSyntax>()
            .Where(method =>
                !method.ParameterList.Parameters.Any() &&
                method.Body?.Statements.Count == 1 &&
                method.Body.Statements.First() is ReturnStatementSyntax returnStatementSyntax &&
                semanticModel.GetOperation(returnStatementSyntax) is IReturnOperation
                {
                    ReturnedValue: IFieldReferenceOperation
                })
            .ToLookup(method =>
                {
                    var returnSyntax = method.Body!.ChildNodes().First();
                    var returnSymbol = (IReturnOperation) semanticModel.GetOperation(returnSyntax)!;
                    var fieldReferenceSymbol = (IFieldReferenceOperation) returnSymbol.ReturnedValue!;

                    return fieldReferenceSymbol.Field;
                },
                SymbolEqualityComparer.Default);
    }

    private ILookup<ISymbol?, MethodDeclarationSyntax> FindSetMethods(
        SemanticModel semanticModel,
        ClassDeclarationSyntax classDeclaration)
    {
        return classDeclaration
            .ChildNodes()
            .OfType<MethodDeclarationSyntax>()
            .Where(method =>
                method.ParameterList.Parameters.Count == 1 &&
                method.Body?.Statements.Count == 1 &&
                method.Body.Statements.First() is ExpressionStatementSyntax expressionStatement &&
                semanticModel.GetOperation(expressionStatement) is IExpressionStatementOperation
                {
                    Operation: ISimpleAssignmentOperation
                    {
                        Value: IParameterReferenceOperation valueReferenceOperation
                    }
                } &&
                SymbolEqualityComparer.Default.Equals(
                    semanticModel.GetDeclaredSymbol(method.ParameterList.Parameters.First()),
                    valueReferenceOperation.Parameter))
            .ToLookup(method =>
                {
                    var expressionStatement = method.Body!.ChildNodes().First();
                    var expressionStatementOperation =
                        (IExpressionStatementOperation) semanticModel.GetOperation(expressionStatement)!;
                    var simpleAssignmentOperation = (ISimpleAssignmentOperation) expressionStatementOperation.Operation;
                    var fieldReferenceOperation = (IFieldReferenceOperation) simpleAssignmentOperation.Target;
                    return fieldReferenceOperation.Field;
                },
                SymbolEqualityComparer.Default);
    }
}