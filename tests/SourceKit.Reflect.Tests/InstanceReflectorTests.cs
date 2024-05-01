using System.Drawing;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceKit.Extensions;
using SourceKit.Reflect.Reflectors;
using SourceKit.Tests.Common;
using Xunit;

namespace SourceKit.Reflect.Tests;

public class InstanceReflectorTests
{
    [Fact(Skip = "Not yet ready")]
    public async Task CreateInstance_ShouldCreateValidPoint()
    {
        // Arrange
        var reflector = SourceReflector.Builder.Build();
        var compilation = await CompilationBuilder.CompileSampleProjectAsync();

        var sourceFile = compilation.SyntaxTrees.Single(x => x.FilePath.Equals("Class1.cs"));

        var root = await sourceFile.GetRootAsync();
        var classDeclaration = root.DescendantNodes().OfType<ClassDeclarationSyntax>().First();
        var method = classDeclaration.ChildNodes().OfType<MethodDeclarationSyntax>().First();

        LocalDeclarationStatementSyntax[] body = method.Body?
            .ChildNodes()
            .OfType<LocalDeclarationStatementSyntax>()
            .ToArray() ?? Array.Empty<LocalDeclarationStatementSyntax>();

        var instanceSyntax1 = body.First().FindValue()!;
        var instanceSyntax2 = body.Skip(1).First().FindValue()!;

        // Act
        var point1 = reflector.CreateInstance<Point>(instanceSyntax1, compilation);
        var point2 = reflector.CreateInstance<Point>(instanceSyntax2, compilation);

        // Assert
        point1.Should().BeEquivalentTo(new Point(0, 1));
        point2.Should().BeEquivalentTo(new Point(0, 2));
    }
}