namespace RtfModels;

/// <summary>
/// IString is the base interface for all tokens that directly convert to strings
/// \uN, \'XX, \tab, and plain text runs fall into this category.
/// </summary>
public interface IString : IToken
{
    /// <summary>
    /// Convert converts the token into it's plain text string.
    /// </summary>
    /// <returns>the string of this </returns>
    public string Convert();
}

/// <summary>
/// Run represents a length of unbroken text.
/// </summary>
public class Run(string text = "") : IString
{
    /// <summary>
    /// Gets or sets the inner text of this token.
    /// </summary>
    public string InnerText { get; set; } = text;

    /// <inheritdoc/>
    public override string ToString()
    {
        return ((InnerText?[0] != ';') ? " " : string.Empty) + InnerText;
    }

    /// <inheritdoc/>
    public string Convert()
    {
        return InnerText;
    }
}

/// <summary>
/// StringValueWord represents control words that have parameters, but evaluate directly to strings.
/// </summary>
public abstract class StringValueWord : ValueWord, IString
{
    public abstract string Convert();
    public StringValueWord(string name = "", string param = "") : base(name, param)
    {
        Type = WordType.Value & WordType.Symbol;
    }
}

/// <summary>
/// UnicodeStringWord represents unicode characters.
/// </summary>
/// <param name="param">The unicode code of for the character.</param>
public class UnicodeStringWord(string param) : StringValueWord("u", param)
{
    public override string Convert()
    {
        return $"{System.Convert.ToChar($"\\u{Param}")}";
    }
}

/// <summary>
/// AsciiStringWord represents an ascii codepage character.
/// </summary>
/// <param name="param">The ascii code of for the character as a string.</param>
public class AsciiStringWord(string param) : StringValueWord("u", param)
{
    /// <inheritdoc/>
    public override string Convert()
    {
        return $"{System.Convert.ToChar($"\\u{Param}")}";
    }
}

/// <summary>
/// StringSymbolWord represents control words that evaluate directly to strings.
/// </summary>
public class StringSymbolWord(string name, string value) : SymbolWord(name), IString
{
    public string Value { get; } = value;
    public string Convert()
    {
        return Value;
    }

    /// <summary>
    /// LineWord represents the paragraph break character in the RTF format.
    /// </summary>
    public static StringSymbolWord LineWord { get; } = new("line", "\n");

    /// <summary>
    /// ParWord represents the paragraph break character in the RTF format.
    /// </summary>
    public static StringSymbolWord ParagraphWord { get; } = new("par", "\u2029");

    /// <summary>
    /// TabWord represents the tab character in the RTF format.
    /// </summary>
    public static StringSymbolWord TabWord { get; } = new("tab", "\t");

    /// <summary>
    /// EnDashWord represents the endash character in the RTF format.
    /// </summary>
    public static StringSymbolWord EnDashWord { get; } = new("emdash", "\u2013");

    /// <summary>
    /// EmDashWord represents the emdash character in the RTF format.
    /// </summary>
    public static StringSymbolWord EmDashWord { get; } = new("emdash", "\u2014");

    /// <summary>
    /// EnSpaceWord represents the enspace character in the RTF format.
    /// </summary>
    public static StringSymbolWord EnSpaceWord { get; } = new("enspace", "\u2002");

    /// <summary>
    /// EmSpaceWord represents the emspace character in the RTF format.
    /// </summary>
    public static StringSymbolWord EmSpaceWord { get; } = new("emspace", "\u2003");

    /// <summary>
    /// QuarterEmSpaceWord represents the four-per-em space character in the RTF format.
    /// </summary>
    public static StringSymbolWord QuarterEmSpaceWord { get; } = new("qmspace", "\u2005");

    /// <summary>
    /// BulletWord represents the bullet character in the RTF format.
    /// </summary>
    public static StringSymbolWord BulletWord { get; } = new("bullet", "\u2022");

    /// <summary>
    /// LeftQuoteWord represents the left-quote character in the RTF format.
    /// </summary>
    public static StringSymbolWord LeftQuoteWord { get; } = new("lquote", "\u2029");

    /// <summary>
    /// RightQuoteWord represents the right-quote character in the RTF format.
    /// </summary>
    public static StringSymbolWord RightQuoteWord { get; } = new("rquote", "\u202A");

    /// <summary>
    /// LeftDoubleQuoteWord represents the left-double-quote character in the RTF format.
    /// </summary>
    public static StringSymbolWord LeftDoubleQuoteWord { get; } = new("ldblquote", "\u201C");

    /// <summary>
    /// RightDoubleQuoteWord represents the right-double-quote character in the RTF format.
    /// </summary>
    public static StringSymbolWord RightDoubleQuoteWord { get; } = new("rdblquote", "\u201D");

    /// <summary>
    /// NonBreakingSpaceWord represents the non-break space character in the RTF format.
    /// </summary>
    public static StringSymbolWord NonBreakingSpaceWord { get; } = new("~", "\u00A0");

    /// <summary>
    /// OpenCurlyBraceWord represents the plain text open curly brace character.
    /// </summary>
    public static StringSymbolWord OpenCurlyBraceWord { get; } = new("{", "{");

    /// <summary>
    /// ClosingCurlyBraceWord represents the plain text closing curly brace character.
    /// </summary>
    public static StringSymbolWord ClosingCurlyBraceWord { get; } = new("}", "}");
}