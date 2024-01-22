namespace RtfDom;

using System.Drawing;
using RtfModels;

/// <summary>
/// DomAttribute describes some attribute that a node may have.
/// </summary>
/// <remarks>Initializes a new instance of the <see cref="DomAttribute"/> class.</remark>
/// <param name="name">The name of the </param>
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
/// <remarks>Initializes a new instance of the <see cref="RtfDomAttribute"/> class.</remark>
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
    /// Gets the <see cref="FormatList"/> of all default formatting in the document.
    /// </summary>
    public FormatList Value { get { return Document.GetDefaultFormatting(); } }

    /// <inheritdoc/>
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
/// <remarks>Initializes a new instance of the <see cref="ColorAttribute"/> class.</remark>
/// <param name="globalID">The global color table <see cref="Guid"/>.</param>
public abstract class ColorAttribute(string gid, DocumentNode doc) : RefDomAttribute<string>("Color", doc, gid), IFormatOption
{
    /// <summary>
    /// Gets the color value associated with this attribute.
    /// If it's not found in the color table, the default color is used.
    /// </summary>

    public Color Value { get { return Document.ColorTable.GetValueOrDefault(GlobalID, Document.ColorTable["0"]); } }

    /// <inheritdoc/>
    public FormatType Type { get; } = FormatType.Color;

    /// <inheritdoc/>
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
/// <remarks>Initializes a new instance of the <see cref="BoldAttribute"/> class.</remark>
/// <param name="on">True if the next run of text should be bold, false otherwise.</param>
public class BoldAttribute(bool on) : DomAttribute("Bold"), IFormatOption
{
    /// <summary>
    /// Gets a value indicating whether italics should be turned on.
    /// </summary>
    public bool Value { get; } = on;

    /// <inheritdoc/>
    public FormatType Type { get; } = FormatType.Decorator;

    /// <inheritdoc/>
    public bool Apply(ref Node node)
    {
        DomAttribute? attr = node.GetAttribute(Name);
        if (attr != null && attr is BoldAttribute other)
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
/// <remarks>Initializes a new instance of the <see cref="ItalicsAttribute"/> class.</remark>
/// <param name="on">True if the next run of text should be italicized, false otherwise.</param>
public class ItalicsAttribute(bool on) : DomAttribute("Italics"), IFormatOption
{
    /// <summary>
    /// Gets a value indicating whether italics should be turned on.
    /// </summary>
    public bool Value { get; } = on;

    /// <inheritdoc/>
    public FormatType Type { get; } = FormatType.Decorator;

    /// <inheritdoc/>
    public bool Apply(ref Node node)
    {
        DomAttribute? attr = node.GetAttribute(Name);
        if (attr != null && attr is ItalicsAttribute other)
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
/// <remarks>Initializes a new instance of the <see cref="UnderlinedAttribute"/> class.</remark>
/// <param name="on">True if the next run of text should be underlined, false otherwise.</param>
public class UnderlinedAttribute(bool on) : DomAttribute("Underlined"), IFormatOption
{
    /// <summary>
    /// Gets a value indicating whether text underlining should be turned on.
    /// </summary>
    public bool Value { get; } = on;

    /// <inheritdoc/>
    public FormatType Type { get; } = FormatType.Decorator;

    /// <inheritdoc/>
    public bool Apply(ref Node node)
    {
        DomAttribute? attr = node.GetAttribute(Name);
        if (attr != null && attr is UnderlinedAttribute other)
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
/// <remarks>Initializes a new instance of the <see cref="StrikeThroughAttribute"/> class.</remark>
/// <param name="on">True if the next run of text should be strikethrough, false otherwise.</param>
public class StrikeThroughAttribute(bool on) : DomAttribute("StrikeThrough"), IFormatOption
{
    /// <summary>
    /// Gets a value indicating whether text strikethrough should be turned on.
    /// </summary>
    public bool Value { get; } = on;

    /// <inheritdoc/>
    public FormatType Type { get; } = FormatType.Decorator;

    /// <inheritdoc/>
    public bool Apply(ref Node node)
    {
        DomAttribute? attr = node.GetAttribute(Name);
        if (attr != null && attr is StrikeThroughAttribute other)
        {
            if (other.Value == Value) return false;
        }

        node.Attributes[Name] = this;
        return true;
    }
}

/// <summary>
/// VerticalTypesettingAttribute defines the vertical typesetting of the node.
/// </summary>
/// <remarks>Initializes a new instance of the <see cref="VerticalTypesettingAttribute"/> class.</remark>
/// <param name="value">The <see cref="VerticalTypesetType"/> to use for the node.</param>
public class VerticalTypesettingAttribute(VerticalTypesetType value) : DomAttribute("VerticalTypesetting"), IFormatOption
{
    /// <summary>
    /// Gets the <see cref="VerticalTypesetType"/> associated with this attribute.
    /// </summary>
    public VerticalTypesetType Value { get; } = value;

    /// <inheritdoc/>
    public FormatType Type { get; } = FormatType.Decorator;

    /// <inheritdoc/>
    public bool Apply(ref Node node)
    {
        DomAttribute? attr = node.GetAttribute("VerticalTypesetting");
        if (attr != null && attr is VerticalTypesettingAttribute other)
        {
            if (other.Value == Value)
                return false;
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
    /// Gets the <see cref="FontReference"/> associated with this font ref id.
    /// </summary>
    public FontReference Value { get { return Document.FontTable.GetValueOrDefault(GlobalID, Document.FontTable["default"]); } }

    /// <inheritdoc/>
    public FormatType Type { get; } = FormatType.Decorator;

    /// <inheritdoc/>
    public bool Apply(ref Node node)
    {
        DomAttribute? attr = node.GetAttribute(Name);
        if (attr != null && attr is FontAttribute other)
        {
            if (other.GlobalID == GlobalID) return false;
        }

        node.Attributes[Name] = this;
        return true;
    }
}