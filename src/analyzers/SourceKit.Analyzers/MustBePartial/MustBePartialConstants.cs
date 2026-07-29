using SourceKit.Analyzers.Annotations.MustBePartial;

namespace SourceKit.Analyzers.MustBePartial;

public static class MustBePartialConstants
{
    public const string AnnotationsNamespace = "SourceKit.Analyzers.Annotations.MustBePartial";

    public const string DerivativesMustBePartialAttributeName = nameof(DerivativesMustBePartialAttribute);

    public const string DerivativesMustBePartialAttributeFullyQualifiedName =
        $"{AnnotationsNamespace}.{DerivativesMustBePartialAttributeName}";

    public const string AnnotatedMustBePartialAttributeName = nameof(AnnotatedMustBePartialAttribute);

    public const string AnnotatedMustBePartialAttributeFullyQualifiedName =
        $"{AnnotationsNamespace}.{AnnotatedMustBePartialAttributeName}";
}