using System;
using System.Collections.Generic;
using System.Linq;
using SourceKit.Generators.Builder.Annotations;

namespace SourceKit.Sample.Generators;

public class ArrayQueryUsage
{
    public void A()
    {
        var query = ArrayQuery1.Build(x => x.WithId(Guid.NewGuid()));
    }
}

[GenerateBuilder]
public record ArrayQuery1(Guid[] Ids, [RequiredValue] string Value, string NotRequiredValue)
{
    public static ArrayQuery1 Build(Func<Builder, Builder> action)
    {
        return action(new Builder()).Build();
    }

    public sealed class Builder
    {
        private readonly List<System.Guid> _ids;

        public Builder()
        {
            _ids = new List<System.Guid>();
        }

        [InitializesProperty(nameof(Ids))]
        public Builder WithId(Guid element)
        {
            _ids.Add(element);
            return this;
        }

        [InitializesPropertyAttribute(nameof(Ids))]
        public Builder WithIds(IEnumerable<System.Guid> elements)
        {
            _ids.AddRange(elements);
            return this;
        }

        public ArrayQuery1 Build()
        {
            return new ArrayQuery1(_ids.Distinct().ToArray(), string.Empty, string.Empty);
        }
    }
}