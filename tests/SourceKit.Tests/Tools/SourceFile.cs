using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace SourceKit.Tests.Tools;

public record struct SourceFile(string Name, string Content, Encoding? Encoding = null)
{
    public static async Task<SourceFile> LoadAsync(string path)
    {
        var name = Path.GetFileName(path);
        var content = await File.ReadAllTextAsync(path);

        return new SourceFile(name, content, null);
    }

    public static implicit operator (string, SourceText)(SourceFile sourceFile)
        => (sourceFile.Name, SourceText.From(sourceFile.Content, sourceFile.Encoding ?? Encoding.UTF8));
}