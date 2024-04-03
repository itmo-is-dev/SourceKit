# SourceKit.Generators.Builder

Use `[GenerateBuilder]` attribute to generate builder.

- Only `record` or `record struct` types are supported 
- Builder is a nested class `Builder`
- For collection properties field of type `List<>` will be generated
- For collection properties, methods `With{PropName}` and `With{PropName}s` will be generated
- If collection element type is `IComparable` or `IComparable<TSelf>` the `.Distinct` LINQ method would be called on `.Build` method
- If collection property is `List<>` or `HashSet<>` then it will be converted to this collection respectively, otherwise, `ToArray` method will be called

You can use `SomeType.Build()` static method to shorthand create type instance using generated builder

```csharp
[GenerateBuilder]
public partial record SomeQuery(IReadOnlyCollection<Guid> Ids, int Count);

...

var query = SomeQuery.Build(x => x.WithCount(2).WithId(Guid.NewGuid());
```

## Required properties

You can annotate property with `[RequiredValue]` attribute to force compile time error 
when it is not initialized withing `Build` method of model.

```csharp
[GenerateBuilder]
public partial record SomeQuery(long[] Ids, [RequiredValue] int PageSize);
```

The following code will produce an error.

```csharp
var query = SomeQuery.Build(x => x.WithId(1));
```