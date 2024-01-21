namespace RtfDom;

using System.Drawing;
using RtfModels;

public struct DefaultKey
{
    public string ColorDefault { get; }
}

/// <summary>
/// DomAttribute describes some attribute that a node may have.
/// </summary>
public abstract class DomAttribute(string name)
{
    /// <summary>
    /// The name of the attribute.
    /// </summary>
    public string Name { get; } = name;
}

/// <summary>
/// RefDomAttribute describes some attribute that a node may have that references document global attributes.
/// </summary>
/// <typeparam name="T">The global reference type.</typeparam>
/// <param name="name">The name of the </param>
/// <param name="doc">The <see cref="DocumentNode"/> to reference from.</param>
/// <param name="globalID">The reference value to use to find the global attribute.</param>
public abstract class RefDomAttribute<T>(string name, DocumentNode doc, T globalID) : DomAttribute(name)
{
    /// <summary>
    /// Gets the ID of the attribute to reference in the <see cref="DocumentNode"/>.
    /// </summary>
    public T GlobalID { get; } = globalID;

    /// <summary>
    /// Gets the document that this attribute should reference to.
    /// </summary>
    public DocumentNode Document { get; } = doc;
}

/// <summary>
/// PlainAttribute defines the next run of text as having default formatting.
/// </summary>
/// <param name="doc">The <see cref="DocumentNode"/> that default formatting should be pulled from.</param>
public class PlainAttribute(DocumentNode doc) : RefDomAttribute<string?>("Plain", doc, null), IFormatOption
{
    /// <summary>
    /// Value is the internal value of the 
    /// </summary>
    public FormatList Value { get { return Document.GetDefaultFormatting(); } }
    public FormatType Type { get; } = FormatType.AllFormatting;

    /// <inheritdoc/>
    public bool Apply(ref Node node)
    {
        return Value.Apply(ref node);
    }
}

/// <summary>
/// ColorAttribute describes a <see cref="Color"/> to apply to the <see cref="Node"/>.
/// </summary>
/// <param name="globalID">The global color table <see cref="Guid"/>.</param>
public abstract class ColorAttribute(string gid, DocumentNode doc) : RefDomAttribute<string>("Color", doc, gid), IFormatOption
{
    public Color Value { get { return Document.ColorTable.GetValueOrDefault(GlobalID, Document.ColorTable["0"]); } }

    public FormatType Type { get; } = FormatType.Color;

    public bool Apply(ref Node node)
    {
        if (node.Attributes.TryGetValue(Name, out DomAttribute? attr) && attr != null && attr is ColorAttribute other)
        {
            if (other.Value == Value) return false;
        }
        node.Attributes[Name] = this;
        return true;
    }
}

/// <summary>
/// BoldAttribute defines the bold value of the node.
/// </summary>
/// <param name="on">True if the next run of text should be bold, false otherwise.</param>
public class BoldAttribute(bool on) : DomAttribute("Bold"), IFormatOption
{
    public bool Value = on;
    public FormatType Type { get; } = FormatType.Decorator;

    public bool Apply(ref Node node)
    {
        if (node.Attributes.TryGetValue(Name, out DomAttribute? attr) && attr != null && attr is BoldAttribute other)
        {
            if (other.Value == Value) return false;
        }

        node.Attributes[Name] = this;
        return true;
    }
}

/// <summary>
/// UnderlinedAttribute defines the bold value of the node.
/// </summary>
/// <param name="on">True if the next run of text should be underlined, false otherwise.</param>
public class UnderlinedAttribute(bool on) : DomAttribute("Underlined"), IFormatOption
{
    public bool Value = on;
    public FormatType Type { get; } = FormatType.Decorator;

    public bool Apply(ref Node node)
    {
        if (node.Attributes.TryGetValue(Name, out DomAttribute? attr) && attr != null && attr is UnderlinedAttribute other)
        {
            if (other.Value == Value) return false;
        }

        node.Attributes[Name] = this;
        return true;
    }
}

/// <summary>
/// SuperscriptAttribute defines the bold value of the node.
/// </summary>
/// <param name="on">True if the next run of text should be superscript, false otherwise.</param>
public class SuperscriptAttribute(bool on) : DomAttribute("Superscript"), IFormatOption
{
    public bool Value = on;
    public FormatType Type { get; } = FormatType.Decorator;

    public bool Apply(ref Node node)
    {
        if (node.Attributes.TryGetValue(Name, out DomAttribute? attr) && attr != null && attr is SuperscriptAttribute other)
        {
            if (other.Value == Value) return false;
        }

        node.Attributes[Name] = this;
        return true;
    }
}

/// <summary>
/// SubscriptAttribute defines the bold value of the node.
/// </summary>
/// <param name="on">True if the next run of text should be subscripted, false otherwise.</param>
public class SubscriptAttribute(bool on) : DomAttribute("Subscript"), IFormatOption
{
    public bool Value = on;
    public FormatType Type { get; } = FormatType.Decorator;

    public bool Apply(ref Node node)
    {
        if (node.Attributes.TryGetValue(Name, out DomAttribute? attr) && attr != null && attr is SubscriptAttribute other)
        {
            if (other.Value == Value) return false;
        }

        node.Attributes[Name] = this;
        return true;
    }
}

/// <summary>
/// StrikeThroughAttribute defines the strike through value of the node.
/// </summary>
/// <param name="on">True if the next run of text should be strikethrough, false otherwise.</param>
public class StrikeThroughAttribute(bool on) : DomAttribute("StrikeThrough"), IFormatOption
{
    public bool Value = on;
    public FormatType Type { get; } = FormatType.Decorator;

    public bool Apply(ref Node node)
    {
        if (node.Attributes.TryGetValue(Name, out DomAttribute? attr) && attr != null && attr is StrikeThroughAttribute other)
        {
            if (other.Value == Value) return false;
        }

        node.Attributes[Name] = this;
        return true;
    }
}

/// <summary>
/// ItalicsAttribute defines the italicization value of the node.
/// </summary>
/// <param name="on">True if the next run of text should be italicized, false otherwise.</param>
public class ItalicsAttribute(bool on) : DomAttribute("Italics"), IFormatOption
{
    public bool Value = on;
    public FormatType Type { get; } = FormatType.Decorator;

    public bool Apply(ref Node node)
    {
        if (node.Attributes.TryGetValue(Name, out DomAttribute? attr) && attr != null && attr is ItalicsAttribute other)
        {
            if (other.Value == Value) return false;
        }

        node.Attributes[Name] = this;
        return true;
    }
}

/// <summary>
/// FontAttribute defines the <see cref="FontReference"/> value of the node.
/// </summary>
/// <param name="fontRef">The <see cref="FontReference"/> to use.</param>
public class FontAttribute(string fontRef, DocumentNode doc) : RefDomAttribute<string>("Font", doc, fontRef), IFormatOption
{
    /// <summary>
    /// Gets the <see cref="FontReference"/> associated with th
    /// </summary>
    public FontReference Value { get { return Document.FontTable.GetValueOrDefault(GlobalID, Document.FontTable["0"]); } }

    /// <inheritdoc/>
    public FormatType Type { get; } = FormatType.Decorator;

    /// <inheritdoc/>
    public bool Apply(ref Node node)
    {
        if (node.Attributes.TryGetValue(Name, out DomAttribute? attr) && attr != null && attr is FontAttribute other)
        {
            if (other.GlobalID == GlobalID) return false;
        }

        node.Attributes[Name] = this;
        return true;
    }
}