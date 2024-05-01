using SourceKit.Generators.Builder.Annotations;

namespace SourceKit.Generators.Builder.Samples;

public class RequiredValueFromParameterComponent
{
    public static Query Create(Query query)
    {
        return Query.Build(builder => builder.WithPageSize(query.PageSize));
    }
}

[GenerateBuilder]
public partial record Query([RequiredValue] int PageSize);