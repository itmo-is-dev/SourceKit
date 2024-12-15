using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;
using SourceKit.Generators.Grpc.Receivers;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Generators.Grpc.Generators;

[Generator]
public class ProtoMessageAliasGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new ProtoMessageAliasReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxContextReceiver is not ProtoMessageAliasReceiver receiver)
            return;

        UsingDirectiveSyntax[] directives = receiver.Symbols.Select(GenerateAlias).ToArray();

        CompilationUnitSyntax unit = CompilationUnit().AddUsings(directives).NormalizeWhitespace(eol: "\n");
        string text = unit.ToFullString();

        context.AddSource("SourceKit.Generators.Builder.ProtoAlias.cs", text);
    }

    private UsingDirectiveSyntax GenerateAlias(INamedTypeSymbol symbol)
    {
        return UsingDirective(IdentifierName(symbol.GetFullyQualifiedName()))
            .WithGlobalKeyword(Token(SyntaxKind.GlobalKeyword))
            .WithAlias(NameEquals(IdentifierName($"Proto{symbol.Name}")));
    }
}