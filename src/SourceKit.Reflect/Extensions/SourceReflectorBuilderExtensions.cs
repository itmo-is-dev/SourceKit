using SourceKit.Reflect.Reflectors;

namespace SourceKit.Reflect.Extensions;

public static class SourceReflectorBuilderExtensions
{
    public static SourceReflector.SourceReflectorBuilder WithEnumerableTypes(
        this SourceReflector.SourceReflectorBuilder builder)
    {
        builder.WithReferencedAssembly(typeof(Enumerable).Assembly);
        return builder;
    }
}