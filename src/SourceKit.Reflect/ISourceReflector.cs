using Microsoft.CodeAnalysis;

namespace SourceKit.Reflect;

public interface ISourceReflector
{
    T CreateInstance<T>(SyntaxNode syntax, Compilation compilation);

    object CreateInstance(SyntaxNode syntax, Compilation compilation);

    T CreateMethod<T>(SyntaxNode syntax, Compilation compilation) where T : Delegate;
}