using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace SourceKit.Generators.Grpc.Models;

public record ProtoMessage(
    INamedTypeSymbol Type,
    ImmutableArray<ProtoProperty> Properties,
    ImmutableArray<ProtoMessage> NestedMessages);
