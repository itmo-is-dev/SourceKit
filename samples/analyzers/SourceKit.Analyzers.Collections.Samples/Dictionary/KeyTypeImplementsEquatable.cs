using System.Collections.Generic;

namespace SourceKit.Analyzers.Collections.Samples.Dictionary;

public class KeyTypeImplementsEquatable
{
    public Dictionary<int, string> Sample { get; set; } = new();
}