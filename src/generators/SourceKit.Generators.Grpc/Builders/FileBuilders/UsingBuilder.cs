using FluentChaining;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;
using SourceKit.Generators.Grpc.Commands;
using SourceKit.Generators.Grpc.Models;
using SourceKit.Tools;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Generators.Grpc.Builders.FileBuilders;

public class UsingBuilder : ILink<FileBuildingCommand, CompilationUnitSyntax>
{
    private static readonly IEqualityComparer<UsingDirectiveSyntax> Comparer =
        EqualityComparerFactory.Create<UsingDirectiveSyntax>(
            (a, b) => string.Equals(a.Name?.ToString(), b.Name?.ToString()),
            x => x.Name?.ToString().GetHashCode() ?? 0);

    private readonly IChain<UsingBuildingCommand, UsingDirectiveSyntax> _usingChain;

    public UsingBuilder(IChain<UsingBuildingCommand, UsingDirectiveSyntax> usingChain)
    {
        _usingChain = usingChain;
    }

    public CompilationUnitSyntax Process(
        FileBuildingCommand request,
        SynchronousContext context,
        LinkDelegate<FileBuildingCommand, SynchronousContext, CompilationUnitSyntax> next)
    {
        CompilationUnitSyntax unit = next(request, context);

        UsingDirectiveSyntax[] usingDirectives = unit.Usings
            .Append(UsingDirective(IdentifierName("System")))
            .Append(UsingDirective(IdentifierName("System.Linq")))
            .Append(UsingDirective(IdentifierName("System.Collections.Generic")))
            .Distinct(Comparer)
            .Where(x => x.Name is not null)
            .OrderBy(x => x.Name?.ToString())
            .ToArray();

        UsingDirectiveSyntax firstDirective = usingDirectives[0];

        var usingBuildingCommand = new UsingBuildingCommand(firstDirective);
        usingDirectives[0] = _usingChain.Process(usingBuildingCommand);

        return unit.WithUsings(List(usingDirectives));
    }

    private static IEnumerable<INamespaceSymbol> ExtractNamespaces(IEnumerable<ProtoProperty> properties)
    {
        foreach (ProtoProperty? property in properties)
        {
            if (property is RepeatableProtoProperty repeatableProperty)
            {
                yield return repeatableProperty.ElementType.ContainingNamespace;
            }
            else if (property is MapProtoProperty mapProperty)
            {
                yield return mapProperty.Key.ContainingNamespace;
                yield return mapProperty.Value.ContainingNamespace;
            }
            else if (property is ValueProtoProperty valueProperty)
            {
                yield return valueProperty.Type.ContainingNamespace;
            }
        }
    }
}