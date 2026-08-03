using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace SourceKit.Generators.Builder.Generators;

public sealed record BuilderDescriptor(
    SyntaxToken? NamespaceIdentifier,
    SyntaxToken ModelIdentifier,
    SyntaxKind ModelKind,
    SyntaxTokenList ModelAccessModifiers,
    Location ModelLocation,
    ImmutableArray<IBuilderParameter> Properties);
