using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;
using Sigil.NonGeneric;
using SourceKit.Reflect.Models;
using SourceKit.Reflect.Extensions;
using ReflectionContext = SourceKit.Reflect.Reflectors.Context.ReflectionContext;

namespace SourceKit.Reflect.Reflectors.Method;

internal class MethodReflector
{
    private readonly ITypeResolver _typeResolver;

    private int _contextNumber;

    public MethodReflector(ITypeResolver typeResolver)
    {
        _typeResolver = typeResolver;
    }

    public T CreateMethod<T>(SyntaxNode syntax, Compilation compilation) where T : Delegate
    {
        var model = compilation.GetSemanticModel(syntax.SyntaxTree);
        var operation = model.GetOperation(syntax);
        var symbol = model.GetDeclaredSymbol(syntax);

        if (operation is not IMethodBodyOperation methodBodyOperation)
            throw new InvalidOperationException($"Operation {operation} cannot be reflected as method");

        if (symbol is not IMethodSymbol methodSymbol)
            throw new InvalidOperationException($"Symbol {symbol} cannot be reflected as method");

        var number = Interlocked.Increment(ref _contextNumber);
        var context = new ReflectionContext($"assembly_{number}", "module");

        var lambdaBuilder = new LambdaBuilder(context, _typeResolver, this, compilation);
        lambdaBuilder.Build(operation);

        var delegateType = _typeResolver.GetDelegateType(methodSymbol);
        var delegateInfo = DelegateInfo.FromType(delegateType);

        var lambdaTypeName = $"{methodSymbol.Name}_delegates";
        var typeBuilder = context.PushType(lambdaTypeName);

        var emit = Emit.BuildMethod(
            delegateInfo.ReturnType,
            delegateInfo.ParameterTypes,
            typeBuilder,
            symbol.Name,
            MethodAttributes.Public | MethodAttributes.Static,
            CallingConventions.Standard);

        var compiler = new OperationCompiler(emit, _typeResolver, this, compilation, context);
        methodBodyOperation.Accept(compiler);
        emit.CreateMethod();

        var type = typeBuilder.CreateType();
        
        var method = type.GetMethod(symbol.Name, BindingFlags.Public | BindingFlags.Static)!;
        
        context.Save("/Users/george/Documents/C#/SourceKit/SourceKit.Tests/a.dll");

        return (T)method.CreateDelegate(delegateType);
    }

    public Delegate CreateMethod(Type type, SyntaxNode syntax, Compilation compilation)
    {
        MethodInfo[] methods = typeof(MethodReflector).GetMethods();
        var method = methods.Single(x => x.IsGenericMethod && x.Name.Equals(nameof(CreateMethod)));
        var closedMethod = method.MakeGenericMethod(type);

        return (Delegate)closedMethod.Invoke(this, new object[] { syntax, compilation });
    }
}