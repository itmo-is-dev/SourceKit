using System.Text;
using System.Text.RegularExpressions;
using SourceKit.Generators.Builder.Annotations;
using SourceKit.Generators.Builder.Generators;
using SourceKit.Tests.Common;
using SourceKit.Tests.Common.TestBases;
using Xunit;

namespace SourceKit.Generators.Builder.Tests;

public class BuilderSourceGeneratorTests : GeneratorTestBase<IncrementalBuilderGenerator>
{
    private const string MethodImplAttribute = "[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]";
    private string InitializesPropertyAttribute(string propertyName) => $"[InitializesPropertyAttribute(nameof({propertyName}))]";

    [Fact]
    public async Task Generate_ShouldBeCorrect_WhenGeneratedFromSimpleUnaryAndCollectionProperties()
    {
        var modelSource = new SourceFile(
            Name: "Model.cs",
            Encoding: Encoding.UTF8,
            Content: """
            using SourceKit.Generators.Builder.Annotations;
            using System.Collections.Generic;
            
            namespace TestNamespace;
            
            public record ReferenceTypeModel;
            
            [GenerateBuilder]
            public partial record Model(
                int UnaryValue, 
                int[] CollectionValue, 
                ReferenceTypeModel? NullableReferenceTypeProperty,
                IEnumerable<int> EnumerableProperty);
            """);

        var generatedSource = new SourceFile(
            Name: "SourceKit.Generators.Builder.Model.g.cs",
            Content: WrapInGeneratorTrivia($$"""
            namespace TestNamespace
            {
                public partial record Model
                {
                    {{MethodImplAttribute}}
                    public static Model Build(Func<Builder, Builder> action)
                    {
                        return action(new Builder()).Build();
                    }
                    
                    public sealed partial class Builder
                    {
                        private int _unaryValue;
                        private global::System.Collections.Generic.HashSet<int>? _collectionValue;
                        private global::TestNamespace.ReferenceTypeModel? _nullableReferenceTypeProperty;
                        private global::System.Collections.Generic.HashSet<int>? _enumerableProperty;
                        public Builder()
                        {
                            _unaryValue = default;
                            _nullableReferenceTypeProperty = default;
                        }
                        
                        {{InitializesPropertyAttribute("UnaryValue")}}
                        public Builder WithUnaryValue(int value)
                        {
                            _unaryValue = value;
                            return this;
                        }
                        
                        {{InitializesPropertyAttribute("CollectionValue")}}
                        public Builder WithCollectionValue(int element)
                        {
                            _collectionValue ??= [];
                            _collectionValue.Add(element);
                            return this;
                        }
                        
                        {{InitializesPropertyAttribute("CollectionValue")}}
                        public Builder WithCollectionValues(global::System.Collections.Generic.IEnumerable<int> elements)
                        {
                            _collectionValue ??= [];
                            foreach (int element in elements)
                                _collectionValue.Add(element);
                            return this;
                        }
                        
                        {{InitializesPropertyAttribute("NullableReferenceTypeProperty")}}
                        public Builder WithNullableReferenceTypeProperty(global::TestNamespace.ReferenceTypeModel? value)
                        {
                            _nullableReferenceTypeProperty = value;
                            return this;
                        }
                        
                        {{InitializesPropertyAttribute("EnumerableProperty")}}
                        public Builder WithEnumerableProperty(int element)
                        {
                            _enumerableProperty ??= [];
                            _enumerableProperty.Add(element);
                            return this;
                        }
                        
                        {{InitializesPropertyAttribute("EnumerableProperty")}}
                        public Builder WithEnumerableProperties(global::System.Collections.Generic.IEnumerable<int> elements)
                        {
                            _enumerableProperty ??= [];
                            foreach (int element in elements)
                                _enumerableProperty.Add(element);
                            return this;
                        }
                        
                        public Model Build()
                        {
                            return new Model(UnaryValue: _unaryValue, CollectionValue: [.._collectionValue ?? []], NullableReferenceTypeProperty: _nullableReferenceTypeProperty, EnumerableProperty: [.._enumerableProperty ?? []]);
                        }
                    }
                }
            }
            """));

        await GeneratorTest
            .WithSource(modelSource)
            .WithGeneratedSource(generatedSource)
            .WithAdditionalReference(typeof(GenerateBuilderAttribute).Assembly)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task Generate_ShouldCache_WhenOutsideFileChanged()
    {
        var modelSource = new SourceFile(
            Name: "Model.cs",
            Content: """
            using SourceKit.Generators.Builder.Annotations;
            
            namespace TestNamespace;
            
            [GenerateBuilder]
            public partial record Model(int UnaryValue);
            """);

        var outsideChangedSource = new SourceFile(
            Name: "Other.cs",
            Content: "public record Other;");

        var generatedSource = new SourceFile(
            Name: "SourceKit.Generators.Builder.Model.g.cs",
            Content: WrapInGeneratorTrivia($$"""
            namespace TestNamespace
            {
                public partial record Model
                {
                    {{MethodImplAttribute}}
                    public static Model Build(Func<Builder, Builder> action)
                    {
                        return action(new Builder()).Build();
                    }
                    
                    public sealed partial class Builder
                    {
                        private int _unaryValue;
                        public Builder()
                        {
                            _unaryValue = default;
                        }
                        
                        {{InitializesPropertyAttribute("UnaryValue")}}
                        public Builder WithUnaryValue(int value)
                        {
                            _unaryValue = value;
                            return this;
                        }
                        
                        public Model Build()
                        {
                            return new Model(UnaryValue: _unaryValue);
                        }
                    }
                }
            }
            """));

        await GeneratorTest
            .WithSource(modelSource)
            .WithGeneratedSource(generatedSource)
            .WithAdditionalReference(typeof(GenerateBuilderAttribute).Assembly)
            .Build()
            .RunWithTrackedStepsCacheVerificationAsync(["MapBuilderProperties"], outsideChangedSource);
    }

    [Fact]
    public async Task Generate_ShouldBeCorrect_WhenModelInGlobalNamespace()
    {
        var modelSource = new SourceFile(
            Name: "Model.cs",
            Encoding: Encoding.UTF8,
            Content: """
            using SourceKit.Generators.Builder.Annotations;
            
            [GenerateBuilder]
            public partial record Model(int UnaryValue);
            """);

        var generatedSource = new SourceFile(
            Name: "SourceKit.Generators.Builder.Model.g.cs",
            Content: WrapInGeneratorTrivia($$"""
            public partial record Model
            {
                {{MethodImplAttribute}}
                public static Model Build(Func<Builder, Builder> action)
                {
                    return action(new Builder()).Build();
                }
                
                public sealed partial class Builder
                {
                    private int _unaryValue;
                    public Builder()
                    {
                        _unaryValue = default;
                    }
                    
                    {{InitializesPropertyAttribute("UnaryValue")}}
                    public Builder WithUnaryValue(int value)
                    {
                        _unaryValue = value;
                        return this;
                    }
                    
                    public Model Build()
                    {
                        return new Model(UnaryValue: _unaryValue);
                    }
                }
            }
            """));

        await GeneratorTest
            .WithSource(modelSource)
            .WithGeneratedSource(generatedSource)
            .WithAdditionalReference(typeof(GenerateBuilderAttribute).Assembly)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task Generate_ShouldBeCorrect_WhenGeneratedFromConstructorParameterProperties()
    {
        var modelSource = new SourceFile(
            Name: "Model.cs",
            Encoding: Encoding.UTF8,
            Content: """
            using SourceKit.Generators.Builder.Annotations;
            
            namespace TestNamespace;
            
            [GenerateBuilder]
            public partial record Model(
                [BuilderProperty(BuilderPropertyOptions.ConstructorParameter)] int UnaryValue, 
                [BuilderProperty(BuilderPropertyOptions.ConstructorParameter)] int[] CollectionValue);
            """);

        var generatedSource = new SourceFile(
            Name: "SourceKit.Generators.Builder.Model.g.cs",
            Content: WrapInGeneratorTrivia($$"""
            namespace TestNamespace
            {
                public partial record Model
                {
                    {{MethodImplAttribute}}
                    public static Model Build(int unaryValue, global::System.Collections.Generic.IEnumerable<int> collectionValue, Func<Builder, Builder> action)
                    {
                        return action(new Builder(unaryValue, collectionValue)).Build();
                    }
                    
                    public sealed partial class Builder
                    {
                        private int _unaryValue;
                        private global::System.Collections.Generic.HashSet<int> _collectionValue;
                        public Builder(int unaryValue, global::System.Collections.Generic.IEnumerable<int> collectionValue)
                        {
                            _unaryValue = unaryValue;
                            _collectionValue = [..collectionValue];
                        }
                        
                        {{InitializesPropertyAttribute("UnaryValue")}}
                        public Builder WithUnaryValue(int value)
                        {
                            _unaryValue = value;
                            return this;
                        }
                        
                        {{InitializesPropertyAttribute("CollectionValue")}}
                        public Builder WithCollectionValue(int element)
                        {
                            _collectionValue.Add(element);
                            return this;
                        }
                        
                        {{InitializesPropertyAttribute("CollectionValue")}}
                        public Builder WithCollectionValues(global::System.Collections.Generic.IEnumerable<int> elements)
                        {
                            foreach (int element in elements)
                                _collectionValue.Add(element);
                            return this;
                        }
                        
                        public Model Build()
                        {
                            return new Model(UnaryValue: _unaryValue, CollectionValue: [.._collectionValue]);
                        }
                    }
                }
            }
            """));

        await GeneratorTest
            .WithSource(modelSource)
            .WithGeneratedSource(generatedSource)
            .WithAdditionalReference(typeof(GenerateBuilderAttribute).Assembly)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task Generate_ShouldBeCorrect_WhenGeneratedFromExposedProperties()
    {
        var modelSource = new SourceFile(
            Name: "Model.cs",
            Encoding: Encoding.UTF8,
            Content: """
            using SourceKit.Generators.Builder.Annotations;
            
            namespace TestNamespace;
            
            [GenerateBuilder]
            public partial record Model(
                [BuilderProperty(BuilderPropertyOptions.Exposed)] int UnaryValue,
                [BuilderProperty(BuilderPropertyOptions.Exposed)] int[] CollectionValue);
            """);

        var generatedSource = new SourceFile(
            Name: "SourceKit.Generators.Builder.Model.g.cs",
            Content: WrapInGeneratorTrivia($$"""
            namespace TestNamespace
            {
                public partial record Model
                {
                    {{MethodImplAttribute}}
                    public static Model Build(Func<Builder, Builder> action)
                    {
                        return action(new Builder()).Build();
                    }
                    
                    public sealed partial class Builder
                    {
                        private global::System.Collections.Generic.HashSet<int>? _collectionValue;
                        public Builder()
                        {
                            UnaryValue = default;
                        }
                        
                        public int UnaryValue { get; private set; }
                        public global::System.Collections.Generic.IEnumerable<int> CollectionValue => _collectionValue ?? [];
                        
                        {{InitializesPropertyAttribute("UnaryValue")}}
                        public Builder WithUnaryValue(int value)
                        {
                            UnaryValue = value;
                            return this;
                        }
                        
                        {{InitializesPropertyAttribute("CollectionValue")}}
                        public Builder WithCollectionValue(int element)
                        {
                            _collectionValue ??= [];
                            _collectionValue.Add(element);
                            return this;
                        }
                        
                        {{InitializesPropertyAttribute("CollectionValue")}}
                        public Builder WithCollectionValues(global::System.Collections.Generic.IEnumerable<int> elements)
                        {
                            _collectionValue ??= [];
                            foreach (int element in elements)
                                _collectionValue.Add(element);
                            return this;
                        }
                        
                        public Model Build()
                        {
                            return new Model(UnaryValue: UnaryValue, CollectionValue: [.._collectionValue ?? []]);
                        }
                    }
                }
            }
            """));

        await GeneratorTest
            .WithSource(modelSource)
            .WithGeneratedSource(generatedSource)
            .WithAdditionalReference(typeof(GenerateBuilderAttribute).Assembly)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task Generate_ShouldBeCorrect_WhenGeneratedFromCollectionPropertyAllowingDuplicates()
    {
        var modelSource = new SourceFile(
            Name: "Model.cs",
            Encoding: Encoding.UTF8,
            Content: """
            using SourceKit.Generators.Builder.Annotations;
            
            namespace TestNamespace;
            
            [GenerateBuilder]
            public partial record Model(
                [BuilderProperty(BuilderPropertyOptions.AllowsDuplicates)] int[] CollectionValue);
            """);

        var generatedSource = new SourceFile(
            Name: "SourceKit.Generators.Builder.Model.g.cs",
            Content: WrapInGeneratorTrivia($$"""
            namespace TestNamespace
            {
                public partial record Model
                {
                    {{MethodImplAttribute}}
                    public static Model Build(Func<Builder, Builder> action)
                    {
                        return action(new Builder()).Build();
                    }
                    
                    public sealed partial class Builder
                    {
                        private global::System.Collections.Generic.List<int>? _collectionValue;
                        public Builder()
                        {
                        }
                        
                        {{InitializesPropertyAttribute("CollectionValue")}}
                        public Builder WithCollectionValue(int element)
                        {
                            _collectionValue ??= [];
                            _collectionValue.Add(element);
                            return this;
                        }
                        
                        {{InitializesPropertyAttribute("CollectionValue")}}
                        public Builder WithCollectionValues(global::System.Collections.Generic.IEnumerable<int> elements)
                        {
                            _collectionValue ??= [];
                            foreach (int element in elements)
                                _collectionValue.Add(element);
                            return this;
                        }
                        
                        public Model Build()
                        {
                            return new Model(CollectionValue: [.._collectionValue ?? []]);
                        }
                    }
                }
            }
            """));

        await GeneratorTest
            .WithSource(modelSource)
            .WithGeneratedSource(generatedSource)
            .WithAdditionalReference(typeof(GenerateBuilderAttribute).Assembly)
            .Build()
            .RunAsync();
    }

    [Fact]
    public async Task Generate_ShouldBeCorrect_WhenGeneratedFromNotCopiedCollectionProperty()
    {
        var modelSource = new SourceFile(
            Name: "Model.cs",
            Encoding: Encoding.UTF8,
            Content: """
            using SourceKit.Generators.Builder.Annotations;
            using System.Collections.Generic;
            
            namespace TestNamespace;
            
            [GenerateBuilder]
            public partial record Model(
                [BuilderProperty(BuilderPropertyOptions.NotCopied)] IReadOnlyCollection<int> CollectionValue);
            """);

        var generatedSource = new SourceFile(
            Name: "SourceKit.Generators.Builder.Model.g.cs",
            Content: WrapInGeneratorTrivia($$"""
            namespace TestNamespace
            {
                public partial record Model
                {
                    {{MethodImplAttribute}}
                    public static Model Build(Func<Builder, Builder> action)
                    {
                        return action(new Builder()).Build();
                    }
                    
                    public sealed partial class Builder
                    {
                        private global::System.Collections.Generic.HashSet<int>? _collectionValue;
                        public Builder()
                        {
                        }
                        
                        {{InitializesPropertyAttribute("CollectionValue")}}
                        public Builder WithCollectionValue(int element)
                        {
                            _collectionValue ??= [];
                            _collectionValue.Add(element);
                            return this;
                        }
                        
                        {{InitializesPropertyAttribute("CollectionValue")}}
                        public Builder WithCollectionValues(global::System.Collections.Generic.IEnumerable<int> elements)
                        {
                            _collectionValue ??= [];
                            foreach (int element in elements)
                                _collectionValue.Add(element);
                            return this;
                        }
                        
                        public Model Build()
                        {
                            return new Model(CollectionValue: _collectionValue ?? []);
                        }
                    }
                }
            }
            """));

        await GeneratorTest
            .WithSource(modelSource)
            .WithGeneratedSource(generatedSource)
            .WithAdditionalReference(typeof(GenerateBuilderAttribute).Assembly)
            .Build()
            .RunAsync();
    }

    private static string WrapInGeneratorTrivia(string content)
    {
        content = $"""
        // <auto-generated>
        // This code was generated by a SourceKit.Generators.Builder code generator.
        // https://github.com/itmo-is-dev/SourceKit
        // </auto-generated>
        #pragma warning disable CS1591
        using SourceKit.Generators.Builder.Annotations;
        using System;
        using System.Linq;
        
        #nullable enable
        {content}
        """;

        return Regex.Replace(content, "\n[ ]+\n", "\n\n");
    }
}
