using FluentChaining;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;
using SourceKit.Generators.Builder.Commands;
using SourceKit.Generators.Builder.Models;
using SourceKit.Tools;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Generators.Builder.Builders.FileBuilders;

public class UsingBuilder : ILink<FileBuildingCommand, CompilationUnitSyntax>
{
    private static readonly IEqualityComparer<UsingDirectiveSyntax> Comparer =
        EqualityComparerFactory.Create<UsingDirectiveSyntax>(
            (a, b) => string.Equals(a.Name?.ToString(), b.Name?.ToString()),
            x => x.Name?.ToString().GetHashCode() ?? 0);

    private readonly IChain<UsingBuildingCommand, UsingDirectiveSyntax> _commentChain;

    public UsingBuilder(IChain<UsingBuildingCommand, UsingDirectiveSyntax> commentChain)
    {
        _commentChain = commentChain;
    }

    public CompilationUnitSyntax Process(
        FileBuildingCommand request,
        SynchronousContext context,
        LinkDelegate<FileBuildingCommand, SynchronousContext, CompilationUnitSyntax> next)
    {
        CompilationUnitSyntax unit = next(request, context);

        IEnumerable<UsingDirectiveSyntax> propertyUsingDirectives = request.Properties
            .Select(SelectType)
            .Select(x => x.ContainingNamespace.GetFullyQualifiedName())
            .Select(x => UsingDirective(IdentifierName(x)));

        UsingDirectiveSyntax[] usingDirectives = unit.Usings
            .Append(UsingDirective(IdentifierName("System")))
            .Append(UsingDirective(IdentifierName("System.Linq")))
            .Append(UsingDirective(IdentifierName("System.Collections.Generic")))
            .Append(UsingDirective(IdentifierName(Constants.AnnotationsNamespace)))
            .Concat(propertyUsingDirectives)
            .Distinct(Comparer)
            .Where(x => x.Name is not null)
            .OrderBy(x => x.Name?.ToString())
            .ToArray();

        UsingDirectiveSyntax firstDirective = usingDirectives[0];

        var commentBuildingCommand = new UsingBuildingCommand(firstDirective, request.Symbol);
        usingDirectives[0] = _commentChain.Process(commentBuildingCommand);

        return unit.WithUsings(List(usingDirectives));
    }

    private ITypeSymbol SelectType(BuilderProperty property)
    {
        return property switch
        {
            BuilderProperty.Collection collection => collection.ElementType,
            BuilderProperty.Value value => value.Type,
            _ => throw new ArgumentOutOfRangeException(nameof(property)),
        };
    }
}