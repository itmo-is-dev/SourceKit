using FluentChaining;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using SourceKit.Generators.Builder.Builders.FileBuilders;
using SourceKit.Generators.Builder.Builders.UsingBuilders;
using SourceKit.Generators.Builder.Commands;
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
            );

        var provider = collection.BuildServiceProvider();

        _chain = provider.GetRequiredService<IChain<FileBuildingCommand, CompilationUnitSyntax>>();
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

        Parallel.ForEach(receiver.TypeSymbols, s => GenerateForType(context, s));
    }

    private void GenerateForType(GeneratorExecutionContext context, INamedTypeSymbol symbol)
    {
        var fileCommand = new FileBuildingCommand(
            context,
            CompilationUnit(),
            symbol);

        var compilationUnit = _chain.Process(fileCommand);
        var fileName = $"{symbol.Name}.{Constants.FilenameSuffix}";
        
        context.AddSource(fileName, compilationUnit.ToFullString());
    }
}