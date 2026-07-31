using System.Collections.Generic;
using SourceKit.Generators.Builder.Annotations;

namespace SourceKit.Generators.Builder.Samples.BuilderConstructorParameter;

[GenerateBuilder]
public sealed partial record IntegerConstructorParameterQuery(
    [BuilderConstructorParameter] int? Id,
    [BuilderConstructorParameter] int[] Values,
    List<int> NotParameterValues,
    string Value)
{
    public partial class Builder
    {
        public int? Id => _id;
    }
}
