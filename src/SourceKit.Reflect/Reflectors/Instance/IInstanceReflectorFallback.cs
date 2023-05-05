using Microsoft.CodeAnalysis;
using SourceKit.Reflect.Models;

namespace SourceKit.Reflect.Reflectors.Instance;

public interface IInstanceReflectorFallback
{
    object? ProduceUnknownAssignmentValue(TypeMemberKey typeMemberKey, IOperation valueOperation);
}