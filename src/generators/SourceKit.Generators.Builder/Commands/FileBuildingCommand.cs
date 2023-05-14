using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceKit.Generators.Builder.Commands;

public record struct FileBuildingCommand(
    GeneratorExecutionContext Context,
    CompilationUnitSyntax CompilationUnit,
    INamedTypeSymbol Symbol);