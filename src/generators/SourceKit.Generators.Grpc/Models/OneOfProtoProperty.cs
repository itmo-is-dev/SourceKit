using Microsoft.CodeAnalysis;

namespace SourceKit.Generators.Grpc.Models;

public record OneOfProtoProperty(
    string Name,
    INamedTypeSymbol Type,
    int Ordinal) : ValueProtoProperty(Name, Type, Ordinal);