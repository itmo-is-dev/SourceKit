using System.Reflection;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;

namespace SourceKit.Tests.Common.TestBases;

public abstract class CodeFixTestBase<TAnalyzer, TCodeFixProvider> : AnalyzerTestBase<TAnalyzer>
    where TAnalyzer : DiagnosticAnalyzer, new()
    where TCodeFixProvider : CodeFixProvider, new()
{
    protected CodeFixTestBuilder CodeFixTest => new();

    protected sealed class CodeFixTestBuilder
    {
        private readonly List<SourceFile> _sources = [];
        private readonly List<Assembly> _additionalReferences = [];
        private readonly List<SourceFile> _fixedSources = [];
        private readonly List<DiagnosticResult> _expectedDiagnostics = [];
        private readonly List<string> _disabledDiagnostics = [];
        private int? _codeFixIterationNumber;

        public CodeFixTestBuilder WithSource(SourceFile file)
        {
            _sources.Add(file);
            return this;
        }

        public CodeFixTestBuilder WithAdditionalReference(Assembly assembly)
        {
            _additionalReferences.Add(assembly);
            return this;
        }

        public CodeFixTestBuilder WithFixedSource(SourceFile file)
        {
            _fixedSources.Add(file);
            return this;
        }

        public CodeFixTestBuilder WithExpectedDiagnostic(DiagnosticResult diagnostic)
        {
            _expectedDiagnostics.Add(diagnostic);
            return this;
        }

        public CodeFixTestBuilder WithDisabledDiagnostic(string diagnostic)
        {
            _disabledDiagnostics.Add(diagnostic);
            return this;
        }

        public CodeFixTestBuilder WithIterationNumber(int value)
        {
            _codeFixIterationNumber = value;
            return this;
        }

        public CSharpCodeFixTest<TAnalyzer, TCodeFixProvider, XUnitVerifier> Build()
        {
            var test = new CSharpCodeFixTest<TAnalyzer, TCodeFixProvider, XUnitVerifier>();

            foreach (SourceFile source in _sources)
                test.TestState.Sources.Add(source);

            foreach (Assembly assembly in _additionalReferences)
            {
                test.TestState.AdditionalReferences.Add(assembly);
                test.FixedState.AdditionalReferences.Add(assembly);
            }

            foreach (SourceFile source in _fixedSources)
                test.FixedState.Sources.Add(source);

            foreach (DiagnosticResult diagnostic in _expectedDiagnostics)
                test.ExpectedDiagnostics.Add(diagnostic);

            foreach (string diagnostic in _disabledDiagnostics)
                test.DisabledDiagnostics.Add(diagnostic);

            test.NumberOfFixAllIterations = _codeFixIterationNumber;
            test.NumberOfFixAllInProjectIterations = _codeFixIterationNumber;
            test.NumberOfFixAllInDocumentIterations = _codeFixIterationNumber;

            return test;
        }
    }
}