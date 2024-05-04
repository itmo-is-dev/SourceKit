using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using SourceKit.Extensions;
using SourceKit.Generators.Grpc.Models;

namespace SourceKit.Generators.Grpc.Receivers;

public class ProtoMessageReceiver : ISyntaxContextReceiver
{
    private readonly List<ProtoMessage> _messages;

    public ProtoMessageReceiver()
    {
        _messages = new List<ProtoMessage>();
    }

    public IReadOnlyCollection<ProtoMessage> Messages => _messages;

    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        INamedTypeSymbol? messageInterfaceSymbol = context.SemanticModel.Compilation
            .GetTypeByMetadataName(Constants.ProtobufMessageInterfaceFullyQualifiedName);

        if (messageInterfaceSymbol is null)
            return;

        ISymbol? symbolInfo = context.SemanticModel.GetDeclaredSymbol(context.Node);

        if (symbolInfo is not INamedTypeSymbol symbol)
            return;

        if (symbol.AllInterfaces.Contains(messageInterfaceSymbol, SymbolEqualityComparer.Default) is false)
            return;

        try
        {
            ProtoProperty[] properties = ParseProperties(symbol, context).ToArray();

            var message = new ProtoMessage(symbol, properties);
            _messages.Add(message);
        }
        catch (Exception e)
        {
            throw new Exception($"Exception of type = {e.GetType()} at {e.Source}", e);
        }
    }

    private static IEnumerable<ProtoProperty> ParseProperties(
        INamedTypeSymbol messageSymbol,
        GeneratorSyntaxContext context)
    {
        const string fieldNumberPostfix = "FieldNumber";

        Compilation compilation = context.SemanticModel.Compilation;

        INamedTypeSymbol? repeatedFieldType = compilation.GetTypeByMetadataName(Constants.ProtobufRepeatedFieldFullyQualifiedName);
        INamedTypeSymbol? mapFieldType = compilation.GetTypeByMetadataName(Constants.ProtobufMapFieldFullyQualifiedName);

        if (repeatedFieldType is null || mapFieldType is null)
            yield break;

        INamedTypeSymbol intSymbol = compilation.GetTypeSymbol<int>();

        ImmutableArray<ISymbol> members = messageSymbol.GetMembers();

        IEnumerable<IFieldSymbol> ordinalFields = members
            .OfType<IFieldSymbol>()
            .Where(x => x.IsConst)
            .Where(x => x.Type.Equals(intSymbol, SymbolEqualityComparer.Default))
            .Where(x => x.Name.EndsWith(fieldNumberPostfix));

        ImmutableHashSet<string> oneOfCases = messageSymbol
            .GetTypeMembers()
            .Where(x => x.TypeKind is TypeKind.Enum)
            .SelectMany(x => x.GetMembers())
            .Select(x => x.Name)
            .ToImmutableHashSet();

        foreach (IFieldSymbol? ordinalField in ordinalFields)
        {
            if (ordinalField.ConstantValue is not int ordinal)
                continue;

            string propertyName = ordinalField.Name[..^fieldNumberPostfix.Length];
            IPropertySymbol? property = members.OfType<IPropertySymbol>().SingleOrDefault(x => x.Name.Equals(propertyName));

            if (property is not { Type: INamedTypeSymbol propertyType })
                continue;

            bool isOneOf = oneOfCases.Contains(propertyName);
            bool? hasNullCheck = HasNullCheck(property, context);

            bool shouldAnnotateReferenceType = propertyType.IsReferenceType && (hasNullCheck is false || isOneOf);

            bool shouldAnnotateValueType =
                propertyType is { IsValueType: true, ConstructedFrom.SpecialType: not SpecialType.System_Nullable_T }
                && isOneOf;

            if (shouldAnnotateReferenceType || shouldAnnotateValueType)
            {
                propertyType = (INamedTypeSymbol)propertyType.WithNullableAnnotation(NullableAnnotation.Annotated);
            }

            if (propertyType.ConstructedFrom.Equals(repeatedFieldType, SymbolEqualityComparer.Default))
            {
                yield return new RepeatableProtoProperty(
                    propertyName,
                    ordinal,
                    (INamedTypeSymbol)propertyType.TypeArguments.Single());
            }
            else if (propertyType.ConstructedFrom.Equals(mapFieldType, SymbolEqualityComparer.Default))
            {
                yield return new MapProtoProperty(
                    propertyName,
                    ordinal,
                    (INamedTypeSymbol)propertyType.TypeArguments[0],
                    (INamedTypeSymbol)propertyType.TypeArguments[1]);
            }
            else
            {
                yield return isOneOf
                    ? new OneOfProtoProperty(propertyName, propertyType, ordinal)
                    : new ValueProtoProperty(propertyName, propertyType, ordinal);
            }
        }
    }

    private static bool? HasNullCheck(
        IPropertySymbol property,
        GeneratorSyntaxContext context)
    {
        if (property.Locations is not [var location])
            return null;

        var propertyNode = location.SourceTree?.GetRoot().FindNode(location.SourceSpan) as PropertyDeclarationSyntax;

        AccessorDeclarationSyntax? setterNode = propertyNode?.AccessorList?.Accessors
            .SingleOrDefault(x => x.Keyword.IsKind(SyntaxKind.SetKeyword));

        if (setterNode is not { Body: not null })
            return null;

        var operation = context.SemanticModel.GetOperation(setterNode.Body) as IBlockOperation;

        return operation?.Descendants()
            .OfType<IInvocationOperation>()
            .Any(x => x.TargetMethod.Name.Equals("CheckNotNull"));
    }

    private static bool TryGetImplementation(
        INamedTypeSymbol propertyType,
        INamedTypeSymbol assertedType,
        [NotNullWhen(true)] out INamedTypeSymbol? implementationType)
    {
        implementationType = propertyType.ConstructedFrom
            .FindAssignableTypeConstructedFrom(assertedType);

        return implementationType is not null;
    }
}