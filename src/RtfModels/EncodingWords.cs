namespace RtfModels;

using System.Text;

/// <summary>
/// EncodingWord describes an interface for <see cref="ControlWord"/>
/// that alter the code pages of the document.
/// </summary>
public interface IEncodingWord
{
    /// <summary>
    /// Gets the current character encoding of the control word.
    /// </summary>
    public Encoding EncodingProvider { get; }
}

/// <summary>
/// Initializes a new instance of the <see cref="EncodingFlagWord"/> class.
/// </summary>
/// <param name="name">The name of this control word.</param>
/// <param name="encoding">The <see cref="System.Text.Encoding"/> to use.</param>
public abstract class EncodingFlagWord(string name = "", Encoding? encoding = null) : DestinationWord(name, null, true), IEncodingWord
{
    /// <inheritdoc/>
    public Encoding EncodingProvider { get; } = encoding ?? Encoding.Default;
}

/// <summary>
/// EncodingValueWord represents an encoding token.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="EncodingValuesWord"/> class.
/// </remark>
/// <param name="name">The name of this control word.</param>
/// <param name="encoding">The parameter of the control word.</param>
public abstract class EncodingValueWord : DestinationWord, IEncodingWord
{
    /// <inheritdoc/>
    public Encoding EncodingProvider { get; }

    public EncodingValueWord(string name = "", string param = "") : base(name, param, true)
    {
        EncodingProvider = Encoding.GetEncoding(Int32.Parse(param));
        Type = WordType.Value & WordType.Destination;
    }
}

/// <summary>
/// AnsiWord describes a <see cref="ControlWord"/> for the ANSI/Windows encoding standards.
/// </summary>
public class AnsiWord() : EncodingFlagWord("ansi") { }

/// <summary>
/// MacWord describes a <see cref="ControlWord"/> for the Apple Macintosh encoding standards.
/// </summary>
public class MacWord() : EncodingFlagWord("mac", Encoding.GetEncoding(10000)) { }

/// <summary>
/// PcWord describes a <see cref="ControlWord"/> for the IBM PC encoding standards (code page 437).
/// </summary>
public class PcWord() : EncodingFlagWord("pc", Encoding.GetEncoding(437)) { }

/// <summary>
/// Pc2Word describes a <see cref="ControlWord"/> for the IBM OS/2 encoding standards (code page 850).
/// </summary>
public class Pc2Word() : EncodingFlagWord("pca", Encoding.GetEncoding(850)) { }

/// <summary>
/// AnsiCodePageWord describes a <see cref="ControlWord"/> for a specific encoding page.
/// </summary>
/// <param name="param"></param>
public class AnsiCodePageWord(string param = "") : EncodingValueWord("ansicpg", param) { }