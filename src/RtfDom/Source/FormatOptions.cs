namespace RtfDom;

using System.Drawing;
using RtfModels;

/// <summary>
/// LineStyleType defines how a line, such as paragraph borders or underlining should be displayed.
/// </summary>
[Flags]
public enum LineStyleType
{
    Filled = 0b0,
    Dot = 0b1,
    DotDot = 0b10,
    Dash = 0b100,
    Hairline = 0b1000,
    Wave = 0b10000,
    Long = 0b100000,
    Thick = 0b1000000,
    Double = 0b10000000,
    Word = 0b100000000,
    DashDot = Dot | Dash,
    DashDotDot = DotDot | Dash,
}

/// <summary>
/// IFormatOption is an interface for any <see cref="DomAttribute"/> that can automatically apply their
/// </summary>
public interface IFormatOption
{
    /// <summary>
    /// Gets the formatting type that the node will apply.
    /// </summary>
    public FormatType Type { get; }

    /// <summary>
    /// Applies the given attribute to the given node.
    /// </summary>
    /// <param name="node">The <see cref="Node"/> to apply the formatting to.</param>
    /// <returns>True if the formatting was applied, false otherwise.</returns>
    public abstract bool Apply(ref Node node);

    /// <summary>
    /// FromFormatWord 
    /// </summary>
    /// <param name="fmt">The <see cref="ControlWord"/> to build the formatting from.</param>
    /// <returns>A new <see cref="DomAttribute"/> built from the formatting word.</returns>
    public static IFormatOption FromFormatWord(IFormat fmt)
    {
        switch (fmt)
        {
            case VerticalTypesetWord word:
                {
                    if (word.State == VerticalTypesetType.None)
                    {
                        return new VerticalTypesettingAttribute(VerticalTypesetType.None);
                    }
                    else if (word.State == VerticalTypesetType.None)
                    {
                        return new VerticalTypesettingAttribute(VerticalTypesetType.None);
                    }
                    else if (word.State == VerticalTypesetType.None)
                    {
                        return new VerticalTypesettingAttribute(VerticalTypesetType.None);
                    }
                    else
                    {
                        throw new ArgumentException($"unknown vertical typesetting value: {word.State}");
                    }
                }
            case BoldWord word: { return new BoldAttribute(word.Param == null || word.Param != "0"); }
            case ItalicsWord word: { return new ItalicsAttribute(word.Param == null || word.Param != "0"); }
            case StrikeThroughWord word: { return new StrikeThroughAttribute(word.Param == null || word.Param != "0"); }
            case UnderlineWord word: { return new UnderlineAttribute(word.Param == null || word.Param != "0", LineStyleType.Filled); }
            case DotUnderlineWord word: { return new UnderlineAttribute(word.Param == null || word.Param != "0", LineStyleType.Dot); }
            case DashUnderlineWord word: { return new UnderlineAttribute(word.Param == null || word.Param != "0", LineStyleType.Dash); }
            case DashDotUnderlineWord word: { return new UnderlineAttribute(word.Param == null || word.Param != "0", LineStyleType.DashDot); }
            case DashDotDotUnderlineWord word: { return new UnderlineAttribute(word.Param == null || word.Param != "0", LineStyleType.DashDotDot); }
            case DoubleUnderlineWord word: { return new UnderlineAttribute(word.Param == null || word.Param != "0", LineStyleType.Double); }
            case HairlineUnderlineWord word: { return new UnderlineAttribute(word.Param == null || word.Param != "0", LineStyleType.Hairline); }
            case HeavyWaveUnderlineWord word: { return new UnderlineAttribute(word.Param == null || word.Param != "0", LineStyleType.Thick | LineStyleType.Wave); }
            case LongDashUnderlineWord word: { return new UnderlineAttribute(word.Param == null || word.Param != "0", LineStyleType.Long | LineStyleType.Dash); }
            case ThickUnderlineWord word: { return new UnderlineAttribute(word.Param == null || word.Param != "0", LineStyleType.Thick); }
            case ThickDotUnderlineWord word: { return new UnderlineAttribute(word.Param == null || word.Param != "0", LineStyleType.Thick | LineStyleType.Dot); }
            case ThickDashUnderlineWord word: { return new UnderlineAttribute(word.Param == null || word.Param != "0", LineStyleType.Thick | LineStyleType.Dash); }
            case ThickDashDotUnderlineWord word: { return new UnderlineAttribute(word.Param == null || word.Param != "0", LineStyleType.Thick | LineStyleType.DashDot); }
            case ThickDashDotDotUnderlineWord word: { return new UnderlineAttribute(word.Param == null || word.Param != "0", LineStyleType.Thick | LineStyleType.DashDotDot); }
            case ThickLongDashUnderlineWord word: { return new UnderlineAttribute(word.Param == null || word.Param != "0", LineStyleType.Thick | LineStyleType.Long | LineStyleType.Dash); }
            case DoubleWaveUnderlineWord word: { return new UnderlineAttribute(word.Param == null || word.Param != "0", LineStyleType.Double | LineStyleType.Wave); }
            case WordUnderlineWord word: { return new UnderlineAttribute(word.Param == null || word.Param != "0", LineStyleType.Word); }
            case WaveUnderlineWord word: { return new UnderlineAttribute(word.Param == null || word.Param != "0", LineStyleType.Wave); }
            default: throw new NotImplementedException($"unknown formatting word: {fmt.GetType()}");
        }
    }
}

/// <summary>
/// FormatList is a list of formatting options.
/// </summary>
public class FormatList : List<IFormatOption>
{
    /// <summary>
    /// Apply applies all <see cref="IFormatOption"/> to the given node.
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public bool Apply(ref Node n)
    {
        bool appliedAll = true;

        foreach (IFormatOption fmt in this)
        {
            appliedAll &= fmt.Apply(ref n);
        }

        return appliedAll;
    }
}

/// <summary>
/// PlainAttribute defines the next run of text as having default formatting.
/// </summary>
/// <param name="doc">The <see cref="DocumentNode"/> that default formatting should be pulled from.</param>
public class PlainAttribute(DocumentNode doc, string? nameOverride = null) : RefDomAttribute<string?>(nameOverride ?? AttributeConstants.Plain, doc, null), IFormatOption
{
    /// <summary>
    /// Gets the <see cref="FormatList"/> of all default formatting in the document.
    /// </summary>
    public FormatList Value { get { return Document.GetDefaultFormatting(); } }

    /// <inheritdoc/>
    public FormatType Type { get; } = FormatType.AllFormatting;

    /// <inheritdoc/>
    public bool Apply(ref Node node)
    {
        return Value.Apply(ref node);
    }
}

/// <summary>
/// ColorAttribute describes a <see cref="Color"/> to apply to the <see cref="Node"/>.
/// </summary>
/// <remarks>Initializes a new instance of the <see cref="ColorAttribute"/> class.</remark>
/// <param name="globalID">The global color table <see cref="Guid"/>.</param>
public abstract class ColorAttribute(string gid, DocumentNode doc, string? nameOverride = null) : RefDomAttribute<string>(nameOverride ?? AttributeConstants.Color, doc, gid), IFormatOption
{
    /// <summary>
    /// Gets the color value associated with this attribute.
    /// If it's not found in the color table, the default color is used.
    /// </summary>

    public Color Value { get { return Document.ColorTable.GetValueOrDefault(GlobalID, Document.ColorTable["0"]); } }

    /// <inheritdoc/>
    public FormatType Type { get; } = FormatType.Color;

    /// <inheritdoc/>
    public bool Apply(ref Node node)
    {
        if (node.Attributes.TryGetValue(Name, out DomAttribute? attr) && attr != null && attr is ColorAttribute other)
        {
            if (other.Value == Value) return false;
        }
        node.Attributes[Name] = this;
        return true;
    }
}

/// <summary>
/// BoldAttribute defines the bold value of the node.
/// </summary>
/// <remarks>Initializes a new instance of the <see cref="BoldAttribute"/> class.</remark>
/// <param name="on">True if the next run of text should be bold, false otherwise.</param>
public class BoldAttribute(bool on, string? nameOverride = null) : DomAttribute(nameOverride ?? AttributeConstants.Bold), IFormatOption
{
    /// <summary>
    /// Gets a value indicating whether italics should be turned on.
    /// </summary>
    public bool Value { get; } = on;

    /// <inheritdoc/>
    public FormatType Type { get; } = FormatType.Decorator;

    /// <inheritdoc/>
    public bool Apply(ref Node node)
    {
        DomAttribute? attr = node.GetAttribute(Name);
        if (attr != null && attr is BoldAttribute other)
        {
            if (other.Value == Value) return false;
        }

        node.Attributes[Name] = this;
        return true;
    }
}

/// <summary>
/// ItalicsAttribute defines the italicization value of the node.
/// </summary>
/// <remarks>Initializes a new instance of the <see cref="ItalicsAttribute"/> class.</remark>
/// <param name="on">True if the next run of text should be italicized, false otherwise.</param>
public class ItalicsAttribute(bool on, string? nameOverride = null) : DomAttribute(nameOverride ?? AttributeConstants.Italics), IFormatOption
{
    /// <summary>
    /// Gets a value indicating whether italics should be turned on.
    /// </summary>
    public bool Value { get; } = on;

    /// <inheritdoc/>
    public FormatType Type { get; } = FormatType.Decorator;

    /// <inheritdoc/>
    public bool Apply(ref Node node)
    {
        DomAttribute? attr = node.GetAttribute(Name);
        if (attr != null && attr is ItalicsAttribute other)
        {
            if (other.Value == Value) return false;
        }

        node.Attributes[Name] = this;
        return true;
    }
}

/// <summary>
/// UnderlineAttribute defines the bold value of the node.
/// </summary>
/// <remarks>Initializes a new instance of the <see cref="UnderlineAttribute"/> class.</remark>
/// <param name="on">True if the next run of text should be underlined, false otherwise.</param>
public class UnderlineAttribute(bool on, LineStyleType type = LineStyleType.Filled, string? nameOverride = null) : DomAttribute(nameOverride ?? AttributeConstants.Underline), IFormatOption
{
    /// <summary>
    /// Gets a value indicating whether text underlining should be turned on.
    /// </summary>
    public bool Value { get; } = on;

    /// <inheritdoc/>
    public FormatType Type { get; } = FormatType.Decorator;

    /// <summary>
    /// Gets the <see cref="RtfDom.LineStyleType"/> of this 
    /// </summary>
    public LineStyleType UnderlineType { get; } = type;

    /// <inheritdoc/>
    public bool Apply(ref Node node)
    {
        DomAttribute? attr = node.GetAttribute(Name);
        if (attr != null && attr is UnderlineAttribute other)
        {
            if (other.Value == Value && other.UnderlineType == UnderlineType) return false;
        }

        node.Attributes[Name] = this;
        return true;
    }
}

/// <summary>
/// StrikeThroughAttribute defines the strike through value of the node.
/// </summary>
/// <remarks>Initializes a new instance of the <see cref="StrikeThroughAttribute"/> class.</remark>
/// <param name="on">True if the next run of text should be strikethrough, false otherwise.</param>
public class StrikeThroughAttribute(bool on, string? nameOverride = null) : DomAttribute(nameOverride ?? AttributeConstants.StrikeThrough), IFormatOption
{
    /// <summary>
    /// Gets a value indicating whether text strikethrough should be turned on.
    /// </summary>
    public bool Value { get; } = on;

    /// <inheritdoc/>
    public FormatType Type { get; } = FormatType.Decorator;

    /// <inheritdoc/>
    public bool Apply(ref Node node)
    {
        DomAttribute? attr = node.GetAttribute(Name);
        if (attr != null && attr is StrikeThroughAttribute other)
        {
            if (other.Value == Value) return false;
        }

        node.Attributes[Name] = this;
        return true;
    }
}

/// <summary>
/// VerticalTypesettingAttribute defines the vertical typesetting of the node.
/// </summary>
/// <remarks>Initializes a new instance of the <see cref="VerticalTypesettingAttribute"/> class.</remark>
/// <param name="value">The <see cref="VerticalTypesetType"/> to use for the node.</param>
public class VerticalTypesettingAttribute(VerticalTypesetType value, string? nameOverride = null) : DomAttribute(nameOverride ?? AttributeConstants.VerticalTypesetting), IFormatOption
{
    /// <summary>
    /// Gets the <see cref="VerticalTypesetType"/> associated with this attribute.
    /// </summary>
    public VerticalTypesetType Value { get; } = value;

    /// <inheritdoc/>
    public FormatType Type { get; } = FormatType.Decorator;

    /// <inheritdoc/>
    public bool Apply(ref Node node)
    {
        DomAttribute? attr = node.GetAttribute(AttributeConstants.VerticalTypesetting);
        if (attr != null && attr is VerticalTypesettingAttribute other)
        {
            if (other.Value == Value)
                return false;
        }

        node.Attributes[Name] = this;
        return true;
    }
}

/// <summary>
/// FontAttribute defines the <see cref="FontReference"/> value of the node.
/// </summary>
/// <param name="fontRef">The <see cref="FontReference"/> to use.</param>
public class FontAttribute(string fontRef, DocumentNode doc, string? nameOverride = null) : RefDomAttribute<string>(nameOverride ?? AttributeConstants.Font, doc, fontRef), IFormatOption
{
    /// <summary>
    /// Gets the <see cref="FontReference"/> associated with this font ref id.
    /// </summary>
    public FontReference Value { get { return Document.FontTable.GetValueOrDefault(GlobalID, Document.FontTable["default"]); } }

    /// <inheritdoc/>
    public FormatType Type { get; } = FormatType.Decorator;

    /// <inheritdoc/>
    public bool Apply(ref Node node)
    {
        DomAttribute? attr = node.GetAttribute(Name);
        if (attr != null && attr is FontAttribute other)
        {
            if (other.GlobalID == GlobalID) return false;
        }

        node.Attributes[Name] = this;
        return true;
    }
}