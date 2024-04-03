using SourceKit.Generators.Builder.Annotations;

namespace SourceKit.Generators.Builder;

public static class Constants
{
    public const string AnnotationsNamespace = "SourceKit.Generators.Builder.Annotations";

    public const string GenerateBuilderAttributeName = nameof(GenerateBuilderAttribute);
    public const string InitializesPropertyAttributeName = nameof(InitializesPropertyAttribute);
    public const string RequiredValueAttributeName = nameof(RequiredValueAttribute);

    public const string GenerateBuilderAttributeFullyQualifiedName =
        $"{AnnotationsNamespace}.{GenerateBuilderAttributeName}";

    public const string InitializesPropertyAttributeFullyQualifiedName =
        $"{AnnotationsNamespace}.{InitializesPropertyAttributeName}";

    public const string RequiredValueAttributeFullyQualifiedName =
        $"{AnnotationsNamespace}.{RequiredValueAttributeName}";

    public const string EnumerableFullyQualifiedName = "System.Collections.IEnumerable";

    public const string GenericEnumerableFullyQualifiedName = "System.Collections.Generic.IEnumerable`1";

    public const string ListFullyQualifiedName = "System.Collections.Generic.List`1";

    public const string FilenameSuffix = "SourceKit.Generation.Builder.g.cs";
}