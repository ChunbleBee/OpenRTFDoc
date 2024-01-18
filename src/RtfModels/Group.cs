namespace RtfModels;

using System.Collections.Generic;

/// <summary>
/// Group 
/// </summary>
public class Group : DestinationWord
{
    /// <summary>
    /// Gets the <see cref="IList"/> of children groups associated with this group.
    /// </summary>
    public IList<ControlWord> Children { get; } = [];
}