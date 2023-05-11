using System.Text;

namespace SourceKit.Tools;

public readonly struct StringModifier
{
    private readonly StringBuilder _builder;

    public StringModifier(string value)
    {
        _builder = new StringBuilder(value);
    }

    public char this[int i]
    {
        set
        {
            if (i < 0)
            {
                _builder.Insert(0, value);
            }
            else
            {
                _builder[i] = value;
            }
        }
    }

    public static implicit operator string(StringModifier modifier)
        => modifier._builder.ToString();
}