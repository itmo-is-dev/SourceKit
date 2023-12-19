using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using SourceKit.Extensions;

namespace SourceKit.Analyzers.Enumerable.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class OfTypeMustUseDerivedTypeAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "SK1300";
    public const string Title = nameof(OfTypeMustUseDerivedTypeAnalyzer);

    public const string Format = """Type {0} is not derived from type {1}""";

    public static readonly DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(
        DiagnosticId,
        Title,
        Format,
        "Usage",
        DiagnosticSeverity.Error,
        true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(Descriptor);

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.RegisterOperationAction(HandleOperation, OperationKind.Invocation);
    }

    private void HandleOperation(OperationAnalysisContext context)
    {
        var operation = (IInvocationOperation)context.Operation;

        if (operation.TargetMethod.Name is not "OfType")
            return;

        var enumerableStaticSymbol = context.Compilation.GetTypeSymbol(typeof(System.Linq.Enumerable));
        var containingType = operation.TargetMethod.ContainingType;

        if (containingType.Equals(enumerableStaticSymbol, SymbolEqualityComparer.Default) is false)
            return;

        var argument = operation.Arguments.Single();

        var value = argument.Value is IConversionOperation conversion
            ? conversion.Operand
            : argument.Value;

        if (value.Type is not INamedTypeSymbol namedSourceType
            || operation.Type is not INamedTypeSymbol namedReturnType)
        {
            return;
        }

        var sourceElementType = namedSourceType.TypeArguments.SingleOrDefault();
        var returnElementType = namedReturnType.TypeArguments.Single();

        if (sourceElementType is null)
            return;

        if (returnElementType.IsAssignableTo(sourceElementType))
            return;

        if (sourceElementType is ITypeParameterSymbol parameterSymbol &&
            parameterSymbol.ConstraintTypes.Any(c => returnElementType.IsAssignableTo(c)))
        {
            return;
        }

        var diagnostic = Diagnostic.Create(
            Descriptor,
            operation.Syntax.GetLocation(),
            returnElementType.Name,
            sourceElementType.Name);

        context.ReportDiagnostic(diagnostic);
    }
}