using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;
using Sigil;
using SourceKit.Extensions;
using SourceKit.Reflect.Models;

namespace SourceKit.Reflect.Reflectors.Instance;

internal class InstanceReflector
{
    private readonly ConcurrentDictionary<TypeMemberKey, Func<object?, Action<object>>> _assignmentCache;
    private readonly IReadOnlyCollection<Type> _types;
    private readonly IInstanceReflectorFallback? _fallback;

    public InstanceReflector(IReadOnlyCollection<Type> types, IInstanceReflectorFallback? fallback)
    {
        _types = types;
        _fallback = fallback;
        _assignmentCache = new ConcurrentDictionary<TypeMemberKey, Func<object?, Action<object>>>();
    }

    public T CreateInstance<T>(SyntaxNode syntax, Compilation compilation)
    {
        var model = compilation.GetSemanticModel(syntax.SyntaxTree);
        var operation = model.GetOperation(syntax);
        var type = typeof(T);

        if (operation is null)
            throw new InvalidOperationException();

        return (T)Create(type, operation);
    }

    public object CreateInstance(SyntaxNode syntax, Compilation compilation)
    {
        var model = compilation.GetSemanticModel(syntax.SyntaxTree);
        var operation = model.GetOperation(syntax);

        if (operation is null)
            throw new InvalidOperationException();

        if (operation.Type is null)
            throw new InvalidOperationException();

        var typeName = operation.Type.GetFullyQualifiedName();

        var type = _types.SingleOrDefault(x => typeName.Equals(x.AssemblyQualifiedName));

        if (type is null)
            throw new InvalidOperationException();

        return Create(type, operation);
    }

    private object Create(Type type, IOperation operation)
    {
        var instance = operation is IObjectCreationOperation creationOperation
            ? Activator.CreateInstance(type, GetParameters(creationOperation))
            : Activator.CreateInstance(type);

        IEnumerable<Action<object>> assignmentOperations = operation
            .Descendants()
            .OfType<ISimpleAssignmentOperation>()
            .Select(x => GetAssignmentAction(type, x));

        foreach (Action<object> action in assignmentOperations)
        {
            action.Invoke(instance);
        }

        return instance;
    }

    private object?[] GetParameters(IObjectCreationOperation operation)
    {
        if (operation.Arguments.Any(x => x.Value is not ILiteralOperation))
            throw new InvalidOperationException();

        return operation.Arguments.Select(x => x.Value.ConstantValue.Value).ToArray();
    }

    private Action<object> GetAssignmentAction(Type type, IAssignmentOperation operation)
    {
        if (operation.Target is not IMemberReferenceOperation memberReferenceOperation)
            throw new NotSupportedException();

        var memberName = memberReferenceOperation.Member.Name;
        var key = new TypeMemberKey(type, memberName);

        Func<object?, Action<object>> action = _assignmentCache.GetOrAdd(key, GetAssignmentFunction);

        var value = operation.Value.ConstantValue.HasValue
            ? operation.Value.ConstantValue.Value
            : _fallback?.ProduceUnknownAssignmentValue(key, operation.Value);

        if (value is null && memberReferenceOperation.MemberHasNullableType() is false)
            throw new ArgumentException($"Could not find argument for {key}");

        return action.Invoke(value);
    }

    private static Func<object?, Action<object>> GetAssignmentFunction(TypeMemberKey key)
    {
        var member = key.Type.GetMember(key.MemberName).SingleOrDefault();

        Action<object, object?> action = member switch
        {
            PropertyInfo property => CreateMethodForProperty(key, property),
            FieldInfo field => CreateMethodForField(key, field),
            _ => throw new NotSupportedException(),
        };

        return arg => instance => action.Invoke(instance, arg);
    }

    private static Action<object, object?> CreateMethodForProperty(TypeMemberKey key, PropertyInfo info)
    {
        Emit<Action<object, object?>> emit = Emit<Action<object, object?>>
            .NewDynamicMethod(info.SetMethod.Name)
            .LoadArgument(0);

        emit = key.Type.IsValueType
            ? emit.Unbox(key.Type)
            : emit.CastClass(key.Type);

        emit = emit.LoadArgument(1);

        emit = info.PropertyType.IsValueType
            ? emit.UnboxAny(info.PropertyType)
            : emit.CastClass(info.PropertyType);

        return emit.Call(info.SetMethod).Return().CreateDelegate();
    }

    private static Action<object, object?> CreateMethodForField(TypeMemberKey key, FieldInfo info)
    {
        Emit<Action<object, object?>> emit = Emit<Action<object, object?>>
            .NewDynamicMethod()
            .LoadArgument(0);

        emit = key.Type.IsValueType
            ? emit.Unbox(key.Type)
            : emit.CastClass(key.Type);

        emit = emit.LoadArgument(1);

        emit = info.FieldType.IsValueType
            ? emit.UnboxAny(info.FieldType)
            : emit.CastClass(info.FieldType);

        return emit.StoreField(info).Return().CreateDelegate();
    }
}