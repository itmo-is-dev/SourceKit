namespace SourceKit.Generators.Builder.Annotations;

[AttributeUsage(AttributeTargets.Parameter)]
public sealed class BuilderPropertyAttribute(BuilderPropertyOptions options) : Attribute;
