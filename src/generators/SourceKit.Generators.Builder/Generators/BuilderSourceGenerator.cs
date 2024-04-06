using System.Collections;
using FluentChaining;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using SourceKit.Extensions;
using SourceKit.Generators.Builder.Builders.BuilderTypeBuilders;
using SourceKit.Generators.Builder.Builders.FileBuilders;
using SourceKit.Generators.Builder.Builders.TypeBuilders;
using SourceKit.Generators.Builder.Builders.UsingBuilders;
using SourceKit.Generators.Builder.Commands;
using SourceKit.Generators.Builder.Models;
using SourceKit.Generators.Builder.Receivers;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Generators.Builder.Generators;

[Generator]
public class BuilderSourceGenerator : ISourceGenerator
{
    private readonly IChain<FileBuildingCommand, CompilationUnitSyntax> _chain;

    public BuilderSourceGenerator()
    {
        var collection = new ServiceCollection();

        collection.AddFluentChaining(x => x.ChainLifetime = ServiceLifetime.Singleton)
            .AddChain<FileBuildingCommand, CompilationUnitSyntax>
            (
                start => start
                    .Then<UsingBuilder>()
                    .Then<PropertyTypesUsingBuilder>()
                    .Then<TypeBuilder>()
                    .FinishWith((r, _) => r.CompilationUnit)
            )
            .AddChain<TypeBuildingCommand, TypeDeclarationSyntax>
            (
                start => start
                    .Then<BuildMethodTypeBuilder>()
                    .Then<BuilderTypeBuilder>()
                    .FinishWith((r, _) => r.Syntax)
            )
            .AddChain<UsingBuildingCommand, UsingDirectiveSyntax>
            (
                start => start
                    .Then<AutoGeneratedCommentBuilder>()
                    .Then<PragmaBuilder>()
                    .FinishWith((r, _) => r.Syntax)
            )
            .AddChain<BuilderTypeBuildingCommand, TypeDeclarationSyntax>
            (
                start => start
                    .Then<FieldBuilderTypeBuilder>()
                    .Then<ConstructorBuilderTypeBuilder>()
                    .Then<CollectionMethodBuilderTypeBuilder>()
                    .Then<ValueMethodBuilderTypeBuilder>()
                    .Then<BuildMethodBuilderTypeBuilder>()
                    .FinishWith((r, _) => r.BuilderSyntax)
            );

        var provider = collection.BuildServiceProvider();

        _chain = provider.GetRequiredService<IChain<FileBuildingCommand, CompilationUnitSyntax>>();
    }

    public DiagnosticDescriptor FailureDescriptor { get; } = new DiagnosticDescriptor(
        "SK2000",
        "Failed to generate builder",
        "Failed to generate builder. Error: {0}",
        "Usage",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static string GetFileName(string typeName)
    {
        return $"{typeName}.{Constants.FilenameSuffix}";
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new BuilderAttributeSyntaxContextReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        context.CancellationToken.ThrowIfCancellationRequested();

        if (context.SyntaxContextReceiver is not BuilderAttributeSyntaxContextReceiver receiver)
            return;

        foreach (var typeSymbol in receiver.TypeSymbols)
        {
            GenerateForType(context, typeSymbol);
        }
    }

    private void GenerateForType(GeneratorExecutionContext context, INamedTypeSymbol symbol)
    {
        try
        {
            BuilderProperty[] properties = symbol
                .GetMembers()
                .OfType<IPropertySymbol>()
                .Where(x => x.IsImplicitlyDeclared is false)
                .Where(x => x.IsAutoProperty())
                .Select(x => MapToBuilderProperty(symbol, x, context.Compilation))
                .ToArray();

            var fileCommand = new FileBuildingCommand(
                context,
                CompilationUnit(),
                symbol,
                properties);

            var compilationUnit = _chain.Process(fileCommand);
            var fileName = GetFileName(symbol.Name);

            context.AddSource(fileName, compilationUnit.NormalizeWhitespace().ToFullString().Replace("\r\n", "\n"));
        }
        catch (Exception e)
        {
            var diagnostic = Diagnostic.Create(
                FailureDescriptor,
                symbol.GetSignatureLocations().First(),
                e.Message);

            context.ReportDiagnostic(diagnostic);
        }
    }

    private BuilderProperty MapToBuilderProperty(
        INamedTypeSymbol type,
        IPropertySymbol propertySymbol,
        Compilation compilation)
    {
        var propertyType = propertySymbol.Type;
        var enumerableType = compilation.GetTypeSymbol<IEnumerable>();

        var defaultValue = type.Constructors
            .SelectMany(x => x.Parameters)
            .Where(x => x.Name.Equals(propertySymbol.Name))
            .Where(x => x.HasExplicitDefaultValue)
            .Select(x => x.ExplicitDefaultValue)
            .WhereNotNull()
            .SingleOrDefault();

        var literalValue = new LiteralValue(defaultValue);

        if (propertyType is not IArrayTypeSymbol && propertyType.IsAssignableTo(enumerableType) is false)
            return new BuilderProperty.Value(propertySymbol, propertyType, literalValue);

        var stringType = compilation.GetTypeSymbol<string>();

        if (propertyType.Equals(stringType, SymbolEqualityComparer.Default))
            return new BuilderProperty.Value(propertySymbol, propertyType, literalValue);

        var elementType = propertyType.GetEnumerableTypeArgument(compilation);

        if (propertyType is IArrayTypeSymbol)
            return new BuilderProperty.Collection(propertySymbol, propertyType, CollectionKind.Array, elementType);

        var listType = compilation.GetTypeSymbol(typeof(List<>)).Construct(elementType);

        if (propertyType.Equals(listType, SymbolEqualityComparer.Default))
            return new BuilderProperty.Collection(propertySymbol, propertyType, CollectionKind.List, elementType);

        var setType = compilation.GetTypeSymbol(typeof(HashSet<>)).Construct(elementType);

        if (propertyType.Equals(setType, SymbolEqualityComparer.Default))
            return new BuilderProperty.Collection(propertySymbol, propertyType, CollectionKind.HashSet, elementType);

        return new BuilderProperty.Collection(propertySymbol, propertyType, CollectionKind.Array, elementType);
    }
}