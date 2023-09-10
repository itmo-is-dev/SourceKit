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
        var methods = classDeclaration
            .ChildNodes()
            .OfType<MethodDeclarationSyntax>();

        var getMethods = new List<KeyValuePair<ISymbol?, MethodDeclarationSyntax>>();

        foreach (var method in methods)
        {
            if (method.ParameterList.Parameters.Count != 0)
                continue;

            var expressionBody = FindGetMethodInExpressionBody(semanticModel, method);

            if (expressionBody is not null)
            {
                getMethods.Add(expressionBody.Value);
                continue;
            }

            var body = FindGetMethodInBody(semanticModel, method);

            if (body is not null)
            {
                getMethods.Add(body.Value);
            }
        }

        return getMethods.ToLookup(pair => pair.Key, pair => pair.Value, SymbolEqualityComparer.Default);
    }

    private static ILookup<ISymbol?, MethodDeclarationSyntax> FindSetMethods(
        SemanticModel semanticModel,
        SyntaxNode classDeclaration)
    {
        var methods = classDeclaration
            .ChildNodes()
            .OfType<MethodDeclarationSyntax>();

        var setMethods = new List<KeyValuePair<ISymbol?, MethodDeclarationSyntax>>();

        foreach (var method in methods)
        {
            if (method.ParameterList.Parameters.Count != 1)
                continue;

            var expressionBody = FindSetMethodInExpressionBody(semanticModel, method);

            if (expressionBody is not null)
            {
                setMethods.Add(expressionBody.Value);
                continue;
            }

            var body = FindSetMethodInBody(semanticModel, method);

            if (body is not null)
            {
                setMethods.Add(body.Value);
            }
        }

        return setMethods.ToLookup(pair => pair.Key, pair => pair.Value, SymbolEqualityComparer.Default);
    }

    private static KeyValuePair<ISymbol?, MethodDeclarationSyntax>? FindGetMethodInExpressionBody(
        SemanticModel semanticModel,
        MethodDeclarationSyntax method)
    {
        if (method.ExpressionBody is null) return null;

        var expressionBodyOperation = semanticModel.GetOperation(method.ExpressionBody);
        if (expressionBodyOperation is not IBlockOperation blockOperation) return null;

        if (blockOperation.Operations.Count() != 1) return null;
        if (blockOperation.Operations.First() is not IReturnOperation returnOperation ||
            returnOperation.ReturnedValue is not IFieldReferenceOperation fieldReferenceOperation)
        {
            return null;
        }

        return new KeyValuePair<ISymbol?, MethodDeclarationSyntax>(fieldReferenceOperation.Field, method);
    }

    private static KeyValuePair<ISymbol?, MethodDeclarationSyntax>? FindGetMethodInBody(
        SemanticModel semanticModel,
        MethodDeclarationSyntax method)
    {
        if (method.Body is null) return null;
        if (method.Body?.Statements.Count != 1) return null;
        if (method.Body.Statements.First() is not ReturnStatementSyntax returnStatementSyntax) return null;

        if (semanticModel.GetOperation(returnStatementSyntax) is not IReturnOperation returnOperation ||
            returnOperation.ReturnedValue is not IFieldReferenceOperation fieldReferenceOperation)
        {
            return null;
        }

        return new KeyValuePair<ISymbol?, MethodDeclarationSyntax>(fieldReferenceOperation.Field, method);
    }

    private static KeyValuePair<ISymbol?, MethodDeclarationSyntax>? FindSetMethodInExpressionBody(
        SemanticModel semanticModel,
        MethodDeclarationSyntax method)
    {
        if (method.ExpressionBody is null) return null;

        var expressionBodyOperation = semanticModel.GetOperation(method.ExpressionBody);
        if (expressionBodyOperation is not IBlockOperation blockOperation) return null;

        if (blockOperation.Operations.Count() != 1) return null;
        if (blockOperation.Operations.First() is not IExpressionStatementOperation expressionStatementOperation ||
            expressionStatementOperation.Operation is not ISimpleAssignmentOperation simpleAssignmentOperation ||
            simpleAssignmentOperation.Value is not IParameterReferenceOperation parameterReferenceOperation)
        {
            return null;
        }

        if (!SymbolEqualityComparer.Default.Equals(
                semanticModel.GetDeclaredSymbol(method.ParameterList.Parameters.First()),
                parameterReferenceOperation.Parameter))
        {
            return null;
        }

        var field = simpleAssignmentOperation.Target;
        if (field is not IFieldReferenceOperation fieldReferenceOperation) return null;

        return new KeyValuePair<ISymbol?, MethodDeclarationSyntax>(fieldReferenceOperation.Field, method);
    }

    private static KeyValuePair<ISymbol?, MethodDeclarationSyntax>? FindSetMethodInBody(
        SemanticModel semanticModel,
        MethodDeclarationSyntax method)
    {
        if (method.Body is null) return null;
        if (method.Body?.Statements.Count != 1) return null;

        if (method.Body.Statements.First() is not ExpressionStatementSyntax expressionStatement) return null;

        var expressionStatementModel = semanticModel.GetOperation(expressionStatement);
        if (expressionStatementModel is not IExpressionStatementOperation expressionStatementOperation ||
            expressionStatementOperation.Operation is not ISimpleAssignmentOperation simpleAssignmentOperation ||
            simpleAssignmentOperation.Value is not IParameterReferenceOperation parameterReferenceOperation)
        {
            return null;
        }

        if (!SymbolEqualityComparer.Default.Equals(
                semanticModel.GetDeclaredSymbol(method.ParameterList.Parameters.First()),
                parameterReferenceOperation.Parameter))
        {
            return null;
        }

        var field = simpleAssignmentOperation.Target;
        if (field is not IFieldReferenceOperation fieldReferenceOperation) return null;

        return new KeyValuePair<ISymbol?, MethodDeclarationSyntax>(fieldReferenceOperation.Field, method);
    }
}