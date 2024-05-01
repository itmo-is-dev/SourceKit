namespace SourceKit.Generators.Builder.Annotations;

#pragma warning disable CS9113 // Parameter is unread.

[AttributeUsage(AttributeTargets.Method)]
public class InitializesPropertyAttribute(string propertyName) : Attribute;