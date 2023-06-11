using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Generators.Builder.Models;

namespace SourceKit.Generators.Builder.Commands;

public record struct FileBuildingCommand(
    GeneratorExecutionContext Context,
    CompilationUnitSyntax CompilationUnit,
    INamedTypeSymbol Symbol,
    IReadOnlyCollection<BuilderProperty> Properties);