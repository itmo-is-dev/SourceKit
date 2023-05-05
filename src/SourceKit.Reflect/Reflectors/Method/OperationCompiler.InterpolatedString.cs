using System.Collections.Immutable;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using Sigil.NonGeneric;
using SourceKit.Reflect.Extensions;

namespace SourceKit.Reflect.Reflectors.Method;

internal partial class OperationCompiler
{
    public override void VisitInterpolatedString(IInterpolatedStringOperation operation)
    {
        var handlerType = _typeResolver.FindType("System.Runtime.CompilerServices.DefaultInterpolatedStringHandler");
        var handlerConstructor = handlerType.GetConstructor(new[] { typeof(int), typeof(int) });
        MethodInfo[] handlerMethods = handlerType.GetMethods();

        var appendFormatted = handlerMethods
            .Where(x => x.Name.Equals("AppendFormatted"))
            .Where(x => x.ContainsGenericParameters)
            .Single(x => x.GetParameters().Length is 1);

        var appendFormattedWithFormat = handlerMethods
            .Where(x => x.Name.Equals("AppendFormatted"))
            .Single(x => x.HasParameterTypes(null, typeof(string)));

        var appendFormattedWithAlignment = handlerMethods
            .Where(x => x.Name.Equals("AppendFormatted"))
            .Single(x => x.HasParameterTypes(null, typeof(int)));

        var appendFormattedWithFormatAndAlignment = handlerMethods
            .Where(x => x.Name.Equals("AppendFormatted"))
            .Single(x => x.HasParameterTypes(null, typeof(int), typeof(string)));

        var appendLiteral = handlerMethods
            .Where(x => x.Name.Equals("AppendLiteral"))
            .Single(x => x.HasParameterTypes(typeof(string)));

        var toStringAndClear = handlerMethods
            .Where(x => x.Name.Equals("ToStringAndClear"))
            .Where(x => x.ReturnType == typeof(string))
            .Single(x => x.GetParameters().Length is 0);

        var literalLength = operation.Parts
            .OfType<IInterpolatedStringTextOperation>()
            .SelectMany(x => x.ChildOperations)
            .Select(x => x.ConstantValue.Value as string)
            .Sum(x => x?.Length ?? 0);

        var formattedCount = operation.Parts
            .OfType<IInterpolationOperation>()
            .Count();

        var handler = _emit.Operation(x => x.DeclareLocal(handlerType));
        _emit.Operation(x => x.LoadConstant(literalLength));
        _emit.Operation(x => x.LoadConstant(formattedCount));
        _emit.Operation(x => x.NewObject(handlerConstructor));
        _emit.Operation(x => x.StoreLocal(handler));

        foreach (var part in operation.Parts)
        {
            _emit.Operation(x => x.LoadLocalAddress(handler));

            if (part is IInterpolatedStringTextOperation stringInterpolation)
            {
                VisitLiteral((ILiteralOperation)stringInterpolation.Text);
                _emit.Operation(x => x.Call(appendLiteral));
            }

            if (part is IInterpolationOperation interpolation)
            {
                HandleInterpolationOperation(interpolation,
                    appendFormatted,
                    appendFormattedWithFormat,
                    appendFormattedWithAlignment,
                    appendFormattedWithFormatAndAlignment);
            }
        }

        _emit.Operation(x => x.LoadLocalAddress(handler));
        _emit.Operation(x => x.Call(toStringAndClear));
    }

    private void HandleInterpolationOperation(
        IInterpolationOperation interpolation,
        MethodInfo appendFormatted,
        MethodInfo appendFormattedWithFormat,
        MethodInfo appendFormattedWithAlignment,
        MethodInfo appendFormattedWithFormatAndAlignment)
    {
        var expressionType = _typeResolver.GetType(interpolation.Expression.Type);
        ImmutableArray<IOperation> operations = interpolation.ChildOperations.ToImmutableArray();

        Action<Emit> AppendFormatted(MethodInfo method, params IOperation[] formatOperations)
        {
            method = method.MakeGenericMethod(expressionType);

            foreach (var o in formatOperations)
            {
                o.Accept(this);
            }

            return x => x.Call(method);
        }

        Action<Emit> action = operations switch
        {
            [var value]
                => AppendFormatted(appendFormatted, value),

            [var value, { Syntax: InterpolationFormatClauseSyntax } format]
                => AppendFormatted(appendFormattedWithFormat, value, format),

            [var value, ILiteralOperation alignment]
                => AppendFormatted(appendFormattedWithAlignment, value, alignment),

            [var value, ILiteralOperation alignment, { Syntax: InterpolationFormatClauseSyntax } format]
                => AppendFormatted(appendFormattedWithFormatAndAlignment, value, alignment, format),

            [var value, { Syntax: InterpolationFormatClauseSyntax } format, ILiteralOperation alignment]
                => AppendFormatted(appendFormattedWithFormatAndAlignment, value, alignment, format),

            _ => throw new ArgumentOutOfRangeException(),
        };

        _emit.Operation(action);
    }
}