namespace RtfDom;

public static class AttributeConstants
{
    public static readonly string ParagraphNumbering = "ParagraphNumbering";
    public static readonly string Plain = "Plain";
    public static readonly string Color = "Color";
    public static readonly string Bold = "Bold";
    public static readonly string Italics = "Italics";
    public static readonly string Underline = "Underline";
    public static readonly string StrikeThrough = "StrikeThrough";
    public static readonly string VerticalTypesetting = "VerticalTypesetting";
    public static readonly string Font = "Font";
}

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
/// RefDomAttribute describes some attribute that references document global attributes.
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