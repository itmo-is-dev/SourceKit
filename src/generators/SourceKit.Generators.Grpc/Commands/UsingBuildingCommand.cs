using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceKit.Generators.Grpc.Commands;

public record struct UsingBuildingCommand(UsingDirectiveSyntax Syntax);