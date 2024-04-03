using Microsoft.CodeAnalysis;

namespace SourceKit.Generators.Grpc.Models;

public record ValueProtoProperty(string Name, INamedTypeSymbol Type, int Ordinal) : ProtoProperty(Name, Ordinal);