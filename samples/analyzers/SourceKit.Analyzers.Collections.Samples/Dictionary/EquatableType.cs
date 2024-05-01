using System;

namespace SourceKit.Analyzers.Collections.Samples.Dictionary;

public class EquatableType : IEquatable<EquatableType>
{
    private readonly string _field1;
    private readonly string _field2;

    public EquatableType(string field1, string field2)
    {
        _field1 = field1;
        _field2 = field2;
    }

    public bool Equals(EquatableType? other)
    {
        if (other is null)
            return false;

        return other._field1 == _field1 && other._field2 == _field2;
    }
}