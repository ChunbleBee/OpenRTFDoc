namespace RtfModels;

/// <summary>
/// ControlWord describes any control word token.
/// </summary>
public abstract class ControlWord
{
    /// <summary>
    /// Gets the <see cref="WordType"/> associated with this control word.
    /// </summary>
    public WordType Type { get; protected set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ControlWord"/> class.
    /// </summary>
    /// <param name="type">The <see cref="WordType"/> of this object.</param>
    public ControlWord(WordType type)
    {
        Type = type;
    }
}

/// <summary>
/// SymbolWord describes a symbol type control word.
/// </summary>
public class SymbolWord : ControlWord
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SymbolWord"/> class.
    /// </summary>
    public SymbolWord() : base(WordType.Symbol) { }
}

/// <summary>
/// FlagWord describes a flag type control word.
/// </summary>
public class FlagWord : ControlWord
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FlagWord"/> class.
    /// </summary>
    public FlagWord() : base(WordType.Flag) { }
}

/// <summary>
/// ValueWord describes a value type control word.
/// </summary>
public class ValueWord : ControlWord
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ValueWord"/> class.
    /// </summary>
    public ValueWord() : base(WordType.Value) { }
}

/// <summary>
/// ToggleWord describes a toggle type control word.
/// </summary>
public class ToggleWord : ControlWord
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ToggleWord"/> class.
    /// </summary>
    public ToggleWord() : base(WordType.Toggle) { }
}

/// <summary>
/// DestinationWord describes a destination or group type control word.
/// </summary>
public class DestinationWord : ControlWord
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DestinationWord"/> class.
    /// </summary>
    public DestinationWord() : base(WordType.Destination) { }
}

/// <summary>
/// UnknownWord describes a unknown type control word.
/// </summary>
public class UnknownWord : ControlWord
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnknownWord"/> class.
    /// </summary>
    public UnknownWord() : base(WordType.Unknown) { }
}