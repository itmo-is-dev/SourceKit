using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Generators.Grpc.Models;

namespace SourceKit.Generators.Grpc.Commands;

public readonly record struct TypeBuildingCommand(
    ProtoMessage Message,
    TypeDeclarationSyntax Syntax,
    GeneratorExecutionContext Context);