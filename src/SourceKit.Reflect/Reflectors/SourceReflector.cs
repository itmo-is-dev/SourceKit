using System.Reflection;
using Microsoft.CodeAnalysis;
using SourceKit.Reflect.Reflectors.Instance;
using SourceKit.Reflect.Reflectors.Method;

namespace SourceKit.Reflect.Reflectors;

public class SourceReflector : ISourceReflector
{
    private readonly InstanceReflector _instanceReflector;
    private readonly MethodReflector _methodReflector;

    private SourceReflector(IReadOnlyCollection<Type> types, IInstanceReflectorFallback? instanceReflectorFallback)
    {
        var typeProvider = new TypeResolver(types);

        _instanceReflector = new InstanceReflector(types, instanceReflectorFallback);
        _methodReflector = new MethodReflector(typeProvider);
    }

    public static SourceReflectorBuilder Builder => new SourceReflectorBuilder();

    public T CreateInstance<T>(SyntaxNode syntax, Compilation compilation)
        => _instanceReflector.CreateInstance<T>(syntax, compilation);

    public object CreateInstance(SyntaxNode syntax, Compilation compilation)
        => _instanceReflector.CreateInstance(syntax, compilation);

    public T CreateMethod<T>(SyntaxNode syntax, Compilation compilation) where T : Delegate
        => _methodReflector.CreateMethod<T>(syntax, compilation);

    public class SourceReflectorBuilder
    {
        private readonly HashSet<Type> _types;
        private IInstanceReflectorFallback? _instanceReflectorFallback;

        public SourceReflectorBuilder()
        {
            _types = new HashSet<Type>();

            WithReferencedAssembly(typeof(int).Assembly);
            WithReferencedAssembly(typeof(System.Runtime.CompilerServices.Unsafe).Assembly);
        }

        public SourceReflectorBuilder WithReferencedType(Type type)
        {
            _types.Add(type);
            return this;
        }

        public SourceReflectorBuilder WithReferencedAssembly(Assembly assembly)
        {
            foreach (var type in assembly.DefinedTypes)
            {
                _types.Add(type);
            }

            return this;
        }

        public SourceReflectorBuilder WithInstanceFallback(IInstanceReflectorFallback fallback)
        {
            _instanceReflectorFallback = fallback;
            return this;
        }

        public SourceReflector Build()
            => new SourceReflector(_types.ToArray(), _instanceReflectorFallback);
    }
}