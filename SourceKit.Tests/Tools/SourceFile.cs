namespace SourceKit.Tests.Tools;

public record struct SourceFile(string Name, string Content)
{
    public static async Task<SourceFile> LoadAsync(string path)
    {
        var name = Path.GetFileName(path);
        var content = await File.ReadAllTextAsync(path);

        return new SourceFile(name, content);
    }

    public static implicit operator (string, string)(SourceFile sourceFile)
        => (sourceFile.Name, sourceFile.Content);
}