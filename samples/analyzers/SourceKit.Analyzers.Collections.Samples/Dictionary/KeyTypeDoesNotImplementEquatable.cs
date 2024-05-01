using System.Collections.Generic;

namespace SourceKit.Analyzers.Collections.Samples.Dictionary;

public class KeyTypeDoesNotImplementEquatable
{
    public Dictionary<object, int> Sample { get; set; } = new();
}