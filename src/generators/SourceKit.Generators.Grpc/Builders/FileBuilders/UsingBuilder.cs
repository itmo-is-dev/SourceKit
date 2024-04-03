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
            (a, b) => a.Name.ToString().Equals(b.Name.ToString()),
            x => x.Name.ToString().GetHashCode());

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
        var unit = next(request, context);

#pragma warning disable RS1024
        IEnumerable<UsingDirectiveSyntax> propertyUsingDirectives = ExtractNamespaces(request.Message.Properties)
            .Distinct()
            .Select(x => x.GetFullyQualifiedName())
            .Select(x => UsingDirective(IdentifierName(x)));
#pragma warning restore RS1024

        UsingDirectiveSyntax[] usingDirectives = unit.Usings
            .Append(UsingDirective(IdentifierName("System")))
            .Append(UsingDirective(IdentifierName("System.Linq")))
            .Append(UsingDirective(IdentifierName("System.Collections.Generic")))
            .Concat(propertyUsingDirectives)
            .Distinct(Comparer)
            .OrderBy(x => x.Name.ToString())
            .ToArray();

        var firstDirective = usingDirectives[0];

        var usingBuildingCommand = new UsingBuildingCommand(firstDirective);
        usingDirectives[0] = _usingChain.Process(usingBuildingCommand);

        return unit.WithUsings(List(usingDirectives));
    }

    private static IEnumerable<INamespaceSymbol> ExtractNamespaces(IEnumerable<ProtoProperty> properties)
    {
        foreach (var property in properties)
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