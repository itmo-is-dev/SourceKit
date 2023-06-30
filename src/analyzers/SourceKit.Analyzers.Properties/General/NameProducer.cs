using System.Globalization;
using System.Text;

namespace SourceKit.Analyzers.Properties.General;

public static class NameProducer
{
    public static string GetPropertyName(string variableName)
    {
        var variableNameBuilder = new StringBuilder(variableName);

        if (variableNameBuilder[0] == '_' && variableNameBuilder.Length > 1)
        {
            variableNameBuilder.Remove(0, 1);
        }

        variableNameBuilder.Insert(0, char.ToUpper(variableNameBuilder[0]));
        variableNameBuilder.Remove(1, 1);
        return variableNameBuilder.ToString();
    }
}