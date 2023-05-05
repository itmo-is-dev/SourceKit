using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;
using Sigil.NonGeneric;
using SourceKit.Reflect.Extensions;
using SourceKit.Reflect.Reflectors.Context;

namespace SourceKit.Reflect.Reflectors.Method;

internal partial class OperationCompiler : OperationVisitor
{
    private readonly Emit _emit;
    private readonly ITypeResolver _typeResolver;
    private readonly MethodReflector _methodReflector;
    private readonly Compilation _compilation;
    private readonly IReflectionContext _context;
    private readonly StoreVisitor _storeVisitor;

    public OperationCompiler(
        Emit emit,
        ITypeResolver typeResolver,
        MethodReflector methodReflector,
        Compilation compilation,
        IReflectionContext context)
    {
        _emit = emit;
        _typeResolver = typeResolver;
        _methodReflector = methodReflector;
        _compilation = compilation;
        _context = context;

        _storeVisitor = new StoreVisitor(emit, typeResolver);
    }

    public override void VisitDelegateCreation(IDelegateCreationOperation operation)
    {
        var functionOperation = operation.ChildOperations
            .OfType<IAnonymousFunctionOperation>()
            .Single();

        var info = _context.ResolveLambda(functionOperation.Symbol);

        var propertyGetMethodReturnLabel = _emit.Operation(x => x.DefineLabel());

        _emit.Operation(x => x
            .LoadField(info.DelegateField)
            .Duplicate()
            .BranchIfTrue(propertyGetMethodReturnLabel)
            .Pop()
            .LoadField(info.InstanceField)
            .LoadFunctionPointer(info.Method)
            .NewObject(info.MethodDelegateType, typeof(object), typeof(IntPtr))
            .Duplicate()
            .StoreField(info.DelegateField)
            .MarkLabel(propertyGetMethodReturnLabel));
    }

    public override void DefaultVisit(IOperation operation)
    {
        foreach (var childOperation in operation.ChildOperations)
        {
            childOperation.Accept(this);
        }
    }

    public override void VisitReturn(IReturnOperation operation)
    {
        operation.ReturnedValue?.Accept(this);
        _emit.Operation(x => x.Return());
    }

    public override void VisitConversion(IConversionOperation operation)
    {
        operation.Operand.Accept(this);

        if (operation.Type is null)
            return;

        var type = _typeResolver.FindType(operation.Type);

        if (type is not { IsPrimitive: true })
            return;

        _emit.Operation(x => x.Convert(type));
    }

    public override void VisitVariableDeclarator(IVariableDeclaratorOperation operation)
    {
        var typeSymbol = operation.Initializer?.Value.Type;

        if (typeSymbol is null)
            return;

        var type = _typeResolver.FindType(typeSymbol);

        if (type is null)
            return;

        _emit.Operation(x => x.DeclareLocal(type, operation.Symbol.Name));

        foreach (var childOperation in operation.ChildOperations)
        {
            childOperation.Accept(this);
        }

        _emit.Operation(x => x.StoreLocal(operation.Symbol.Name));
    }

    public override void VisitLiteral(ILiteralOperation operation)
    {
        Action<Emit> action = operation.ConstantValue.Value switch
        {
            bool o => x => x.LoadConstant(o),
            int o => x => x.LoadConstant(o),
            uint o => x => x.LoadConstant(o),
            long o => x => x.LoadConstant(o),
            ulong o => x => x.LoadConstant(o),
            float o => x => x.LoadConstant(o),
            double o => x => x.LoadConstant(o),
            string o => x => x.LoadConstant(o),
            _ => throw new ArgumentOutOfRangeException(),
        };

        _emit.Operation(action);
    }

    public override void VisitParameterReference(IParameterReferenceOperation operation)
    {
        var index = operation.Parameter.GetIndex();
        _emit.Operation(x => x.LoadArgument(index));
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

        var declaringSymbol = operation.Property.ContainingType;

        var shouldCallVirtual =
            declaringSymbol.IsAbstract ||
            declaringSymbol.IsSealed is false ||
            operation.Property.IsVirtual;

        Action<Emit> action = shouldCallVirtual
            ? x => x.CallVirtual(property.GetMethod)
            : x => x.Call(property.GetMethod);

        _emit.Operation(action);
    }

    public override void VisitFieldReference(IFieldReferenceOperation operation)
    {
        var type = _typeResolver.GetType(operation.Instance?.Type);

        var field = type.GetField(
            operation.Field.Name,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!;

        _emit.Operation(x => x.LoadField(field));
    }

    public override void VisitLocalReference(ILocalReferenceOperation operation)
    {
        _emit.Operation(x => x.LoadLocal(operation.Local.Name));
    }

    public override void VisitBinaryOperator(IBinaryOperation operation)
    {
        operation.LeftOperand.Accept(this);
        operation.RightOperand.Accept(this);

        Action<Emit> action = MapBinaryOperation(operation.OperatorKind);

        _emit.Operation(action);
    }

    public override void VisitCompoundAssignment(ICompoundAssignmentOperation operation)
    {
        operation.Target.Accept(this);
        operation.Value.Accept(this);

        Action<Emit> action = MapBinaryOperation(operation.OperatorKind);
        _emit.Operation(action);

        operation.Target.Accept(_storeVisitor);
    }

    public override void VisitArrayCreation(IArrayCreationOperation operation)
    {
        if (operation.Type is not IArrayTypeSymbol arrayTypeSymbol)
            throw new NotSupportedException($"Array type {operation.Type} is invalid array type");

        var type = _typeResolver.GetType(arrayTypeSymbol);
        var elementType = _typeResolver.GetType(arrayTypeSymbol.ElementType);

        foreach (var size in operation.DimensionSizes)
        {
            size.Accept(this);
        }

        Type[] constructorParameterType = operation.DimensionSizes.Select(_ => typeof(int)).ToArray();
        _emit.Operation(x => x.NewObject(type, constructorParameterType));

        if (operation.Initializer is not null)
        {
            IEnumerable<(IOperation x, int i)> initializerOperations = operation.Initializer.ChildOperations
                .Select((x, i) => (x, i));

            foreach (var (initializerOperation, index) in initializerOperations)
            {
                _emit.Operation(x => x.Duplicate());
                _emit.Operation(x => x.LoadConstant(index));
                initializerOperation.Accept(this);

                _emit.Operation(x => x.StoreElement(elementType));
            }
        }
    }

    private static Action<Emit> MapBinaryOperation(BinaryOperatorKind kind)
    {
        Action<Emit> action = kind switch
        {
            BinaryOperatorKind.Add => x => x.Add(),
            BinaryOperatorKind.Subtract => x => x.Subtract(),
            BinaryOperatorKind.Multiply => x => x.Multiply(),
            BinaryOperatorKind.Divide => x => x.Divide(),
            BinaryOperatorKind.Remainder => x => x.Remainder(),
            BinaryOperatorKind.LeftShift => x => x.ShiftLeft(),
            BinaryOperatorKind.RightShift => x => x.ShiftRight(),
            BinaryOperatorKind.And => x => x.And(),
            BinaryOperatorKind.Or => x => x.Or(),
            BinaryOperatorKind.ExclusiveOr => x => x.Xor(),
            BinaryOperatorKind.ConditionalAnd => x => x.And(),
            BinaryOperatorKind.ConditionalOr => x => x.Or(),
            BinaryOperatorKind.Equals => x => x.CompareEqual(),
            BinaryOperatorKind.NotEquals => x => x.CompareEqual().Negate(),
            BinaryOperatorKind.LessThan => x => x.CompareLessThan(),
            BinaryOperatorKind.LessThanOrEqual => x => x.CompareGreaterThan().Negate(),
            BinaryOperatorKind.GreaterThanOrEqual => x => x.CompareLessThan().Negate(),
            BinaryOperatorKind.GreaterThan => x => x.CompareGreaterThan(),
            _ => throw new NotSupportedException($"Operation {kind} is not supported"),
        };
        return action;
    }
}