namespace RtfDom;

using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Node represents the base class of the RTF DOM.
/// </summary>
public class Node : IEnumerator<Node>, IEnumerable<Node>
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

    /// <inheritdoc/>
    public Node Current => Children[index];

    /// <summary>
    /// Attributes is the <see cref="Dictionary"/> of formatting attributes applied to this node.
    /// </summary>
    internal Dictionary<string, DomAttribute> Attributes { get; set; } = [];

    object IEnumerator.Current => throw new NotImplementedException();

    private int index = 0;

    private Node? parent;

    /// <summary>
    /// Initializes a new instance of the <see cref="Node"/> class.
    /// </summary>
    public Node() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Node"/> class.
    /// </summary>
    /// <param name="parent">The <see cref="Node"/> to set as the parent node.</param>
    public Node(Node parent)
    {
        Parent = parent;
        Attributes = parent.Attributes;
    }

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
        return Attributes.TryGetValue(other.Name, out DomAttribute? ours) && ours == other;
    }

    /// <summary>
    /// AddAttribute adds the given attribute to the node.
    /// </summary>
    /// <param name="attr">The <see cref="DomAttribute"/> to add to the node.</param>
    public void AddAttribute(DomAttribute attr)
    {
        Attributes[attr.Name] = attr;
    }

    /// <inheritdoc/>
    public IEnumerator<Node> GetEnumerator()
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

        return (++index) > 0;
    }

    /// <inheritdoc/>
    public void Reset()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    protected virtual void Dispose(bool isDisposing)
    {
        index = 0;
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}