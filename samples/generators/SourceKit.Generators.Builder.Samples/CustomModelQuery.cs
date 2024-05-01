using SourceKit.Generators.Builder.Annotations;
using SourceKit.Generators.Builder.Samples.Models;

namespace SourceKit.Generators.Builder.Samples;

[GenerateBuilder]
public partial record CustomModelQuery(Point[] Point);