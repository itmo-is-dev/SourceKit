using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Reflect.Extensions;
using SourceKit.Reflect.Reflectors;
using SourceKit.Sample;
using SourceKit.Tests.Tools;
using Xunit;

namespace SourceKit.Tests;

public class MethodReflectorTests
{
    [Fact]
    public async Task CreateMethod_LocalBinaryReference_ShouldWorkAsCompiledVersion()
    {
        // Arrange
        var reflector = SourceReflector.Builder.Build();
        var compilation = await CompilationBuilder.CompileSampleProjectAsync();

        var sourceFile = compilation.SyntaxTrees.Single(x => x.FilePath.Equals("Class1.cs"));

        var root = await sourceFile.GetRootAsync();
        var classDeclaration = root.DescendantNodes().OfType<ClassDeclarationSyntax>().First();

        var method = classDeclaration
            .ChildNodes()
            .OfType<MethodDeclarationSyntax>()
            .First(x => x.Identifier.Text.Equals(nameof(Class.LocalBinaryReference)));

        // Act
        var action = reflector.CreateMethod<Func<int, int>>(method, compilation);

        // Assert
        action.Invoke(1).Should().Be(Class.LocalBinaryReference(1));
    }

    [Fact]
    public async Task CreateMethod_MethodExtensionMethodCall_ShouldWorkAsCompiledVersion()
    {
        // Arrange
        var reflector = SourceReflector.Builder.WithEnumerableTypes().Build();
        var compilation = await CompilationBuilder.CompileSampleProjectAsync();

        var sourceFile = compilation.SyntaxTrees.Single(x => x.FilePath.Equals("Class1.cs"));

        var root = await sourceFile.GetRootAsync();
        var classDeclaration = root.DescendantNodes().OfType<ClassDeclarationSyntax>().First();

        var method = classDeclaration
            .ChildNodes()
            .OfType<MethodDeclarationSyntax>()
            .First(x => x.Identifier.Text.Equals(nameof(Class.MethodExtensionMethodCall)));

        // Act
        var action = reflector.CreateMethod<Func<IEnumerable<int>, int>>(method, compilation);

        // Assert
        var enumerable = new[] { 1, 2, 3 };
        action.Invoke(enumerable).Should().Be(Class.MethodExtensionMethodCall(enumerable));
    }

    [Fact]
    public async Task CreateMethod_CustomAggregate_ShouldWorkAsCompiledVersion()
    {
        // Arrange
        var reflector = SourceReflector.Builder.WithEnumerableTypes().Build();
        var compilation = await CompilationBuilder.CompileSampleProjectAsync();

        var sourceFile = compilation.SyntaxTrees.Single(x => x.FilePath.Equals("Class1.cs"));

        var root = await sourceFile.GetRootAsync();
        var classDeclaration = root.DescendantNodes().OfType<ClassDeclarationSyntax>().First();

        var method = classDeclaration
            .ChildNodes()
            .OfType<MethodDeclarationSyntax>()
            .First(x => x.Identifier.Text.Equals(nameof(Class.CustomAggregate)));

        // Act
        var action = reflector.CreateMethod<Func<IEnumerable<int>, int, string>>(method, compilation);

        // Assert
        var enumerable = new[] { 1, 2, 3 };
        var b = 1;
        action.Invoke(enumerable.ToArray(), b).Should().Be(Class.CustomAggregate(enumerable.ToArray(), b));
    }
}