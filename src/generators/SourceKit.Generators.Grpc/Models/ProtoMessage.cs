using Microsoft.CodeAnalysis;

namespace SourceKit.Generators.Grpc.Models;

public record ProtoMessage(
    INamedTypeSymbol Type,
    IReadOnlyList<ProtoProperty> Properties,
    IReadOnlyList<ProtoMessage> NestedMessages);