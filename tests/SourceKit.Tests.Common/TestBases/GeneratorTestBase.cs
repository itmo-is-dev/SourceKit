using System.Reflection;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;

namespace SourceKit.Tests.Common.TestBases;

public abstract class GeneratorTestBase<TGenerator>
    where TGenerator : IIncrementalGenerator, new()
{
    protected GeneratorTestBuilder GeneratorTest => ConfigureTest(new GeneratorTestBuilder());

    protected virtual GeneratorTestBuilder ConfigureTest(GeneratorTestBuilder test) => test;

    protected DiagnosticResult Diagnostic(DiagnosticDescriptor descriptor) => new(descriptor);

    protected sealed class GeneratorTestBuilder
    {
        private readonly List<SourceFile> _sources = [];
        private readonly List<SourceFile> _generatedSources = [];
        private readonly List<Assembly> _additionalReferences = [];
        private readonly List<DiagnosticResult> _expectedDiagnostics = [];
        private readonly List<string> _disabledDiagnostics = ["CS1591", "CS8019"];

        private CompilerDiagnostics _compilerDiagnostics = CompilerDiagnostics.All;
        private ReferenceAssemblies _referenceAssemblies = ReferenceAssemblies.Net.Net90;

        public GeneratorTestBuilder WithSource(SourceFile file)
        {
            _sources.Add(file);
            return this;
        }

        public GeneratorTestBuilder WithSources(IEnumerable<SourceFile> files)
        {
            _sources.AddRange(files);
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

        public GeneratorTestBuilder WithExpectedDiagnostic(DiagnosticResult diagnostic)
        {
            _expectedDiagnostics.Add(diagnostic);
            return this;
        }

        public GeneratorTestBuilder WithDisabledDiagnostics(params string[] ids)
        {
            _disabledDiagnostics.AddRange(ids);
            return this;
        }

        public GeneratorTestBuilder WithCompilerDiagnostics(CompilerDiagnostics compilerDiagnostics)
        {
            _compilerDiagnostics = compilerDiagnostics;
            return this;
        }

        public CSharpSourceGeneratorTest<TGenerator, DefaultVerifier> Build()
        {
            var test = new CSharpSourceGeneratorTest<TGenerator, DefaultVerifier>
            {
                ReferenceAssemblies = _referenceAssemblies,
                SolutionTransforms =
                {
                    (solution, projectId) =>
                    {
                        Project project = solution.GetProject(projectId)!;

                        if (project.CompilationOptions is not CSharpCompilationOptions options)
                            return solution;

                        project = project.WithCompilationOptions(
                            options.WithNullableContextOptions(NullableContextOptions.Enable));

                        return project.Solution;
                    },
                },
            };

            foreach (SourceFile source in _sources)
                test.TestState.Sources.Add(source.AsTestSource());

            foreach (SourceFile source in _generatedSources)
                test.TestState.GeneratedSources.Add(source.AsGeneratorSource<TGenerator>());

            foreach (Assembly assembly in _additionalReferences)
                test.TestState.AdditionalReferences.Add(assembly);

            test.TestState.ExpectedDiagnostics.AddRange(_expectedDiagnostics);
            test.DisabledDiagnostics.AddRange(_disabledDiagnostics);
            test.CompilerDiagnostics = _compilerDiagnostics;

            return test;
        }
    }
}

public static class SourceGeneratorTestExtensions
{
    public static Task RunWithCacheVerificationAsync<TGenerator>(
        this CSharpSourceGeneratorTest<TGenerator, DefaultVerifier> test,
        params SourceFile[] modifiedSources)
        where TGenerator : IIncrementalGenerator, new()
    {
        return test.RunWithCacheVerificationAsync(trackedSteps: [], modifiedSources);
    }

    public static Task RunWithTrackedStepsCacheVerificationAsync<TGenerator>(
        this CSharpSourceGeneratorTest<TGenerator, DefaultVerifier> test,
        string[] trackedSteps,
        params SourceFile[] modifiedSources)
        where TGenerator : IIncrementalGenerator, new()
    {
        return test.RunWithCacheVerificationAsync(trackedSteps, modifiedSources);
    }

    private static async Task RunWithCacheVerificationAsync<TGenerator>(
        this CSharpSourceGeneratorTest<TGenerator, DefaultVerifier> test,
        string[] trackedSteps,
        SourceFile[] modifiedSources)
        where TGenerator : IIncrementalGenerator, new()
    {
        await test.RunAsync();

        var compilation = CSharpCompilation.Create(
            "TestAssembly",
            test.TestState.Sources.Select(source => CSharpSyntaxTree.ParseText(source.content)),
            test.TestState.AdditionalReferences,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary).WithNullableContextOptions(NullableContextOptions.Enable));

        IEnumerable<SyntaxTree> cloneSources = test.TestState.Sources
            .Where(source => modifiedSources.Any(modifiedSource => modifiedSource.Name == source.filename) is false)
            .Concat(modifiedSources.Select(source => source.AsTestSource()))
            .Select(source => CSharpSyntaxTree.ParseText(source.Item2));

        var compilationClone = CSharpCompilation.Create(
            "TestAssembly",
            cloneSources,
            test.TestState.AdditionalReferences,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary).WithNullableContextOptions(NullableContextOptions.Enable));

        var driverOptions = new GeneratorDriverOptions(
            disabledOutputs: IncrementalGeneratorOutputKind.None,
            trackIncrementalGeneratorSteps: true);

        var driver = CSharpGeneratorDriver.Create([new TGenerator().AsSourceGenerator()], driverOptions: driverOptions);

        GeneratorDriver dirtyDriver = driver.RunGenerators(compilation);

        GeneratorDriver incrementedDriver = dirtyDriver.RunGenerators(compilationClone);
        GeneratorDriverRunResult incrementedResult = incrementedDriver.GetRunResult();

        IEnumerable<(IncrementalGeneratorRunStep step, object Value, IncrementalStepRunReason Reason)> outputs = incrementedResult.Results[0]
            .TrackedOutputSteps
            .SelectMany(step => step.Value)
            .Where(step => trackedSteps is [] || trackedSteps.Contains(step.Name))
            .SelectMany(step => step.Outputs, (step, output) => (step, output.Value, output.Reason));

        foreach (var (step, value, reason) in outputs)
        {
            reason
                .Should()
                .BeOneOf(
                    [IncrementalStepRunReason.Cached, IncrementalStepRunReason.Unchanged],
                    "Step '{0}' had unexpected reason {1}, value = {2}",
                    step.Name,
                    reason,
                    value);
        }
    }
}
