using SourceKit.Analyzers.MustBePartial.Annotations;

namespace SourceKit.Analyzers.MustBePartial.Tools;

public static class Constants
{
    public const string AnnotationsNamespace = "SourceKit.Analyzers.MustBePartial.Annotations";

    public const string DerivativesMustBePartialAttributeName = nameof(DerivativesMustBePartialAttribute);

    public const string DerivativesMustBePartialAttributeFullyQualifiedName =
        $"{AnnotationsNamespace}.{DerivativesMustBePartialAttributeName}";

    public const string AnnotatedMustBePartialAttributeName = nameof(AnnotatedMustBePartialAttribute);

    public const string AnnotatedMustBePartialAttributeFullyQualifiedName =
        $"{AnnotationsNamespace}.{AnnotatedMustBePartialAttributeName}";
}