using System.Collections.Generic;

namespace SourceKit.Analyzers.Collections.Samples.Dictionary;

public class CustomKeyTypeImplementsEquatable
{
    public Dictionary<EquatableType, string> Custom { get; set; } = new();
}