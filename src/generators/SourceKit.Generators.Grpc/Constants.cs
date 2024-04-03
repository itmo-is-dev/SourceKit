namespace SourceKit.Generators.Grpc;

internal static class Constants
{
    public const string ProtobufNamespace = "Google.Protobuf";
    public const string ProtobufCollectionsNamespace = $"{ProtobufNamespace}.Collections";

    public const string ProtobufMessageInterfaceFullyQualifiedName = $"{ProtobufNamespace}.IMessage";

    public const string ProtobufRepeatedFieldFullyQualifiedName = $"{ProtobufCollectionsNamespace}.RepeatedField`1";
    public const string ProtobufMapFieldFullyQualifiedName = $"{ProtobufCollectionsNamespace}.MapField`2";

    public const string FilenameSuffix = "SourceKit.Generators.Grpc.g.cs";
}