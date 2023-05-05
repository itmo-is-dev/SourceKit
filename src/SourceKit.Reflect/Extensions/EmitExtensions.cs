using Sigil.NonGeneric;

namespace SourceKit.Reflect.Extensions;

/// <summary>
///     Helper methods for easier emit sequence debug
/// </summary>
public static class EmitExtensions
{
    public static T Operation<T>(this Emit emit, Func<Emit, T> func)
    {
        return func.Invoke(emit);
    }

    public static void Operation(this Emit emit, Action<Emit> action)
    {
        action.Invoke(emit);
    }
}