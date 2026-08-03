using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Generators.Builder.Models;

public readonly record struct InitializesPropertyAttributeBuilder(SyntaxToken PropertyName)
{
    private static readonly AttributeSyntax AttributeValue = Attribute(
        IdentifierName(Constants.InitializesPropertyAttributeName));

    public static implicit operator AttributeListSyntax(InitializesPropertyAttributeBuilder builder)
    {
        ArgumentSyntax propertyName = Argument(IdentifierName(builder.PropertyName));

        IdentifierNameSyntax nameofSyntax = IdentifierName(Identifier(
            TriviaList(),
            SyntaxKind.NameOfKeyword,
            "nameof",
            "nameof",
            TriviaList()));

        AttributeArgumentSyntax argument = AttributeArgument(InvocationExpression(nameofSyntax).AddArgumentListArguments(propertyName));
        AttributeSyntax attribute = AttributeValue.AddArgumentListArguments(argument);

        return AttributeList(SingletonSeparatedList(attribute));
    }
}
