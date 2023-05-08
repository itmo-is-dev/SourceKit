using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace SourceKit.Tests.Tools;

public static class CompilationBuilder
{
    public static Task<Compilation> CompileSampleProjectAsync()
    {
        var workspace = new AdhocWorkspace();
        var _ = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions);

        var solution = workspace.CurrentSolution;
        var projectId = ProjectId.CreateNewId();

        solution = solution.AddProject(projectId, "SourceKit.Sample", "SourceKit.Sample", LanguageNames.CSharp);

        IEnumerable<string> sourceFiles = Directory.EnumerateFiles(
                "SourceKit.Sample",
                "*.*",
                SearchOption.AllDirectories)
            .Select(Path.GetFullPath);

        foreach (var sourceFile in sourceFiles)
        {
            var fileName = Path.GetFileName(sourceFile);
            var source = new FileTextLoader(sourceFile, Encoding.Default);

            solution = solution.AddDocument(DocumentId.CreateNewId(projectId), fileName, source);
        }

        Type[] referencedTypes =
        {
            typeof(object),
            typeof(Console),
            typeof(IEnumerable<>),
            typeof(Enumerable),
        };

        var project = solution.GetProject(projectId)!;
        project = project.WithCompilationOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        project = project.AddMetadataReferences(GetAllReferencesNeededForTypes(referencedTypes));

        workspace.TryApplyChanges(project.Solution);

        return workspace.CurrentSolution.Projects.Single().GetCompilationAsync()!;
    }

    private static IEnumerable<MetadataReference> GetAllReferencesNeededForTypes(IReadOnlyCollection<Type> types)
    {
        IEnumerable<string> files = types.SelectMany(GetAllAssemblyFilesNeededForType);
        return files.Select(x => (MetadataReference)MetadataReference.CreateFromFile(x));
    }

    private static string[] GetAllAssemblyFilesNeededForType(Type type)
    {
        return type.Assembly.GetReferencedAssemblies()
            .Select(x => Assembly.Load(x.FullName))
            .Append(type.Assembly)
            .Select(x => x.Location)
            .ToArray();
    }
}