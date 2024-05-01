using System;
using SourceKit.Generators.Builder.Annotations;

namespace SourceKit.Sample.Generators;

public class InitializedRequiredValueSample
{
    public void A()
    {
        var query = InitializedRequiredValueSampleQuery.Build(x => x.WithValue("aboba"));
    }
}

[GenerateBuilder]
public partial record InitializedRequiredValueSampleQuery(
    Guid[] Ids,
    [RequiredValue] string Value,
    string NotRequiredValue = "aboba");