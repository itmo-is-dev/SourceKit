using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Generators.Builder.Models;

public readonly record struct InitializesPropertyAttributeBuilder(string PropertyName)
{
    private static readonly AttributeSyntax AttributeValue = Attribute(
        IdentifierName(Constants.InitializesPropertyAttributeName));

    public static implicit operator AttributeListSyntax(InitializesPropertyAttributeBuilder builder)
    {
        var propertyName = Argument(IdentifierName(builder.PropertyName));

        var nameofSyntax = IdentifierName(Identifier(
            TriviaList(),
            SyntaxKind.NameOfKeyword,
            "nameof",
            "nameof",
            TriviaList()));

        var argument = AttributeArgument(InvocationExpression(nameofSyntax).AddArgumentListArguments(propertyName));
        var attribute = AttributeValue.AddArgumentListArguments(argument);

        return AttributeList(SingletonSeparatedList(attribute));
    }
}