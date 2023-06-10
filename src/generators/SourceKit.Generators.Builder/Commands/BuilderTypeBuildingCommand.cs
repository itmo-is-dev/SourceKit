using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Generators.Builder.Models;

namespace SourceKit.Generators.Builder.Commands;

public record struct BuilderTypeBuildingCommand(
    INamedTypeSymbol TypeSymbol,
    TypeDeclarationSyntax BuilderSyntax,
    IReadOnlyCollection<BuilderProperty> Properties,
    GeneratorExecutionContext Context);