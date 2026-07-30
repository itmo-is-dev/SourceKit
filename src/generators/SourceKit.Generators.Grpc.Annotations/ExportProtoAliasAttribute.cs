namespace SourceKit.Generators.Grpc.Annotations;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public sealed class ExportProtoAliasAttribute(string Alias, string TypeName) : Attribute;
