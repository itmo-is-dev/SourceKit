using System;
using System.Collections.Generic;

namespace SourceKit.Sample.Analyzers.Collections.Dictionary;

public class CustomKeyTypeImplementsOtherEquatable
{
    class KeyType : IEquatable<int>
    {
        public bool Equals(int other)
        {
            throw new NotImplementedException();
        }
    }

    class Sample
    {
        public static Dictionary<KeyType, string> fjdkfj { get; } = new();
    }
}