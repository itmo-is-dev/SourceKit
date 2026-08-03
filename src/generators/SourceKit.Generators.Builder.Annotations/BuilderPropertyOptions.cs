namespace SourceKit.Generators.Builder.Annotations;

[Flags]
public enum BuilderPropertyOptions
{
    /// <summary>
    ///     When builder used within static Build method, compiler will verify that this property is specified.
    /// </summary>
    Required = 1 << 0,

    /// <summary>
    ///     Generates constructor parameter as well as static Build method parameter for the property.
    /// </summary>
    ConstructorParameter = 1 << 1,

    /// <summary>
    ///     Generates public property for this property on builder.
    /// </summary>
    Exposed = 1 << 2,

    /// <summary>
    ///     Does not generate code that filters out duplicates
    ///     (only applicable to collections where element is equatable)    
    /// </summary>
    AllowsDuplicates = 1 << 3,

    /// <summary>
    ///     Does not reallocate collection when building the object
    ///     (only applicable to properties with List/HashSet types or with collection interface types)
    /// </summary>
    NotCopied = 1 << 4,
}
