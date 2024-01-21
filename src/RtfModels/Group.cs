namespace RtfModels;

using System.Collections;
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
public class Group : IToken, IEnumerator<IToken>, IEnumerable<IToken>
{
    /// <summary>
    /// Gets the <see cref="IList"/> of children groups associated with this group.
    /// </summary>
    public IList<IToken> Children { get; } = [];

    private int index = 0;

    /// <summary>
    /// Gets or sets the type of group.
    /// </summary>
    public GroupType Type { get; set; } = GroupType.Default;

    /// <inheritdoc/>
    public IToken Current => Children[index];

    /// <inheritdoc>/>
    object IEnumerator.Current => Children[index];

    /// <inheritdoc>/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc>/>
    public IEnumerator<IToken> GetEnumerator()
    {
        return Children.GetEnumerator();
    }

    /// <inheritdoc/>
    public bool MoveNext()
    {
        if (index + 1 >= Children.Count)
        {
            return false;
        }

        index++;
        return true;
    }

    /// <inheritdoc/>
    public void Reset()
    {
        index = 0;
    }

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

    /// <inheritdoc/>
    protected virtual void Dispose(bool isDisposing)
    {
        index = 0;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
