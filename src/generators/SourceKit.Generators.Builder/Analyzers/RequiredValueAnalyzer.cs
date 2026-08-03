using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using SourceKit.Extensions;
using SourceKit.Generators.Builder.Annotations;

namespace SourceKit.Generators.Builder.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class RequiredValueAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "SK2100";
    public const string Title = nameof(RequiredValueAnalyzer);

    public const string Format = """Reqired properties {0} must be initialized""";

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
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze |
                                               GeneratedCodeAnalysisFlags.ReportDiagnostics);

        context.RegisterOperationAction(AnalyzeOperation, OperationKind.Invocation);
    }

    private void AnalyzeOperation(OperationAnalysisContext context)
    {
        var operation = (IInvocationOperation)context.Operation;

        if (operation.Instance is not null)
            return;

        INamedTypeSymbol? generateBuilderAttribute = context.Compilation.FindTypeSymbol<GenerateBuilderAttribute>();
        INamedTypeSymbol? requiredValueAttribute = context.Compilation.FindTypeSymbol<RequiredValueAttribute>();
        INamedTypeSymbol? initializesValueAttribute = context.Compilation.FindTypeSymbol<InitializesPropertyAttribute>();
        INamedTypeSymbol? builderPropertyAttribute = context.Compilation.FindTypeSymbol<BuilderPropertyAttribute>();

        if (generateBuilderAttribute is null
            || requiredValueAttribute is null
            || initializesValueAttribute is null
            || builderPropertyAttribute is null)
        {
            return;
        }

        if (operation.Type is not INamedTypeSymbol modelType)
            return;

        bool hasBuilderAttribute = operation.Type
            .GetAttributes()
            .Any(x => x.AttributeClass?.Equals(generateBuilderAttribute, SymbolEqualityComparer.Default) is true);

        if (hasBuilderAttribute is false)
            return;

        if (operation.TargetMethod.Name is not "Build")
            return;

        ImmutableArray<ISymbol> modelTypeMembers = modelType.GetMembers();

        bool IsRequiredAttribute(AttributeData attributeData)
        {
            if (attributeData.IsAttribute(requiredValueAttribute))
                return true;

            if (attributeData.IsAttribute(builderPropertyAttribute) is false)
                return false;

            return ((BuilderPropertyOptions)(int)attributeData.ConstructorArguments[0].Value!).HasFlag(BuilderPropertyOptions.Required);
        }

        IEnumerable<string> requiredProperties = modelTypeMembers
            .OfType<IPropertySymbol>()
            .Where(property => property.GetAttributes().Any(IsRequiredAttribute))
            .Select(x => x.Name);

        IEnumerable<string> requiredParameters = modelType.Constructors
            .SelectMany(x => x.Parameters)
            .Where(x => x.GetAttributes().Any(IsRequiredAttribute))
            .Select(x => x.Name);

        IEnumerable<IInvocationOperation> descendantInvocations = operation.Descendants()
            .OfType<IInvocationOperation>();

        IEnumerable<string> initializedPropertyNames = GetInitializedPropertyNames(descendantInvocations);

        var unintializedPropertyNames = requiredProperties
            .Union(requiredParameters)
            .Except(initializedPropertyNames)
            .ToArray();

        if (unintializedPropertyNames is [])
            return;

        var location = operation.Syntax.GetLocation();

        context.ReportDiagnostic(
            Diagnostic.Create(Descriptor, location, messageArgs: string.Join(", ", unintializedPropertyNames)));

        return;

        IEnumerable<string> GetInitializedPropertyNames(IEnumerable<IInvocationOperation> invocations)
        {
            foreach (var invocation in invocations)
            {
                var attribute = invocation.TargetMethod
                    .GetAttributes()
                    .SingleOrDefault(x => x.IsAttribute(initializesValueAttribute));

                if (attribute?.ConstructorArguments is [{ Value: string parameterName }])
                    yield return parameterName;
            }
        }
    }
}
