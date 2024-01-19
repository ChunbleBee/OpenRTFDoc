namespace RtfModels;

using System.Collections.Generic;
using System.Text;

/// <summary>
/// Group 
/// </summary>
public class Group : DestinationWord
{
    /// <summary>
    /// Gets the <see cref="IList"/> of children groups associated with this group.
    /// </summary>
    public IList<ControlWord> Children { get; } = [];

    /// <inheritdoc/>
    public override string ToString()
    {
        StringBuilder builder = new();
        builder.Append('{');

        foreach(var child in Children)
        {
            builder.Append(child.ToString());
        }

        builder.Append('}');
        return builder.ToString();
    }
}