namespace RtfDom;

using System.Drawing;
using RtfModels;

/// <summary>
/// DocumentNode represents the root of an RTF document, and thus contains
/// Metadata, Style Tables, and ohter information about the document.
/// </summary>
public class DocumentNode : Node
{
    public Dictionary<string, Color> ColorTable { get; } = [];
    public Dictionary<string, FontReference> FontTable { get; } = [];
}