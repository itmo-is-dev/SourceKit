using Microsoft.CodeAnalysis;

namespace SourceKit.Generators.Grpc.Models;

public record ProtoMessage(INamedTypeSymbol Type, IReadOnlyCollection<ProtoProperty> Properties);