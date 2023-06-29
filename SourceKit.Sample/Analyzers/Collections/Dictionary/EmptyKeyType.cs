using System;
using System.Collections.Generic;

namespace SourceKit.Sample.Analyzers.Collections.Dictionary;

public class EmptyKeyType
{
    private Type _dictionary = typeof(Dictionary<,>);
}