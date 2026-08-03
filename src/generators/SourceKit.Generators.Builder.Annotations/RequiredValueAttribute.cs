namespace SourceKit.Generators.Builder.Annotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
[Obsolete("Use [BuilderProperty(BuilderPropertyOptions.Required)] instead")]
public class RequiredValueAttribute : Attribute;
