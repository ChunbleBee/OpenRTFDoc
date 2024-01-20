namespace RtfModels;

/// <summary>
/// WordType defines the types of control words in the RTF specification.
/// See <i>Microsoft®️ Office Rich Text Format (RTF) Specification Version 1.9.1</i>, page 231.
/// </summary>
[Flags]
public enum WordType
{
    /// <summary>
    /// Unknown represents control words that haven't been properly interpreted.
    /// </summary>
    UNKNOWN,

    /// <summary>
    /// Symbol represents special characters.
    /// </summary>
    Symbol = 0b0000001,

    /// <summary>
    /// Flag represents control words that ignore parameters.
    /// </summary>
    Flag = 0b0000010,

    /// <summary>
    /// Value represents control words that require parameters.
    /// </summary>
    Value = 0b0000100,

    /// <summary>
    /// Toggle represents control words that toggle a state on or off.
    /// No parameters or a non-zero numeric parameter represent some on state.
    /// A zero parameter will turn the toggle off/to the default state.
    /// </summary>
    Toggle = 0b0001000,

    /// <summary>
    /// Destination represents control words that are either groups or destination
    /// They ignore parameters.
    /// </summary>
    Destination = 0b0100000
}