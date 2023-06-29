using System.Collections.Generic;

namespace SourceKit.Sample.Analyzers.Collections.Dictionary;

public class KeyTypeDoesNotImplementEquatable
{
    public Dictionary<object, int> Sample { get; set; }
}