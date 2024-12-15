using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace SourceKit.Extensions;

public static class NamedTypeSymbolExtensions
{
    private static SymbolDisplayFormat ConfigureDisplayFormat(SymbolDisplayFormat format) => format
        .WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted)
        .WithGenericsOptions(SymbolDisplayGenericsOptions.None);

    private static readonly SymbolDisplayFormat FullyQualifiedSymbolFormat =
        ConfigureDisplayFormat(SymbolDisplayFormat.FullyQualifiedFormat);

    private static readonly SymbolDisplayFormat ShortSymbolFormat =
        ConfigureDisplayFormat(SymbolDisplayFormat.MinimallyQualifiedFormat);

    private static bool TryGetTypeArgumentSyntax(
        this INamespaceOrTypeSymbol symbol,
        bool fullyQualified,
        [NotNullWhen(true)] out TypeArgumentListSyntax? syntax)
    {
        bool isNullableValueType = symbol is INamedTypeSymbol
        {
            ConstructedFrom.SpecialType: SpecialType.System_Nullable_T,
        };

        if (symbol is INamedTypeSymbol { TypeArguments: { Length: not 0 } typeArguments }
            && isNullableValueType is false)
        {
            syntax = TypeArgumentList(SeparatedList(typeArguments.Select(x => x.ToNameSyntax(fullyQualified))));
            return true;
        }

        syntax = null;
        return false;
    }

    public static string GetFullyQualifiedName(this INamespaceOrTypeSymbol symbol)
        => symbol.ToDisplayString(FullyQualifiedSymbolFormat);

    public static string GetShortName(this INamespaceOrTypeSymbol symbol)
        => symbol.ToDisplayString(ShortSymbolFormat);

    public static TypeSyntax ToNameSyntax(this INamespaceOrTypeSymbol symbol, bool fullyQualified = true)
    {
        string name = fullyQualified ? symbol.GetFullyQualifiedName() : symbol.GetShortName();

        TypeSyntax type = TryGetTypeArgumentSyntax(symbol, fullyQualified, out TypeArgumentListSyntax? typeArguments)
            ? GenericName(Identifier(name), typeArguments)
            : IdentifierName(name);

        bool shouldAnnotateReferenceType = symbol is INamedTypeSymbol
        {
            IsReferenceType: true,
            NullableAnnotation: NullableAnnotation.Annotated,
        };

        bool shouldAnnotateValueType = symbol is INamedTypeSymbol
        {
            IsValueType: true,
            NullableAnnotation: NullableAnnotation.Annotated,
            ConstructedFrom.SpecialType: not SpecialType.System_Nullable_T,
        };

        if (shouldAnnotateReferenceType || shouldAnnotateValueType)
        {
            type = NullableType(type);
        }

        return type;
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

            _ => throw new ArgumentOutOfRangeException(),
        };
    }
}