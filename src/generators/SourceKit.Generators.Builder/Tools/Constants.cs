using SourceKit.Generators.Builder.Annotations;

namespace SourceKit.Generators.Builder.Tools;

public static class Constants
{
    public const string AnnotationsNamespace = "SourceKit.Generators.Builder.Annotations";

    public const string GenerateBuilderAttributeName = nameof(GenerateBuilderAttribute);

    public const string GenerateBuilderAttributeFullyQualifiedName =
        $"{AnnotationsNamespace}.{GenerateBuilderAttributeName}";

    public const string EnumerableFullyQualifiedName = "System.Collections.IEnumerable";

    public const string FilenameSuffix = "SourceKit.Generation.Builder.cs";
}