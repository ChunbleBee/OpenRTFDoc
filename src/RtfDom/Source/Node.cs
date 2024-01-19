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
        List<Node> nodes = new();
        nodes.Add(this);

        foreach (Node child in Children)
        {
            nodes.AddRange(child.Flatten());
        }

        return nodes;
    }

    private Node? parent;
}