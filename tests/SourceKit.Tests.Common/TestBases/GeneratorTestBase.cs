using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;

namespace SourceKit.Tests.Common.TestBases;

public abstract class GeneratorTestBase<TGenerator>
    where TGenerator : ISourceGenerator, new()
{
    protected GeneratorTestBuilder GeneratorTest => new();
    
    protected sealed class GeneratorTestBuilder
    {
        private readonly List<SourceFile> _sources = [];
        private readonly List<SourceFile> _generatedSources = [];
        private readonly List<Assembly> _additionalReferences = [];
        private ReferenceAssemblies _referenceAssemblies = ReferenceAssemblies.Net.Net60;

        public GeneratorTestBuilder WithSource(SourceFile file)
        {
            _sources.Add(file);
            return this;
        }

        public GeneratorTestBuilder WithGeneratedSource(SourceFile file)
        {
            _generatedSources.Add(file);
            return this;
        }

        public GeneratorTestBuilder WithAdditionalReference(Assembly assembly)
        {
            _additionalReferences.Add(assembly);
            return this;
        }

        public GeneratorTestBuilder WithReferenceAssemblies(ReferenceAssemblies assemblies)
        {
            _referenceAssemblies = assemblies;
            return this;
        }

        public CSharpSourceGeneratorTest<TGenerator, XUnitVerifier> Build()
        {
            var test = new CSharpSourceGeneratorTest<TGenerator, XUnitVerifier>
            {
                ReferenceAssemblies = _referenceAssemblies,
            };

            foreach (SourceFile source in _sources)
                test.TestState.Sources.Add(source);

            foreach (SourceFile source in _generatedSources)
                test.TestState.GeneratedSources.Add(source);

            foreach (Assembly assembly in _additionalReferences)
                test.TestState.AdditionalReferences.Add(assembly);

            return test;
        }
    }
}