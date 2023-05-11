using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceKit.Generators.Builder.Builders.FileBuilders;

public record struct BuilderTypeBuildingCommand(TypeDeclarationSyntax TypeSyntax, TypeDeclarationSyntax BuilderSyntax);