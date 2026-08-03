using Microsoft.CodeAnalysis;

namespace SourceKit.Generators.Builder.Generators;

public interface IBuilderParameter : IEquatable<IBuilderParameter>
{
    void Configure(BuilderSyntax builder, Compilation compilation);
}
