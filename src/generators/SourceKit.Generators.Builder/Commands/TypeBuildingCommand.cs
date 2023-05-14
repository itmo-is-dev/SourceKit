using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceKit.Generators.Builder.Commands;

public record struct TypeBuildingCommand(
    GeneratorExecutionContext Context,
    TypeDeclarationSyntax Syntax,
    INamedTypeSymbol Symbol);