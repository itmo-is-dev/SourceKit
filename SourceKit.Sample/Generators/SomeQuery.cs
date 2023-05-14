using System;
using System.Collections.Generic;
using SourceKit.Generators.Builder.Annotations;

namespace SourceKit.Sample.Generators;

[GenerateBuilder]
public partial record SomeQuery(IReadOnlyCollection<Guid> Ids, int Count, int? OrderById)
{
    public bool HasOrderParameters => OrderById is not null;
}