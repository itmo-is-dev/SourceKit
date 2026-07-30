using System.Diagnostics;
using System.Text;
using SourceKit.Tests.Common;

namespace SourceKit.Generators.Grpc.Tests.Tools;

public static class ProtocHelper
{
    public static SourceFile[] CompileFromSources(params IEnumerable<SourceFile> sources)
    {
        DirectoryInfo tempDirectory = Directory.CreateTempSubdirectory();

        try
        {
            var paths = new List<string>();

            foreach (SourceFile sourceFile in sources)
            {
                string targetPath = Path.Combine(tempDirectory.FullName, sourceFile.Name);
                paths.Add(targetPath);

                if (targetPath.StartsWith(tempDirectory.FullName) is false)
                {
                    throw new InvalidOperationException("Invalid source path, source path should not be rooted");
                }

                File.WriteAllText(targetPath, sourceFile.Content, sourceFile.Encoding ?? Encoding.UTF8);
            }

            return CompileFromPaths(paths);
        }
        finally
        {
            tempDirectory.Delete(recursive: true);
        }
    }

    public static SourceFile[] CompileFromPaths(params IEnumerable<string> paths)
    {
        using var process = new Process();
        DirectoryInfo tempDirectory = Directory.CreateTempSubdirectory();

        DirectoryInfo outDirectory = tempDirectory.CreateSubdirectory("out");

        foreach (string path in paths)
        {
            File.Copy(path, Path.Combine(tempDirectory.FullName, Path.GetFileName(path)));
        }

        process.StartInfo = new ProcessStartInfo("protoc")
        {
            ArgumentList = { $"--csharp_out={outDirectory.FullName}" },
            RedirectStandardError = true,
            WorkingDirectory = tempDirectory.FullName,
        };

        foreach (string path in tempDirectory.EnumerateFiles().Select(file => file.Name))
        {
            process.StartInfo.ArgumentList.Add(path);
        }

        try
        {
            process.Start();
            process.WaitForExit();

            if (process.ExitCode is not 0)
            {
                throw new InvalidOperationException(process.StandardError.ReadToEnd());
            }
            
            return outDirectory
                .EnumerateFiles()
                .Select(file => new SourceFile(file.Name, File.ReadAllText(file.FullName), Encoding.UTF8))
                .ToArray();
        }
        finally
        {
            tempDirectory.Delete(recursive: true);
        }
    }
}
