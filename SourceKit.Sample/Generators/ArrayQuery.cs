using System;
using SourceKit.Generators.Builder.Annotations;

namespace SourceKit.Sample.Generators;

[GenerateBuilder]
public partial record ArrayQuery(Guid[] Ids);