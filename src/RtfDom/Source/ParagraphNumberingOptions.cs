namespace RtfDom;

/// <summary>
/// EnumerationType defines the various types of enumeration available for paragraph numbering in RTF.
/// </summary>
public enum EnumerationType
{
    None,
    Cardinal,
    Decimal,
    UpperAlphabetic,
    UpperRoman,
    LowerAlphabetic,
    LowerRoman,
    Ordinal,
    TextOrdinal,
    AbjadJawaz,
    AlifBaTah,
    KatakanaAIUEO01,
    DoubleByteKatakanaAIUEO01,
    KatakanaAIUEO02,
    DoubleByteKatakanaAIUEO02,
    Chosung,
    Circle,
    Kanji01,
    Kanji02,
    Kanji03,
    ArabicDoubleByte,
    Ganada,
    Chinese01,
    Chinese02,
    Chinese03,
    Chinese04,
    KatakanaIROHA,
    KatakanaDoubleByteIROHA,
    Zodiac01,
    Zodiac02,
    Zodiac03
}

/// <summary>
/// IParagraphNumbering 
/// </summary>
public interface IParagraphNumbering
{
    /// <summary>
    /// Gets the <see cref="EnumerationType"/> of this object, defining how the enumeration should be handled.
    /// </summary>
    public EnumerationType EnumerationScheme { get; }
}

public class ParagraphNumberingAttribute(string name, EnumerationType scheme) : DomAttribute(name), IParagraphNumbering
{
    /// <inheritdoc/>
    public EnumerationType EnumerationScheme { get; } = scheme;

    /// <inheritdoc/>
    public FormatType Type { get; } = FormatType.ParagraphNumbering;

    /// <inheritdoc/>
    public virtual bool Apply(ref Node n)
    {
        DomAttribute? attr = node.GetAttribute("ParagraphNumbering");
        if (attr != null && attr is ParagraphNumberingAttribute other)
        {
            if (other.Value == Value)
                return false;
        }

        node.Attributes[Name] = this;
        return true;
    }
}