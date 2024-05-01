using System.Collections.Immutable;
using System.Reflection;
using System.Runtime;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.CSharp.Testing.XUnit;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Microsoft.CodeAnalysis.Text;

namespace SourceKit.Tests.Common.TestBases;

public abstract class AnalyzerTestBase<TAnalyzer>
    where TAnalyzer : DiagnosticAnalyzer, new()
{
    protected Verifier AnalyzerVerifier { get; } = new();

    protected AnalyzerTestBuilder AnalyzerTest => new();

    protected sealed class Verifier
    {
        public DiagnosticResult Diagnostic(DiagnosticDescriptor descriptor)
        {
            return AnalyzerVerifier<TAnalyzer>.Diagnostic(descriptor);
        }
    }

    protected sealed class AnalyzerTestBuilder
    {
        private readonly List<SourceFile> _sources = [];
        private readonly List<Assembly> _additionalReferences = [];
        private readonly List<DiagnosticResult> _expectedDiagnostics = [];
        private readonly List<ISourceGenerator> _sourceGenerators = [];
        private readonly ReferenceAssemblies _referenceAssemblies = ReferenceAssemblies.Net.Net60;

        public AnalyzerTestBuilder WithSource(SourceFile file)
        {
            _sources.Add(file);
            return this;
        }

        public AnalyzerTestBuilder WithAdditionalReference(Assembly assembly)
        {
            _additionalReferences.Add(assembly);
            return this;
        }

        public AnalyzerTestBuilder WithExpectedDiagnostic(DiagnosticResult diagnostic)
        {
            _expectedDiagnostics.Add(diagnostic);
            return this;
        }

        public AnalyzerTestBuilder WithSourceGenerator(ISourceGenerator generator)
        {
            _sourceGenerators.Add(generator);
            return this;
        }

        public CSharpAnalyzerTest<TAnalyzer, XUnitVerifier> Build()
        {
            var test = new CSharpAnalyzerTest<TAnalyzer, XUnitVerifier>
            {
                ReferenceAssemblies = _referenceAssemblies,
            };

            foreach (SourceFile source in _sources)
                test.TestState.Sources.Add(source);

            foreach (Assembly assembly in _additionalReferences)
                test.TestState.AdditionalReferences.Add(assembly);

            foreach (DiagnosticResult diagnostic in _expectedDiagnostics)
                test.ExpectedDiagnostics.Add(diagnostic);

            AddGeneratedSources(test);

            return test;
        }

        private void AddGeneratedSources(CSharpAnalyzerTest<TAnalyzer, XUnitVerifier> test)
        {
            if (_sourceGenerators is [])
                return;

            var workspace = new AdhocWorkspace();
            _ = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions);

            Solution solution = workspace.CurrentSolution;
            var projectId = ProjectId.CreateNewId();

            solution = solution.AddProject(
                projectId,
                "project",
                "project",
                LanguageNames.CSharp);

            solution = _sources.Aggregate(
                solution,
                (current, source) => current.AddDocument(
                    DocumentId.CreateNewId(projectId),
                    name: source.Name,
                    text: source.Content));

            IEnumerable<MetadataReference> metadataReferences = test.TestState.AdditionalReferences
                .Concat(_referenceAssemblies.ResolveAsync(test.Language, default).GetAwaiter().GetResult())
                .DistinctBy(x => x.Display);

            Project project = solution.GetProject(projectId)!;
            project = project.WithCompilationOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            project = project.WithMetadataReferences(metadataReferences);

            if (workspace.TryApplyChanges(project.Solution) is false)
                throw new InvalidOperationException("Failed to apply changes to workspace");

            Compilation? compilation = workspace.CurrentSolution.Projects
                .Single()
                .GetCompilationAsync()
                .GetAwaiter()
                .GetResult();

            if (compilation is null)
                throw new InvalidOperationException("Failed to get compilation");

            GeneratorDriver driver = CSharpGeneratorDriver.Create(_sourceGenerators);

            driver = driver.RunGeneratorsAndUpdateCompilation(
                compilation,
                out _,
                out _);

            GeneratorDriverRunResult result = driver.GetRunResult();

            IEnumerable<(string HintName, SourceText SourceText)> generatedSources = result.Results
                .SelectMany(x => x.GeneratedSources)
                .Select(x => (x.HintName, x.SourceText));

            test.TestState.Sources.AddRange(generatedSources);
        }
    }
}