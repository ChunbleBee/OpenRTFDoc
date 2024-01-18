namespace RtfModels;

/// <summary>
/// ControlWord describes any control word token.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ControlWord"/> class.
/// </remarks>
/// <param name="type">The <see cref="WordType"/> of this object.</param>
/// <param name="name">The RTF string name of the control word.</param>
public class ControlWord(WordType type, string name = "", string? param = null)
{
    /// <summary>
    /// Gets the <see cref="WordType"/> associated with this control word.
    /// </summary>
    public WordType Type { get; protected set; } = type;

    public string Name { get; protected set; } = name;

    public string Param { get; set; } = param;
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
/// DestinationWord describes a destination or group type control word.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="DestinationWord"/> class.
/// </remarks>
public class DestinationWord(string name = "", string? param = null) : ControlWord(WordType.Destination, name, param)
{
}

/// <summary>
///TextnWord describes a plain text run.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TextWord"/> class.
/// </remarks>
public class TextWord(string text) : ControlWord(WordType.Text, "", text)
{
}