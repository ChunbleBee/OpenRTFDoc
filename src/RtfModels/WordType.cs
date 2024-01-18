namespace RtfModels;

/// <summary>
/// WordType defines the types of control words in the RTF specification.
/// See <i>Microsoft®️ Office Rich Text Format (RTF) Specification Version 1.9.1</i>, page 231.
/// </summary>
public enum WordType
{
    /// <summary>
    /// Unknown represents an unknown control word.
    /// </summary>
    Unknown,

    /// <summary>
    /// Symbol represents special characters.
    /// </summary>
    Symbol,

    /// <summary>
    /// Flag represents control words that ignore parameters.
    /// </summary>
    Flag,

    /// <summary>
    /// Value represents control words that require parameters.
    /// </summary>
    Value,

    /// <summary>
    /// Toggle represents control words that toggle a state on or off.
    /// No parameters or a non-zero numeric parameter represent some on state.
    /// A zero paraemter will turn the toggle off.
    /// </summary>
    Toggle,

    /// <summary>
    /// Destination represents control words that are either groups or destination
    /// They ignnore parameters.
    /// </summary>
    Destination
}