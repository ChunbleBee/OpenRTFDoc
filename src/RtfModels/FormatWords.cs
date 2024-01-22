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
public class EngraveWord(string? param = null) : FormattingBinaryToggleWord(FormatType.Decorator, "impr", param) { }
public class StrikeThroughWord(string? param = null) : FormattingBinaryToggleWord(FormatType.Decorator, "strike", param) { }
public class EmbossWord(string? param = null) : FormattingBinaryToggleWord(FormatType.Decorator, "embo", param) { }
public class ShadowWord(string? param = null) : FormattingBinaryToggleWord(FormatType.Decorator, "shad", param) { }
public class OutlineWord(string? param = null) : FormattingBinaryToggleWord(FormatType.Decorator, "outl", param) { }
public class CapitalsLockWord(string? param = null) : FormattingBinaryToggleWord(FormatType.Decorator, "caps", param) { }
public class LowersLockWord(string? param = null) : FormattingBinaryToggleWord(FormatType.Decorator, "scaps", param) { }
public class VisibilityWord(string? param = null) : FormattingBinaryToggleWord(FormatType.Decorator, "v", param) { }

// Underline control words
public class UnderlineWord(string? param = null) : FormattingBinaryToggleWord(FormatType.Decorator, "ul", param) { }
public class DotUnderlineWord(string? param = null) : FormattingBinaryToggleWord(FormatType.Decorator, "uld", param) { }
public class DashUnderlineWord(string? param = null) : FormattingBinaryToggleWord(FormatType.Decorator, "uldash", param) { }
public class DashDotUnderlineWord(string? param = null) : FormattingBinaryToggleWord(FormatType.Decorator, "uldashd", param) { }
public class DashDotDotUnderlineWord(string? param = null) : FormattingBinaryToggleWord(FormatType.Decorator, "uldashdd", param) { }
public class DoubleUnderlineWord(string? param = null) : FormattingBinaryToggleWord(FormatType.Decorator, "uldb", param) { }
public class HairlineUnderlineWord(string? param = null) : FormattingBinaryToggleWord(FormatType.Decorator, "ulhair", param) { }
public class HeavyWaveUnderlineWord(string? param = null) : FormattingBinaryToggleWord(FormatType.Decorator, "ulhwave", param) { }
public class LongDashUnderlineWord(string? param = null) : FormattingBinaryToggleWord(FormatType.Decorator, "ulldash", param) { }
public class ThickUnderlineWord(string? param = null) : FormattingBinaryToggleWord(FormatType.Decorator, "ulth", param) { }
public class ThickDotUnderlineWord(string? param = null) : FormattingBinaryToggleWord(FormatType.Decorator, "ulthd", param) { }
public class ThickDashUnderlineWord(string? param = null) : FormattingBinaryToggleWord(FormatType.Decorator, "ulthdash", param) { }
public class ThickDashDotUnderlineWord(string? param = null) : FormattingBinaryToggleWord(FormatType.Decorator, "ulthdashd", param) { }
public class ThickDashDotDotUnderlineWord(string? param = null) : FormattingBinaryToggleWord(FormatType.Decorator, "ulthdashdd", param) { }
public class ThickLongDashUnderlineWord(string? param = null) : FormattingBinaryToggleWord(FormatType.Decorator, "ulthldash", param) { }
public class DoubleWaveUnderlineWord(string? param = null) : FormattingBinaryToggleWord(FormatType.Decorator, "ululdbwave", param) { }
public class WordUnderlineWord(string? param = null) : FormattingBinaryToggleWord(FormatType.Decorator, "ulw", param) { }
public class WaveUnderlineWord(string? param = null) : FormattingBinaryToggleWord(FormatType.Decorator, "ulwave", param) { }
