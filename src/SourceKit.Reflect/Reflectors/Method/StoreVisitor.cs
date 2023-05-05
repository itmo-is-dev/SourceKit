using System.Reflection;
using Microsoft.CodeAnalysis.Operations;
using Sigil.NonGeneric;
using SourceKit.Reflect.Extensions;

namespace SourceKit.Reflect.Reflectors.Method;

public class StoreVisitor : OperationVisitor
{
    private readonly Emit _emit;
    private readonly ITypeResolver _typeResolver;

    public StoreVisitor(Emit emit, ITypeResolver typeResolver)
    {
        _emit = emit;
        _typeResolver = typeResolver;
    }

    public override void VisitParameterReference(IParameterReferenceOperation operation)
    {
        var index = operation.Parameter.GetIndex();
        _emit.Operation(x => x.StoreArgument(index));
    }

    public override void VisitFieldReference(IFieldReferenceOperation operation)
    {
        var type = _typeResolver.GetType(operation.Instance?.Type);

        var field = type.GetField(
            operation.Field.Name,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!;

        _emit.Operation(x => x.StoreField(field));
    }

    public override void VisitPropertyReference(IPropertyReferenceOperation operation)
    {
        if (operation.Instance is null)
            return;

        operation.Instance.Accept(this);

        var type = _typeResolver.GetType(operation.Instance.Type);

        var property = type.GetProperty(
            operation.Property.Name,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!;

        Action<Emit> action = operation.Property.IsVirtual
            ? x => x.CallVirtual(property.SetMethod)
            : x => x.Call(property.SetMethod);

        _emit.Operation(action);
    }

    public override void VisitLocalReference(ILocalReferenceOperation operation)
    {
        _emit.Operation(x => x.StoreLocal(operation.Local.Name));
    }
}