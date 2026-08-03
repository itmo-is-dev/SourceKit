# SourceKit.Generators.Builder

A source generator that generates builders for types.

- Mark a type with `[GenerateBuilder]` attribute
- Generator supports types with single or primary constructor
- `Builder` class will be generated inside annotated type
- Builder will have methods that set/add values
- `With{PropName}` for non-collection properties
- `With{PropName}` and `With{PropName}s` for collection properties
- Builder can be used as a standalone object, or it can be used within static Build method
    - Static Build method accepts delegate with configures builder and calls build immediately, returning the built
      object
    - It is primarily used for scoping the builder calls which allows for analysis of methods called on builder (for
      required values)

```csharp
[GenerateBuilder]
public partial record SomeQuery(IReadOnlyCollection<Guid> Ids, int Count = 10);

...

var query = SomeQuery.Build(x => x.WithCount(2).WithId(Guid.NewGuid());
```

> Default values for record properties are supported

## Configure builder properties

Use `[BuilderPropery]` and `BuilderPropertyOptions` to customize generated code.

### Required properties

Use `BuilderPropertyOptions.Required` to configure required properties. This annotation will be used for analysis of
calls to static Build method, it will ensure that the method that initializes this parameter is called within the
delegate.

```csharp
[Generate builder]
public partial record SomeQuery(long[] Ids, [BuilderProperty(BuilderPropertyOptions.Required)] int PageSize);
```

The following code will produce an error.

```csharp
var query = SomeQuery.Build(x => x.WithId(1));
```

### Constructor parameter

Use `BuilderPropertyOptions.ConstructorParameter` to configure parameter to be builder's constructor parameter. It also
would be a
parameter of a static Build method. Such option is primarily used when you need a guarantee that a certain value is
specified when working with builder at any point (this option is commonly combined with `Exposed` option).

```csharp
[Generate builder]
public partial record SomeQuery(long[] Ids, [BuilderProperty(BuilderPropertyOptions.ConstructorParameter)] int PageSize);
```

```csharp
var query = SomeQuery.Build(1, builder => builder.WithId(2));
```

### Exposed parameter

Use `BuilderPropertyOptions.Exposed` to configure parameter to be exposed as a property.

- This option is commonly combined with `ConstructorParameter` option, ex:
    - you have a builder for some entity that is used in an enrichment pipeline as a data buffer
    - you will need an id of the entity at each enrichment step
    - so it would be useful to have it exposed as a property of the builder
    - and it is useful that it is guaranteed to be specified on builder creation
- For non-collection parameters annotated with `Exposed` and `ConstructorParameter` options the non-nullable property
  will be generated (if the underlying parameter's type is non-nullable)
- For non-collection parameters annotated with `Exposed` option only – the property will be nullable.
- For collection parameter, annotated with `Exposed` option the IEnumerable type is used, when collection parameter yet
  to be called, then the empty enumerable will be returned.

```csharp
[Generate builder]
public partial record SomeQuery(
    long[] Ids, 
    [BuilderProperty(BuilderPropertyOptions.ConstructorParameter | BuilderPropertyOptions.Exposed)] 
    int PageSize);
```

```csharp
var builder = new SomeQuery.Builder(pageSize: 1);
Console.WriteLine(builder.PageSize); // 1
```

### Allow duplicates

Use `BuilderPropertyOptions.AllowsDuplicates` to disable deduplication logic for collections with equatable elements.

- By default, builder uses `HashSet<>` to store intermediate values of collection parameters with equatable elements
- There may be reasons for you not wanting to do that:
    - performance overhead
    - logic requirements
- When parameter is annotated with `AllowDuplicates` option, the `List<>` would be used to store intermediate values

```csharp
[Generate builder]
public partial record SomeQuery([BuilderProperty(BuilderPropertyOptions.AllowsDuplicates)] long[] Ids);
```

```csharp
var query = SomeQuery.Build(builder => builder.WithId(1).WithId(1));
Console.WriteLine(query.Ids.Length); // 2
```

### Disable collection copying

Use `BuilderPropertyOptions.NotCopied` to avoid copying intermediate collections when consturcting models.

- By default, when passing values of collection properties the spread operator is used (`[..values]`)
- For better performance this behavior may be altered and copying could be disabled
- Note that generator does not validate type compatibility on its own
    - If you have an array parameter and annotate it with `NotCopied` option – the compiler error will occur
    - It happens as the type of intermediate collection is not compatible with your parameter type
    - You should manually match the type of intermediate collection, or use collection interfaces
- Consider the builder's usecase and whether the data will be or won't be copied down the line
    - For example: Npgsql could be a valid usecase for this option
    - If you want to allocate as little memory as possible you may want to enable this option
    - But! Npgsql only works with arrays and types that implement `IList<>` the conditions that `HashSet<>` type fails
      to meet
    - So if your usecase has some collection's type limitations, consider combining this option with `AllowDuplicates`
      as the `List<>` is wider acceptable type.

```csharp
[Generate builder]
public partial record SomeQuery([BuilderProperty(BuilderPropertyOptions.NotCopied)] IReadOnlyCollection<long> Ids);
```

```csharp
var builder = new SomeQuery.Builder().WithId(1);
var model = builder.Build();
builder.WithId(2);

Console.WriteLine(model.Ids.Count); // 2

```

---