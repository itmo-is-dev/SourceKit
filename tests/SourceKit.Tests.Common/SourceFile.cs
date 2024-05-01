using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace SourceKit.Tests.Common;

public readonly record struct SourceFile(string Name, string Content, Encoding? Encoding = null)
{
    public string FilePath { get; init; } = Name;
    
    public static async Task<SourceFile> LoadAsync(string path)
    {
        string name = Path.GetFileName(path);
        string content = await File.ReadAllTextAsync(path);

        return new SourceFile(name, content, null) { FilePath = path };
    }

    public static implicit operator (string, SourceText)(SourceFile sourceFile)
        => (sourceFile.Name, SourceText.From(sourceFile.Content, sourceFile.Encoding ?? Encoding.UTF8));
}