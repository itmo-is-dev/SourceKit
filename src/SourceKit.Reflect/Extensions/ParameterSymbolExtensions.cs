using Microsoft.CodeAnalysis;

namespace SourceKit.Reflect.Extensions;

public static class ParameterSymbolExtensions
{
    public static ushort GetIndex(this IParameterSymbol parameter)
    {
        var containingSymbol = parameter.ContainingSymbol;

        if (containingSymbol is not IMethodSymbol methodSymbol)
        {
            var message = $"Parameter {parameter} has unsupported containing symbol {containingSymbol}";
            throw new NotSupportedException(message);
        }

        var index = (ushort)methodSymbol.Parameters.IndexOf(parameter);

        return containingSymbol.IsStatic ? index : ++index;
    }
}