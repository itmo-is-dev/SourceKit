using System.Collections.Generic;

namespace SourceKit.Sample.Analyzers.Collections.Dictionary;

public class CustomKeyTypeImplementsEquatable
{
    public Dictionary<EquatableType, string> Custom { get; set; }
}