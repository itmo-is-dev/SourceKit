using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace SourceKit.Extensions;

public static class MethodSyntaxExtensions
{
    public static Location GetSignatureLocation(this MethodDeclarationSyntax method)
    {
        Location methodLocation = method.GetLocation();
        Location parameterListLocation = method.ParameterList.GetLocation();

        var signatureSpan = TextSpan.FromBounds(
            methodLocation.SourceSpan.Start,
            parameterListLocation.SourceSpan.End);

        return Location.Create(method.SyntaxTree, signatureSpan);
    }
}
