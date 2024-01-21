namespace RtfModels;

/// <summary>
/// FormatType describes what type of formatting is applied.
/// </summary>
[Flags]
public enum FormatType
{
    Font = 0b1,
    FontSize = 0b10,
    Decorator = 0b100,
    Color = 0b1000,
    Justification = 0b10000,
    Visibility = 0b100000,
    Margin = 0b1000000,
    TextDirection = 0b10000000,
    ParagraphNumbering = 0b10000000,
    AllFormatting = Font | FontSize | Decorator | Color | Justification | Visibility | Margin | TextDirection | ParagraphNumbering
}

/// <summary>
/// IFormat is an interface for all control words that provide formatting.
/// </summary>
public interface IFormat
{
    public FormatType FormatType { get; }
}

