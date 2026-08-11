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

    public static SourceFile FromContent(string content)
    {
        return new SourceFile(Name: "File.cs", Content: content, Encoding: Encoding.UTF8);
    }

    public (string, SourceText) AsTestSource()
    {
        return (FilePath, SourceText.From(Content, Encoding));
    }

    public (Type, string, string) AsGeneratorSource<T>()
        where T : IIncrementalGenerator
    {
        return (typeof(T), Name, Content);
    }
}
