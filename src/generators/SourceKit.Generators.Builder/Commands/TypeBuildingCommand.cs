using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Generators.Builder.Models;

namespace SourceKit.Generators.Builder.Commands;

public record struct TypeBuildingCommand(
    GeneratorExecutionContext Context,
    TypeDeclarationSyntax Syntax,
    INamedTypeSymbol Symbol,
    IReadOnlyCollection<BuilderProperty> Properties);