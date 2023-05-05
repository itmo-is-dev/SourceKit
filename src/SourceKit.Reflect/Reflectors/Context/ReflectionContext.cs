using System.Reflection;
using System.Reflection.Emit;
using Microsoft.CodeAnalysis;
using SourceKit.Reflect.Models;

namespace SourceKit.Reflect.Reflectors.Context;

public class ReflectionContext : IReflectionContext
{
    private readonly AssemblyBuilder _assemblyBuilder;
    private readonly ModuleBuilder _moduleBuilder;
    private readonly Stack<TypeBuilder> _typeBuilders;
    private readonly Dictionary<IMethodSymbol, LambdaClassInfo> _lambdaMap;

    public ReflectionContext(string assemblyName, string moduleName)
    {
        _assemblyBuilder = AssemblyBuilder
            .DefineDynamicAssembly(new AssemblyName(assemblyName), AssemblyBuilderAccess.RunAndCollect);

        _moduleBuilder = _assemblyBuilder.DefineDynamicModule(moduleName);
        _typeBuilders = new Stack<TypeBuilder>();
        _lambdaMap = new Dictionary<IMethodSymbol, LambdaClassInfo>(SymbolEqualityComparer.IncludeNullability);
    }

    public void Save(string path)
    {
        var generator = new Lokad.ILPack.AssemblyGenerator();
        generator.GenerateAssembly(_assemblyBuilder, path);
    }

    public TypeBuilder PushType(string name)
    {
        var top = _typeBuilders.Count is 0 ? null : _typeBuilders.Peek();

        if (top is not null)
        {
            var current = top.DefineNestedType(name, TypeAttributes.NestedPublic);
            _typeBuilders.Push(current);

            return current;
        }

        var root = _moduleBuilder.DefineType(name, TypeAttributes.Public);
        _typeBuilders.Push(root);

        return root;
    }

    public Type PopType()
    {
        var top = _typeBuilders.Pop();
        return top.CreateTypeInfo() ?? throw new InvalidOperationException("Could not build type");
    }

    public TypeBuilder PeekType()
        => _typeBuilders.Peek();

    public void RegisterLambda(IMethodSymbol methodSymbol, LambdaClassInfo memberInfo)
    {
        _lambdaMap[methodSymbol] = memberInfo;
    }

    public LambdaClassInfo ResolveLambda(IMethodSymbol methodSymbol)
        => _lambdaMap[methodSymbol];
}