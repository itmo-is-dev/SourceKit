using System.Reflection;
using Microsoft.CodeAnalysis;

namespace SourceKit.Reflect;

public interface ITypeResolver
{
    Type? FindType(ITypeSymbol? symbol);

    Type FindType(string typeName);

    Type? FindDelegateType(IMethodSymbol method);

    bool IsParametersGenericTypeCompatible(IEnumerable<Type> parameterTypes, MethodInfo method);

    int GetParametersGenericDifference(IEnumerable<Type> parameterTypes, MethodInfo method);
}