using SourceKit.Analyzers.Annotations.MustBePartial;

namespace SourceKit.Generators.Builder.Annotations;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
[AnnotatedMustBePartial]
public class GenerateBuilderAttribute : Attribute { }