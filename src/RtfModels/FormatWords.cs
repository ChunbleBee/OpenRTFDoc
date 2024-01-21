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
    ParagraphNumbering = 0b100000000,
    AllFormatting = Font | FontSize | Decorator | Color | Justification | Visibility | Margin | TextDirection | ParagraphNumbering
}

/// <summary>
/// VerticalTypesetType describes what type of typesetting the 
/// </summary>
public enum VerticalTypesetType
{
    None,
    SuperScript,
    SubScript
}

/// <summary>
/// IFormat is an interface for all control words that provide formatting.
/// </summary>
public interface IFormat
{
    /// <summary>
    /// Gets the formatting type of this control word.
    /// </summary>
    public FormatType FormatType { get; }
}

/// <summary>
/// Initializes a new instance of the <see cref="FormattingBinaryToggleWord"/> class.
/// </summary>
/// <remarks>FormattingBinaryToggleWord describes control words that provide document formatting with on or off states.</remarks>
/// <param name="formatType">The <see cref="FormatType"/> of this control word.</param>
/// <param name="name">The name of this control word.</param>
/// <param name="param">The parameter value of this control word.</param>
public class FormattingBinaryToggleWord(FormatType formatType, string name, string? param = null) : BinaryToggleWord(name, param), IFormat
{
    /// <inheritdoc/>
    public FormatType FormatType { get; } = formatType;
}

/// <summary>
/// Initializes a new instance of the <see cref="MultiStateToggleWord"/> class.
/// </summary>
/// <remarks>MultiStateToggleWord describes flag control words that, can have one of many discrete states.</remarks>
/// <param name="formatType">The <see cref="FormatType"/> of this control word.</param>
/// <param name="name">The name of this control word.</param>
public abstract class MultiStateFlagWord<T>(string name, T type) : FlagWord(name)
{
    /// <summary>
    /// Gets the state of this word.
    /// </summary>
    public T State { get; } = type;
}

/// <summary>
/// VerticalTypesetWord describes the whether the next run of text should be superscripted, subscripted, or neither.
/// </summary>
public class VerticalTypesetWord : MultiStateFlagWord<VerticalTypesetType>, IFormat
{
    /// <summary>
    /// Gets the <see cref="RtfModels.FormatType"/> flag of this class.
    /// </summary>
    public FormatType FormatType { get; } = FormatType.Decorator;

    /// <summary>
    /// Initializes a new instance of the <see cref="VerticalTypesetWord"/> class.
    /// </summary>
    /// <param name="name">The RTF control word name.</param>
    /// <param name="type">The <see cref="VerticalTypesetType"/> of this typesetting.</param>
    public VerticalTypesetWord(string name, VerticalTypesetType type) : base(name, type) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="VerticalTypesetWord"/> class.
    /// </summary>
    /// <param name="type">The <see cref="VerticalTypesetType"/> of this typesetting.</param>
    /// <exception cref="ArgumentException">Thrown if the value of <paramref name="type"/> isn't recognized</exception>
    public VerticalTypesetWord(VerticalTypesetType type) : base(string.Empty, type)
    {
        Name = type switch
        {
            VerticalTypesetType.None => "nosupersub",
            VerticalTypesetType.SuperScript => "super",
            VerticalTypesetType.SubScript => "sub",
            _ => throw new ArgumentException($"unknown enum value: {type}")
        };
    }
}

public class BoldWord(string? param = null) : FormattingBinaryToggleWord(FormatType.Decorator, "b", param) { }
public class ItalicsWord(string? param = null) : FormattingBinaryToggleWord(FormatType.Decorator, "i", param) { }
public class UnderlinedWord(string? param = null) : FormattingBinaryToggleWord(FormatType.Decorator, "ul", param) { }
public class StrikeThroughWord(string? param = null) : FormattingBinaryToggleWord(FormatType.Decorator, "strike", param) { }