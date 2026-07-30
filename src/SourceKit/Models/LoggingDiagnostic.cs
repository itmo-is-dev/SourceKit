using Microsoft.CodeAnalysis;

namespace SourceKit.Models;

public static class LoggingDiagnostic
{
#pragma warning disable RS2008
    private static readonly DiagnosticDescriptor Descriptor = new(
        id: "SK0000",
        description: "Analyzer execution log.",
        title: "Log",
        messageFormat: "{0}",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    public static Diagnostic Create(string text, Location? location = null)
    {
        return Diagnostic.Create(Descriptor, location, messageArgs: text);
    }
}
