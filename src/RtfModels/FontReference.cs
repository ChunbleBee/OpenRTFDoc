namespace RtfModels;

using System.Text;

public class FontReference(string fontName)
{
    public string FontName { get; } = fontName;
    public Encoding Encoding { get; } = Encoding.Default;
}