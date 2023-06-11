using SourceKit.Generators.Builder.Annotations;
using SourceKit.Sample.Models;

namespace SourceKit.Sample.Generators;

[GenerateBuilder]
public partial record CustomModelQuery(Point[] Point);