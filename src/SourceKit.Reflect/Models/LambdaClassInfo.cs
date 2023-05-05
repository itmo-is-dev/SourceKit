using System.Reflection;

namespace SourceKit.Reflect.Models;

public record LambdaClassInfo(
    FieldInfo DelegateField,
    FieldInfo InstanceField,
    MethodInfo Method,
    Type MethodDelegateType);