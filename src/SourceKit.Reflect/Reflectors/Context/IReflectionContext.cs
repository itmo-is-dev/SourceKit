using System.Reflection.Emit;
using Microsoft.CodeAnalysis;
using SourceKit.Reflect.Models;

namespace SourceKit.Reflect.Reflectors.Context;

public interface IReflectionContext
{
    TypeBuilder PushType(string name);

    Type PopType();

    TypeBuilder PeekType();

    void RegisterLambda(IMethodSymbol methodSymbol, LambdaClassInfo info);

    LambdaClassInfo ResolveLambda(IMethodSymbol methodSymbol);
}