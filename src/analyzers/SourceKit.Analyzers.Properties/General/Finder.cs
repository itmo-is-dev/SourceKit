using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

namespace SourceKit.Analyzers.Properties.General;

public static class Finder
{
    public static FieldWithMethods FindFieldWithMethods(
        SemanticModel semanticModel,
        VariableDeclaratorSyntax variableDeclarator,
        ClassDeclarationSyntax classDeclaration)
    {
        var fieldWithMethods = new FieldWithMethods(variableDeclarator);

        var fieldSymbol = semanticModel.GetDeclaredSymbol(variableDeclarator);
        if (fieldSymbol is null)
        {
            return fieldWithMethods;
        }

        var getMethods = FindGetMethods(semanticModel, classDeclaration);
        var setMethods = FindSetMethods(semanticModel, classDeclaration);
        
        if (getMethods.Contains(fieldSymbol) is false)
        {
            return fieldWithMethods;
        }

        fieldWithMethods.GetMethods = getMethods[fieldSymbol].ToList();
        fieldWithMethods.SetMethods = setMethods[fieldSymbol].ToList();

        return fieldWithMethods;
    }
    
    private static ILookup<ISymbol?, MethodDeclarationSyntax> FindGetMethods(
        SemanticModel semanticModel,
        SyntaxNode classDeclaration)
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

    private static ILookup<ISymbol?, MethodDeclarationSyntax> FindSetMethods(
        SemanticModel semanticModel,
        SyntaxNode classDeclaration)
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