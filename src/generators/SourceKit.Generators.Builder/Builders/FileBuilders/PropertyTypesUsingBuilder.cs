using FluentChaining;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;
using SourceKit.Generators.Builder.Commands;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Generators.Builder.Builders.FileBuilders;

public class PropertyTypesUsingBuilder : ILink<FileBuildingCommand, CompilationUnitSyntax>
{
    public CompilationUnitSyntax Process(
        FileBuildingCommand request,
        SynchronousContext context,
        LinkDelegate<FileBuildingCommand, SynchronousContext, CompilationUnitSyntax> next)
    {
        var propertyTypes = request.Symbol
            .GetMembers()
            .OfType<IPropertySymbol>()
            .Select(x => x.Type)
            .Distinct(SymbolEqualityComparer.Default)
            .WhereNotNull();

        var namespaces = propertyTypes
            .Select(x => x.ContainingNamespace)
            .WhereNotNull()
            .Select(x => x.GetFullyQualifiedName())
            .Distinct();

        var directives = namespaces
            .Select(x => UsingDirective(IdentifierName(x)))
            .ToArray();

        request = request with
        {
            CompilationUnit = request.CompilationUnit.AddUsings(directives),
        };

        return next(request, context);
    }
}