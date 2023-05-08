using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;
using Sigil.NonGeneric;
using SourceKit.Reflect.Extensions;
using SourceKit.Reflect.Models;
using SourceKit.Reflect.Reflectors.Context;

namespace SourceKit.Reflect.Reflectors.Method;

internal class LambdaBuilder
{
    private readonly IReflectionContext _context;
    private readonly ITypeResolver _typeResolver;
    private readonly MethodReflector _methodReflector;
    private readonly Compilation _compilation;

    private int _number;

    public LambdaBuilder(
        IReflectionContext context,
        ITypeResolver typeResolver,
        MethodReflector methodReflector,
        Compilation compilation)
    {
        _context = context;
        _typeResolver = typeResolver;
        _methodReflector = methodReflector;
        _compilation = compilation;
    }

    public void Build(IOperation operation)
    {
        var functions = new List<IAnonymousFunctionOperation>();

        var collector = new AnonymousFunctionCollector(functions);
        operation.Accept(collector);

        foreach (var function in functions)
        {
            BuildLambda(function);
        }
    }

    private void BuildLambda(IAnonymousFunctionOperation function)
    {
        var number = Interlocked.Increment(ref _number);
        var typeBuilder = _context.PushType($"DelegateClass_{number}");
        var functionType = _typeResolver.FindDelegateType(function.Symbol);

        if (functionType is null)
            throw new NotSupportedException($"Function {function.Syntax} type is not registered");

        var delegateInfo = DelegateInfo.FromType(functionType);

        var emit = Emit.BuildMethod(
            delegateInfo.ReturnType,
            delegateInfo.ParameterTypes,
            typeBuilder,
            $"lambda_{number}",
            MethodAttributes.Public,
            CallingConventions.Standard | CallingConventions.HasThis);

        var compiler = new OperationCompiler(emit, _typeResolver, _methodReflector, _compilation, _context);
        function.Accept(compiler);

        var functionMethod = emit.CreateMethod(out var instructions);

        var functionField = typeBuilder.DefineField(
            $"{functionMethod.Name}__field",
            functionType,
            FieldAttributes.Public | FieldAttributes.Static);

        var instanceFieldBuilder = typeBuilder.DefineField(
            $"{functionMethod.Name}__instance",
            typeBuilder,
            FieldAttributes.Public | FieldAttributes.Static);

        var constructorBuilder = Emit.BuildConstructor(
            Array.Empty<Type>(),
            typeBuilder,
            MethodAttributes.Public,
            CallingConventions.Standard | CallingConventions.HasThis);

        constructorBuilder.Operation(x => x.Return());

        var constructor = constructorBuilder.CreateConstructor();

        var staticConstructorBuilder = Emit.BuildTypeInitializer(typeBuilder);
        staticConstructorBuilder.Operation(x => x.NewObject(constructor, Array.Empty<Type>()));
        staticConstructorBuilder.Operation(x => x.StoreField(instanceFieldBuilder));
        staticConstructorBuilder.Operation(x => x.Return());

        staticConstructorBuilder.CreateTypeInitializer();

        var type = typeBuilder.CreateTypeInfo();
        var delegateField = type.GetField(functionField.Name, BindingFlags.Public | BindingFlags.Static)!;
        var instanceField = type.GetField(instanceFieldBuilder.Name, BindingFlags.Public | BindingFlags.Static)!;
        var method = type.GetMethod(functionMethod.Name, BindingFlags.Public | BindingFlags.Instance)!;

        var info = new LambdaClassInfo(delegateField, instanceField, method, functionType);

        _context.RegisterLambda(function.Symbol, info);
        _context.PopType();
    }

    private class AnonymousFunctionCollector : OperationVisitor
    {
        private readonly List<IAnonymousFunctionOperation> _anonymousFunctions;

        public AnonymousFunctionCollector(List<IAnonymousFunctionOperation> anonymousFunctions)
        {
            _anonymousFunctions = anonymousFunctions;
        }

        public override void DefaultVisit(IOperation operation)
        {
            foreach (var childOperation in operation.ChildOperations)
            {
                childOperation.Accept(this);
            }
        }

        public override void VisitAnonymousFunction(IAnonymousFunctionOperation operation)
        {
            DefaultVisit(operation);
            _anonymousFunctions.Add(operation);
        }
    }
}