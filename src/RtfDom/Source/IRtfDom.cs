namespace RtfDom;

/// <summary>
/// IRtfDom describes any entity that can be returned to an RTF token, and (thus) into an RTF string.
/// </summary>
public interface IRtfDom
{
    public List<IToken> ToRtfToken();
}