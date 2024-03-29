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
using SourceKit.Generators.Builder.Tools;
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
                .Select(x => MapToBuilderProperty(x, context.Compilation))
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

    private BuilderProperty MapToBuilderProperty(IPropertySymbol propertySymbol, Compilation compilation)
    {
        var type = propertySymbol.Type;
        var enumerableType = compilation.GetTypeSymbol<IEnumerable>();

        if (type is not IArrayTypeSymbol && type.IsAssignableTo(enumerableType) is false)
            return new BuilderProperty.Value(propertySymbol, type);

        var stringType = compilation.GetTypeSymbol<string>();

        if (type.Equals(stringType, SymbolEqualityComparer.Default))
            return new BuilderProperty.Value(propertySymbol, type);

        var elementType = type.GetEnumerableTypeArgument(compilation);

        if (type is IArrayTypeSymbol)
            return new BuilderProperty.Collection(propertySymbol, type, CollectionKind.Array, elementType);

        var listType = compilation.GetTypeSymbol(typeof(List<>)).Construct(elementType);

        if (type.Equals(listType, SymbolEqualityComparer.Default))
            return new BuilderProperty.Collection(propertySymbol, type, CollectionKind.List, elementType);

        var setType = compilation.GetTypeSymbol(typeof(HashSet<>)).Construct(elementType);

        if (type.Equals(setType, SymbolEqualityComparer.Default))
            return new BuilderProperty.Collection(propertySymbol, type, CollectionKind.HashSet, elementType);

        return new BuilderProperty.Collection(propertySymbol, type, CollectionKind.Array, elementType);
    }
}