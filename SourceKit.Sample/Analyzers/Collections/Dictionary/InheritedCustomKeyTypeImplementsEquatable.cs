using System;
using System.Collections.Generic;
using SourceKit.Sample.Analyzers.Collections.Dictionary;

namespace SourceKit.Sample.Analyzers.Collections.Dictionary;

public class InheritedCustomKeyTypeImplementsEquatable
{
    class Base : IEquatable<Base>
    {
        public bool Equals(Base? other)
        {
            throw new NotImplementedException();
        }
    }

    class Derived : Base {}

    static class Helper
    {
        public static Dictionary<Derived, string> Dict { get; } = new();
    }
}