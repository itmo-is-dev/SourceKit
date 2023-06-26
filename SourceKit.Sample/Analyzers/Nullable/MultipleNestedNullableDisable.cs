#nullable disable
using System;

namespace SourceKit.Sample.Analyzers.Nullable
{
    public class MultipleNestedNullableDisable
    {
        private static void TestFunc(string text)
        {
            Console.WriteLine(text);
        }

        private class Program
        {
            private static void Main(string[] args)
            {
                var testClass = new MultipleNestedNullableDisable();
                    #nullable disable
                TestFunc("Hello, Roslyn!");
            }
        }
    }
}