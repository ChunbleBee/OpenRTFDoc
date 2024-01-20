namespace RtfModels;

using System.Collections.Generic;
using System.Text;

/// <summary>
/// GroupType represents what type of 
/// </summary>
public enum GroupType
{
    /// <summary>
    ///  Default represents non-destination groups.
    /// </summary>
    Default,

    /// <summary>
    /// Local represents destination groups that apply to the next text run.
    /// </summary>
    Local,

    /// <summary>
    /// Global represents global destination groups (font tables, color tables, etc.)
    /// </summary>
    Global
}

/// <summary>
/// Group defines any group of 
/// </summary>
public class Group : IToken
{
    /// <summary>
    /// Gets the <see cref="IList"/> of children groups associated with this group.
    /// </summary>
    public IList<IToken> Children { get; } = [];

    public GroupType Type { get; set; } = GroupType.Default;

    /// <inheritdoc/>
    public override string ToString()
    {
        StringBuilder builder = new();
        builder.Append('{');

        foreach (var child in Children)
        {
            builder.Append(child.ToString());
        }

        builder.Append('}');
        return builder.ToString();
    }
}
