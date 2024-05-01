using System;
using SourceKit.Generators.Builder.Annotations;

namespace SourceKit.Sample.Generators;

public class UninitializedRequiredValueSample
{
    public void A()
    {
        var query = UninitializedRequiredValueSampleQuery.Build(x => x.WithId(Guid.NewGuid()));
    }
}

[GenerateBuilder]
public partial record UninitializedRequiredValueSampleQuery(
    Guid[] Ids,
    [RequiredValue] string Value,
    string NotRequiredValue = "aboba");