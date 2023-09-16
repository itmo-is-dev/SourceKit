using System.Collections.Generic;

namespace SourceKit.Sample.Analyzers.Collections.Dictionary;

public class EnumKeyDictionary
{
    private readonly Dictionary<CustomEnumKey, string> _dictionary;

    public EnumKeyDictionary(Dictionary<CustomEnumKey, string> dictionary)
    {
        _dictionary = dictionary;
    }
}

public enum CustomEnumKey
{
    Value,
}