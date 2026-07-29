using System.Text;
using Google.Protobuf;
using SourceKit.Generators.Grpc.Generators;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;
using Xunit;

namespace SourceKit.Generators.Grpc.Tests;

public class ProtoAliasGeneratorTests : GeneratorTestBase<ProtoMessageAliasGenerator>
{
    [Fact]
    public async Task ProtoMessageConstructorGenerator_ShouldGenerateCorrectConstructor()
    {
        var protoSource = new SourceFile(
            Name: "Model.cs",
            Content: """
            namespace My;
            
            public class Model : global::Google.Protobuf.IMessage<Model>;
            """,
            Encoding: Encoding.UTF8);

        var generatedSource = new SourceFile(
            Name: """
            SourceKit.Generators.Grpc/SourceKit.Generators.Grpc.Generators.ProtoMessageAliasGenerator/SourceKit.Generators.Grpc.ProtoAlias.cs
            """,
            Content: """
            namespace My;
            
            public class Model : global::Google.Protobuf.IMessage<Model>;
            """,
            Encoding: Encoding.UTF8);

        await GeneratorTest
            .WithSource(protoSource)
            .WithGeneratedSource(generatedSource)
            .WithAdditionalReference(typeof(IMessage).Assembly)
            .Build()
            .RunAsync();
    }
}