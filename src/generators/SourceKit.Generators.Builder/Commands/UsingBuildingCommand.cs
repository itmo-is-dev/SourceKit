using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceKit.Generators.Builder.Commands;

public record struct UsingBuildingCommand(UsingDirectiveSyntax Syntax, INamedTypeSymbol TypeSymbol);