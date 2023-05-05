namespace SourceKit.Reflect.Models;

public record DelegateInfo(Type ReturnType, Type[] ParameterTypes)
{
    public static DelegateInfo FromType(Type type)
    {
        if (type.IsAssignableFrom(typeof(Delegate)))
            throw new ArgumentException("Specified type is not delegate");

        var invokeMethod = type.GetMethod("Invoke");

        if (invokeMethod is null)
            throw new ArgumentException("Invalid delegate type");

        Type[] parameterTypes = invokeMethod.GetParameters().Select(x => x.ParameterType).ToArray();

        return new DelegateInfo(invokeMethod.ReturnType, parameterTypes);
    }
}