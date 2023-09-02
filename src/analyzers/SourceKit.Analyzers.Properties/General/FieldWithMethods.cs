using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceKit.Analyzers.Properties.General;

public class FieldWithMethods
{
    public FieldWithMethods(
        VariableDeclaratorSyntax variable, 
        List<MethodDeclarationSyntax> getMethods,
        List<MethodDeclarationSyntax> setMethods)
    {
        Variable = variable;
        GetMethods = getMethods;
        SetMethods = setMethods;
    }
    
    public FieldWithMethods(VariableDeclaratorSyntax variable)
    {
        Variable = variable;
        GetMethods = new List<MethodDeclarationSyntax>();
        SetMethods = new List<MethodDeclarationSyntax>();
    }

    public VariableDeclaratorSyntax Variable { get; }
    public List<MethodDeclarationSyntax> GetMethods { get; set; }
    public List<MethodDeclarationSyntax> SetMethods { get; set; }
}