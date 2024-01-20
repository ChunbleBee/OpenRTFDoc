namespace RtfDom;

using System.Collections.Generic;

/// <summary>
/// Node represents the base class of the RTF DOM.
/// </summary>
public class Node
{
    /// <summary>
    /// Gets or sets the parent of this node.
    /// Setting the parent will remove this node from any previous parent.
    /// </summary>
    public Node? Parent
    {
        get { return parent; }
        set
        {
            parent?.Children.Remove(this);
            value?.Children.Add(this);
            parent = value;
        }
    }

    /// <summary>
    /// Gets the list of <see cref="Node"/> that are direct descendants of this.
    /// </summary>
    public IList<Node> Children { get; } = [];


    internal Dictionary<string, DomAttribute> Attributes { get; set; } = [];

    /// <summary>
    /// Gets or sets the inner text of this node.
    /// </summary>
    public string InnerText { get; set; } = string.Empty;

    /// <summary>
    /// Flatten recursively traverses the DOM tree in preorder, and returns the flattened list.
    /// </summary>
    /// <returns>The list of <see cref="Node"/> traversed from this node.</returns>
    public IList<Node> Flatten()
    {
        List<Node> nodes = [this];

        foreach (Node child in Children)
        {
            nodes.AddRange(child.Flatten());
        }

        return nodes;
    }

    /// <summary>
    /// HasAttribute returns whether or not this node has the given <see cref="DomAttribute"/>.
    /// </summary>
    /// <returns>True if the node contains the attribute, false otherwise.</returns>
    public bool HasAttribute(DomAttribute other)
    {
        return Attributes.TryGetValue(other.Name, out DomAttribute? ours);
    }

    /// <summary>
    /// HasAttributeValue returns whether or not this node contains the given <see cref="DomAttribute"/>
    /// and if the <see cref="DomAttribute.Value"/> are a match.
    /// </summary>
    /// <param name="other"></param>
    /// <returns>True if the node contains the attribute with the same values, false otherwise.</returns>
    public bool HasAttributeValue(DomAttribute other)
    {
        return Attributes.TryGetValue(other.Name, out DomAttribute? ours) && ours.Value == other.Value;
    }

    /// <summary>
    /// AddAttribute adds the given attribute to the node.
    /// </summary>
    /// <param name="attr">The <see cref="DomAttribute"/> to add to the node.</param>
    public void AddAttribute(DomAttribute attr)
    {
        Attributes[attr.Name] = attr;
    }

    private Node? parent;
}