namespace RtfModels;

/// <summary>
/// Run represents a length of unbroken text.
/// </summary>
public class Run(string text = "") : IToken
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
}