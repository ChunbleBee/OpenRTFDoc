namespace RtfDom;

using System.Collections;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Node represents the base class of the RTF DOM.
/// </summary>
public class Node(DocumentNode? doc = null, Node? parent = null) : IEnumerator<Node>, IEnumerable<Node>
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
    /// 
    /// </summary>
    public DocumentNode? Document { get; } = doc;

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

    private Node? parent = parent;

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

    public virtual DomAttribute? GetAttribute(string attrName)
    {
        Attributes.TryGetValue(attrName, out DomAttribute? attr);
        if (attr != null)
        {
            return attr;
        }

        if (Parent == null)
        {
            return null;
        }

        foreach (Node n in Parent.Children)
        {
            n.Attributes.TryGetValue(attrName, out attr);
        }

        return attr ??= Parent.GetAttribute(attrName);
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

    public override string ToString()
    {
        return ToString(0);
    }

    internal string ToString(int depth = 0)
    {
        StringBuilder builder = new();

        string prependLvl1 = string.Empty.PadLeft(depth);
        string prependLvl2 = string.Empty.PadLeft(depth + 2);
        string prependLvl3 = string.Empty.PadLeft(depth + 4);

        builder.AppendLine($"{prependLvl1}Node: {GetType()}");
        builder.AppendLine($"{prependLvl2}Attributes:");
        foreach (KeyValuePair<string, DomAttribute> kvp in Attributes)
        {
            builder.AppendLine($"{prependLvl3}{kvp.Key}: {kvp.Value.ToString()}");
        }

        builder.AppendLine($"{prependLvl2}Children:");
        foreach (Node child in Children)
        {
            Console.WriteLine("here");
            builder.AppendLine(child.ToString(depth + 4));
        }

        return builder.ToString();
    }
}