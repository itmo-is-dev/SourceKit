using System.Collections.Generic;
using SourceKit.Generators.Builder.Annotations;
using static SourceKit.Generators.Builder.Annotations.BuilderPropertyOptions;

namespace SourceKit.Generators.Builder.Samples.BuilderConstructorParameter;

[GenerateBuilder]
public sealed partial record IntegerConstructorParameterQuery(
    [BuilderProperty(ConstructorParameter | Exposed)]
    int? Id,
    [BuilderProperty(ConstructorParameter)]
    int[] Values,
    List<int> NotParameterValues,
    string Value);
