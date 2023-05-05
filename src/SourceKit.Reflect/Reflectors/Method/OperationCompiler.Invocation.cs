using System.Reflection;
using Microsoft.CodeAnalysis.Operations;
using SourceKit.Extensions;
using SourceKit.Reflect.Extensions;

namespace SourceKit.Reflect.Reflectors.Method;

internal partial class OperationCompiler
{
    public override void VisitInvocation(IInvocationOperation operation)
    {
        var methodSymbol = operation.TargetMethod;
        var containerType = _typeResolver.FindType(methodSymbol.ContainingType);

        if (containerType is null)
        {
            var syntax = operation.TargetMethod.ConstructedFrom.DeclaringSyntaxReferences.SingleOrDefault()?.GetSyntax();
            var methodType = _typeResolver.FindDelegateType(methodSymbol);

            if (syntax is null || methodType is null)
                return;

            var innerMethod = _methodReflector.CreateMethod(methodType, syntax, _compilation);

            foreach (var argument in operation.Arguments)
            {
                argument.Accept(this);
            }

            _emit.Operation(x => x.Call(innerMethod.Method));

            return;
        }

        IEnumerable<MethodInfo> methods = containerType
            .GetMethods()
            .Where(x => x.Name.Equals(methodSymbol.Name));

        if (methodSymbol.IsStatic)
        {
            methods = methods.Where(x => x.IsStatic);
        }

        Type[] parameterTypes = methodSymbol.Parameters
            .Select(x => _typeResolver.FindType(x.Type))
            .WhereNotNull()
            .ToArray();

        methods = methods.Where(x => _typeResolver.IsParametersGenericTypeCompatible(parameterTypes, x));

        var method = methods
            .OrderBy(x => _typeResolver.GetParametersGenericDifference(parameterTypes, x))
            .FirstOrDefault();

        if (method is null)
            throw new NotSupportedException($"Method symbol {methodSymbol} is not supported");

        if (methodSymbol.IsGenericMethod)
        {
            Type[] arguments = methodSymbol.TypeArguments.Select(symbol => _typeResolver.FindType(symbol)).WhereNotNull()
                .ToArray();
            method = method.MakeGenericMethod(arguments);
        }

        foreach (var argument in operation.Arguments)
        {
            argument.Accept(this);
        }

        _emit.Operation(x => x.Call(method));
    }
}