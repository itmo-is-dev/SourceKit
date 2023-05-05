using System.Reflection;
using Microsoft.CodeAnalysis;
using SourceKit.Extensions;

namespace SourceKit.Reflect.Reflectors;

public class TypeResolver : ITypeResolver
{
    private readonly IReadOnlyCollection<Type> _types;

    public TypeResolver(IReadOnlyCollection<Type> types)
    {
        _types = types;
    }

    public Type? FindType(ITypeSymbol? symbol)
    {
        if (symbol is null)
            return null;

        if (symbol is IArrayTypeSymbol arrayTypeSymbol)
        {
            var elementType = FindType(arrayTypeSymbol.ElementType);

            if (elementType is null)
                return null;

            return arrayTypeSymbol.Rank is 1
                ? elementType.MakeArrayType()
                : elementType.MakeArrayType(arrayTypeSymbol.Rank);
        }

        var symbolName = symbol.GetFullyQualifiedName();
        var type = _types.SingleOrDefault(x => symbolName.Equals(x.FullName));

        if (type is null)
            return type;

        if (symbol is not INamedTypeSymbol namedTypeSymbol)
            return type;

        if (namedTypeSymbol.IsGenericType is false)
            return type;

        Type[] typeArguments = namedTypeSymbol.TypeArguments
            .Select(symbol1 => FindType(symbol1))
            .WhereNotNull()
            .ToArray();

        return type.MakeGenericType(typeArguments);
    }

    public Type FindType(string typeName)
    {
        var type = _types.SingleOrDefault(x => typeName.Equals(x.FullName));

        if (type is null)
            throw new ArgumentException("Unknown type", nameof(typeName));

        return type;
    }

    public Type? FindDelegateType(IMethodSymbol method)
    {
        var assembly = typeof(Action).Assembly;
        var prefix = method.ReturnsVoid ? "Action" : "Func";
        var typeParameterCount = method.Parameters.Length + (method.ReturnsVoid ? 0 : 1);

        var genericType = assembly.DefinedTypes
            .Where(x => x.Name.StartsWith(prefix))
            .SingleOrDefault(x => x.GenericTypeParameters.Length.Equals(typeParameterCount));

        if (genericType is null)
            return null;

        IEnumerable<Type> typeArgumentsEnumerable = method.Parameters
            .Select(x => x.Type)
            .Select(FindType)
            .WhereNotNull();

        if (method.ReturnsVoid is false)
        {
            var returnType = FindType(method.ReturnType);

            if (returnType is not null)
            {
                typeArgumentsEnumerable = typeArgumentsEnumerable.Append(returnType);
            }
        }

        Type[] typeArguments = typeArgumentsEnumerable.ToArray();

        return genericType.MakeGenericType(typeArguments);
    }

    public bool IsParametersGenericTypeCompatible(IEnumerable<Type> parameterTypes, MethodInfo method)
    {
        IEnumerable<Type> secondaryEnumerable = method.GetParameters().Select(x => x.ParameterType);

        using IEnumerator<Type> primary = parameterTypes.GetEnumerator();
        using IEnumerator<Type> secondary = secondaryEnumerable.GetEnumerator();

        (bool Left, bool Right) moveResult = (primary.MoveNext(), secondary.MoveNext());

        while (moveResult is (true, true))
        {
            var primaryType = primary.Current!;
            var secondaryType = secondary.Current!;

            while (primaryType != secondaryType)
            {
                if (primaryType.IsConstructedGenericType is false || secondaryType.ContainsGenericParameters is false)
                    return false;

                primaryType = primaryType.GetGenericTypeDefinition();
                secondaryType = secondaryType.GetGenericTypeDefinition();
            }

            moveResult = (primary.MoveNext(), secondary.MoveNext());
        }

        return moveResult.Left == moveResult.Right;
    }

    public int GetParametersGenericDifference(IEnumerable<Type> parameterTypes, MethodInfo method)
    {
        IEnumerable<Type> secondaryEnumerable = method.GetParameters().Select(x => x.ParameterType);

        using IEnumerator<Type> primary = parameterTypes.GetEnumerator();
        using IEnumerator<Type> secondary = secondaryEnumerable.GetEnumerator();

        var counter = 0;

        (bool Left, bool Right) moveResult = (primary.MoveNext(), secondary.MoveNext());

        while (moveResult is (true, true))
        {
            var primaryType = primary.Current!;
            var secondaryType = secondary.Current!;

            while (primaryType != secondaryType)
            {
                if (primaryType.IsConstructedGenericType is false || secondaryType.ContainsGenericParameters is false)
                {
                    counter = (counter + 1) * 2;
                }
                else
                {
                    counter++;
                }

                primaryType = primaryType.GetGenericTypeDefinition();
                secondaryType = secondaryType.GetGenericTypeDefinition();
            }

            moveResult = (primary.MoveNext(), secondary.MoveNext());
        }

        return counter;
    }
}