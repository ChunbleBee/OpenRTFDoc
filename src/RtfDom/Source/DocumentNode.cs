namespace RtfDom;

using System.Drawing;
using RtfModels;

/// <summary>
/// DocumentNode represents the root of an RTF document, and thus contains
/// Metadata, Style Tables, and ohter information about the document.
/// </summary>
public class DocumentNode : Node
{
    private readonly string defaultKey = "default";
    public Dictionary<string, Color> ColorTable { get; } = [];
    public Dictionary<string, FontReference> FontTable { get; } = [];

    public DocumentNode()
    {
        FontReference defFont = new("Times New Roman");
        Color defColor = Color.Black;
        ColorTable[defaultKey] = defColor;
        FontTable[defaultKey] = defFont;

        Attributes.Add("Font", new FontAttribute(defaultKey, this));
    }

    public FormatList GetDefaultFormatting()
    {
        throw new NotImplementedException();
    }
}