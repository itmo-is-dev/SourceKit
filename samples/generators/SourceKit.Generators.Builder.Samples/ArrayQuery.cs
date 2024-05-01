using System;
using SourceKit.Generators.Builder.Annotations;

namespace SourceKit.Generators.Builder.Samples;

[GenerateBuilder]
public partial record ArrayQuery(Guid[] Ids);