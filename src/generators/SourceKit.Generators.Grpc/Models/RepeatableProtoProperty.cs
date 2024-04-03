using Microsoft.CodeAnalysis;

namespace SourceKit.Generators.Grpc.Models;

public sealed record RepeatableProtoProperty(
    string Name,
    int Ordinal,
    INamedTypeSymbol ElementType) : ProtoProperty(Name, Ordinal);