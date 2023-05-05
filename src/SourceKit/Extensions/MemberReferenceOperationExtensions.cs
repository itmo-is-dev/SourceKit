using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;

namespace SourceKit.Extensions;

public static class MemberReferenceOperationExtensions
{
    public static bool MemberHasNullableType(this IMemberReferenceOperation referenceOperation)
    {
        return referenceOperation switch
        {
            IPropertyReferenceOperation o => o.Property.Type.NullableAnnotation is not NullableAnnotation.NotAnnotated,
            IFieldReferenceOperation o => o.Field.Type.NullableAnnotation is not NullableAnnotation.NotAnnotated,
            _ => throw new NotSupportedException("Only property or field members are supported"),
        };
    }
}