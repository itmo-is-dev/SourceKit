using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace SourceKit.Analyzers.Properties.General;

public static class Finder
{
    public static FieldWithMethods FindFieldWithMethods(
        SyntaxNodeAnalysisContext context,
        VariableDeclaratorSyntax variableDeclarator,
        ILookup<ISymbol?, MethodDeclarationSyntax> getMethods,
        ILookup<ISymbol?, MethodDeclarationSyntax> setMethods)
    {
        var fieldWithMethods = new FieldWithMethods(variableDeclarator);
        
        var semanticModel = context.SemanticModel;

        var fieldSymbol = ModelExtensions.GetDeclaredSymbol(semanticModel, variableDeclarator);
        if (fieldSymbol is null)
        {
            return fieldWithMethods;
        }

        if (getMethods.Contains(fieldSymbol) is false)
        {
            return fieldWithMethods;
        }

        fieldWithMethods.GetMethods = getMethods[fieldSymbol].ToList();
        fieldWithMethods.SetMethods = setMethods[fieldSymbol].ToList();

        return fieldWithMethods;
    }
}