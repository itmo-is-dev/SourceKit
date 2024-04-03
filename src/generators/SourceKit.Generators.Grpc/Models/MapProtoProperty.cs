using Microsoft.CodeAnalysis;

namespace SourceKit.Generators.Grpc.Models;

public sealed record MapProtoProperty(
    string Name,
    int Ordinal,
    INamedTypeSymbol Key,
    INamedTypeSymbol Value) : ProtoProperty(Name, Ordinal);