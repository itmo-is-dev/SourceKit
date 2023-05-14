using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Extensions;

public static class NamedTypeSymbolExtensions
{
    public static SimpleNameSyntax ToNameSyntax(this INamespaceOrTypeSymbol symbol, bool fullyQualified = false)
    {
        IReadOnlyCollection<IdentifierNameSyntax> typeParameters = symbol switch
        {
            INamedTypeSymbol namedTypeSymbol => namedTypeSymbol.TypeArguments.ToTypeArgumentSyntax().ToArray(),
            _ => Array.Empty<IdentifierNameSyntax>(),
        };

        var name = fullyQualified ? symbol.GetFullyQualifiedName() : symbol.Name;

        return typeParameters.Count is 0
            ? IdentifierName(name)
            : GenericName(Identifier(name), TypeArgumentList(SeparatedList<TypeSyntax>(typeParameters)));
    }

    public static IEnumerable<INamedTypeSymbol> GetBaseTypes(this INamedTypeSymbol symbol)
    {
        return symbol.BaseType is null
            ? Enumerable.Empty<INamedTypeSymbol>()
            : Enumerable.Repeat(symbol.BaseType, 1).Concat(symbol.BaseType.GetBaseTypes());
    }

    public static IEnumerable<INamedTypeSymbol> GetBaseTypesAndInterfaces(this INamedTypeSymbol symbol)
    {
        return symbol.GetBaseTypes().Concat(symbol.AllInterfaces);
    }

    public static bool HasAttribute(this INamedTypeSymbol symbol, INamedTypeSymbol attribute)
    {
        return symbol
            .GetAttributes()
            .Select(x => x.AttributeClass)
            .WhereNotNull()
            .Contains(attribute, SymbolEqualityComparer.Default);
    }

    public static IEnumerable<TypeDeclarationSyntax> GetDeclarations(this INamedTypeSymbol symbol)
    {
        return symbol.Locations
            .Select(x => (location: x, x.SourceTree))
            .Where(x => x.SourceTree is not null)
            .Select(x => x.SourceTree!.GetRoot().FindNode(x.location.SourceSpan))
            .OfType<TypeDeclarationSyntax>();
    }

    public static bool IsPartial(this INamedTypeSymbol symbol)
    {
        return symbol
            .GetDeclarations()
            .SelectMany(x => x.Modifiers)
            .Any(x => x.IsKind(SyntaxKind.PartialKeyword));
    }

    public static IEnumerable<Location> GetSignatureLocations(this INamedTypeSymbol symbol)
    {
        return symbol.GetDeclarations().Select(x => x.Identifier.GetLocation());
    }

    public static TypeDeclarationSyntax ToSyntax(this INamedTypeSymbol symbol)
    {
        return symbol.TypeKind switch
        {
            TypeKind.Class when symbol.IsRecord => RecordDeclaration(Token(SyntaxKind.RecordKeyword), symbol.Name)
                .WithOpenBraceToken(Token(SyntaxKind.OpenBraceToken))
                .WithCloseBraceToken(Token(SyntaxKind.CloseBraceToken)),

            TypeKind.Class => ClassDeclaration(symbol.Name),

            TypeKind.Struct when symbol.IsRecord => RecordDeclaration(Token(SyntaxKind.RecordKeyword), symbol.Name)
                .WithClassOrStructKeyword(Token(SyntaxKind.StructKeyword))
                .WithOpenBraceToken(Token(SyntaxKind.OpenBraceToken))
                .WithCloseBraceToken(Token(SyntaxKind.CloseBraceToken)),

            TypeKind.Struct => StructDeclaration(symbol.Name),

            TypeKind.Interface => InterfaceDeclaration(symbol.Name),

            _ => throw new ArgumentOutOfRangeException()
        };
    }
}