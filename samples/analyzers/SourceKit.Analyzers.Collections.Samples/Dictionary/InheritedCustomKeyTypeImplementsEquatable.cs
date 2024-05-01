using System;
using System.Collections.Generic;

namespace SourceKit.Analyzers.Collections.Samples.Dictionary;

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