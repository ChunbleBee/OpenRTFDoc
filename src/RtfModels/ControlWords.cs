namespace RtfModels;

using System.Text;

/// <summary>
/// IToken is the base interface for all control tokens in the parser
/// </summary>
public interface IToken { }

/// <summary>
/// ControlWord describes any control word token.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ControlWord"/> class.
/// </remarks>
/// <param name="type">The <see cref="WordType"/> of this object.</param>
/// <param name="name">The RTF string name of the control word.</param>
public class ControlWord(WordType type = WordType.UNKNOWN, string name = "", string? param = null) : IToken
{
    /// <summary>
    /// Gets the <see cref="WordType"/> associated with this control word.
    /// </summary>
    public WordType Type { get; protected set; } = type;

    /// <summary>
    /// Gets the rtf name of this control word.
    /// </summary>
    public string Name { get; protected set; } = name;

    /// <summary>
    /// Gets the parameter of this control word.
    /// </summary>
    public string? Param { get; set; } = param;

    /// <summary>
    /// ToString gets the RTF string representation of this control word.
    /// </summary>
    /// <returns>The string representation of this control word.</returns>
    public override string ToString()
    {
        StringBuilder builder = new();
        builder.Append('\\');
        builder.Append(Name);
        builder.Append(Param);
        return builder.ToString();
    }
}

/// <summary>
/// SymbolWord describes a symbol type control word.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="SymbolWord"/> class.
/// </remarks>
public class SymbolWord(string name = "") : ControlWord(WordType.Symbol, name)
{
}

/// <summary>
/// FlagWord describes a flag type control word.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="FlagWord"/> class.
/// </remarks>
public class FlagWord(string name = "") : ControlWord(WordType.Flag, name)
{
}

/// <summary>
/// ValueWord describes a value type control word.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ValueWord"/> class.
/// </remarks>
public class ValueWord(string name = "", string param = "") : ControlWord(WordType.Value, name, param)
{
}

/// <summary>
/// ToggleWord describes a toggle type control word.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ToggleWord"/> class.
/// </remarks>
public class ToggleWord(string name = "", string? param = null) : ControlWord(WordType.Toggle, name, param)
{
}

/// <summary>
/// DestinationWord describes a destination type control word.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="DestinationWord"/> class.
/// </remarks>
public class DestinationWord(string name = "", string? param = null, bool global = false) : ControlWord(WordType.Destination, name, param)
{
    /// <summary>
    /// Gets a value that determines whether or not this destination control word is globally applied.
    /// </summary>
    public bool IsGlobal { get; } = global;
}

/// <summary>
/// BinaryToggleWord describes a toggle <see cref="ControlWord"/> that can only be on or off.
/// </summary>
/// <param name="name">The name of the word.</param>
/// <param name="param">The parameter of the control word, if any.</param>
public class BinaryToggleWord(string name = "", string? param = null) : ToggleWord(name, (param != "0") ? null : param)
{
}
