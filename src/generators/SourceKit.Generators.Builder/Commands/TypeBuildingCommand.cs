using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceKit.Generators.Builder.Commands;

public record struct TypeBuildingCommand(
    Compilation Compilation,
    TypeDeclarationSyntax Syntax,
    INamedTypeSymbol Symbol);