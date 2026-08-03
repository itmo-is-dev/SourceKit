namespace SourceKit.Generators.Builder.Annotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
[Obsolete("Use [BuilderProperty(BuilderPropertyOptions.ConstructorParameter)] instead")]
public class BuilderConstructorParameterAttribute : Attribute;