using SourceKit.Generators.Builder.Annotations;

namespace SourceKit.Generators.Builder;

public static class Constants
{
    public const string AnnotationsNamespace = "SourceKit.Generators.Builder.Annotations";

    public const string GenerateBuilderAttributeName = nameof(GenerateBuilderAttribute);
    public const string InitializesPropertyAttributeName = nameof(InitializesPropertyAttribute);
    public const string RequiredValueAttributeName = nameof(RequiredValueAttribute);
    public const string BuilderConstructorParameterAttributeName = nameof(BuilderConstructorParameterAttribute);

    public const string GenerateBuilderAttributeFullyQualifiedName =
        $"{AnnotationsNamespace}.{GenerateBuilderAttributeName}";

    public const string InitializesPropertyAttributeFullyQualifiedName =
        $"{AnnotationsNamespace}.{InitializesPropertyAttributeName}";

    public const string RequiredValueAttributeFullyQualifiedName =
        $"{AnnotationsNamespace}.{RequiredValueAttributeName}";

    public const string BuilderConstructorParameterAttributeFullyQualifiedName =
        $"{AnnotationsNamespace}.{BuilderConstructorParameterAttributeName}";

    public const string FilenameSuffix = "SourceKit.Generation.Builder.g.cs";
}