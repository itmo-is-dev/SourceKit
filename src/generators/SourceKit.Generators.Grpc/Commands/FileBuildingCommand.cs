using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Generators.Grpc.Models;

namespace SourceKit.Generators.Grpc.Commands;

public readonly record struct FileBuildingCommand(
    Compilation Compilation,
    CompilationUnitSyntax CompilationUnit,
    ProtoMessage Message);