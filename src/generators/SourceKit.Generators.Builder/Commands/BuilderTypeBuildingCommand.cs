using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceKit.Generators.Builder.Commands;

public record struct BuilderTypeBuildingCommand(
    INamedTypeSymbol TypeSymbol,
    TypeDeclarationSyntax BuilderSyntax,
    IReadOnlyCollection<IPropertySymbol> Properties,
    GeneratorExecutionContext Context);