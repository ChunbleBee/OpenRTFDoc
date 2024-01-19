namespace RtfModels;

using System.Collections.Generic;
using System.Text;

/// <summary>
/// Group defines any group of 
/// </summary>
public class Group : IToken
{
    /// <summary>
    /// Gets the <see cref="IList"/> of children groups associated with this group.
    /// </summary>
    public IList<IToken> Children { get; } = [];

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

/// <summary>
/// DestinationGroup
/// </summary>
public class DestinationGroup : Group
{

}