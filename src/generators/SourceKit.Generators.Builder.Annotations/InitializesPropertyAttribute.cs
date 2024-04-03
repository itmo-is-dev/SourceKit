namespace SourceKit.Generators.Builder.Annotations;

[AttributeUsage(AttributeTargets.Method)]
public class InitializesPropertyAttribute(string PropertyName) : Attribute;