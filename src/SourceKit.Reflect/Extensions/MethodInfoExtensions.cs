using System.Reflection;

namespace SourceKit.Reflect.Extensions;

public static class MethodInfoExtensions
{
    /// <summary>
    ///     Checks is method has parameter sequence of specified types
    /// </summary>
    /// <param name="method"></param>
    /// <param name="parameterTypes">Parameter type;null – generic prameter</param>
    /// <returns></returns>
    public static bool HasParameterTypes(this MethodInfo method, params Type?[] parameterTypes)
    {
        IEnumerable<Type> methodParameterTypes = method.GetParameters().Select(x => x.ParameterType);

        using IEnumerator<Type> methodParameter = methodParameterTypes.GetEnumerator();
        using IEnumerator<Type?> parameter = parameterTypes.Select(x => x).GetEnumerator();

        (bool Left, bool Right) moveResult = (methodParameter.MoveNext(), parameter.MoveNext());

        while (moveResult is (true, true))
        {
            var methodType = methodParameter.Current;
            var type = parameter.Current;

            if (methodType != type && (methodType!.IsGenericParameter && type is null) is false)
                return false;

            moveResult = (methodParameter.MoveNext(), parameter.MoveNext());
        }

        return moveResult.Left == moveResult.Right;
    }
    
    
}