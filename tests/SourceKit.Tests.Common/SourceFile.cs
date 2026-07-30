using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace SourceKit.Tests.Common;

public readonly record struct SourceFile(string Name, string Content, Encoding? Encoding = null)
{
    public string FilePath { get; init; } = Name;

    public static async Task<SourceFile> LoadAsync(string path)
    {
        string name = Path.GetFileName(path);
        string content = await File.ReadAllTextAsync(path);

        return new SourceFile(name, content) { FilePath = path };
    }

    public static implicit operator (string, SourceText)(SourceFile sourceFile)
        => (sourceFile.Name, SourceText.From(sourceFile.Content, sourceFile.Encoding ?? Encoding.Default));

    public static implicit operator (string, string)(SourceFile sourceFile) => (sourceFile.Name, sourceFile.Content);

    public (Type GeneratorType, string Name, SourceText Content) AsGeneratorSource<T>()
        where T : IIncrementalGenerator
    {
        return (typeof(T), Name, SourceText.From(Content, Encoding ?? Encoding.Default));
    }
}
