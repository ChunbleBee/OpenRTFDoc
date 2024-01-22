namespace RtfDom;

public enum ListType
{
    List,
    ParagraphNumbering
}

/// <summary>
/// ListNode defines how lists are handled in the DOM.
/// </summary>
public class ListNode : Node
{
    /// <summary>
    /// GenerateParagraphPrefix gets the generates the prefixed text node of the paragraph.
    /// </summary>
    /// <returns>The <see cref="Node"/> created from the generation.</returns>
    public Node GenerateParagraphPrefix()
    {
        GetAttribute();
    }
}