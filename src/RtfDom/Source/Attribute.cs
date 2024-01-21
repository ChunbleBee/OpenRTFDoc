namespace RtfDom;

using System.Drawing;
using RtfModels;

/// <summary>
/// DomAttribute describes some attribute that a node may have.
/// </summary>
public abstract class DomAttribute<T>(string name) : IEquatable<T>
{
    public string Name { get; } = name;

    public abstract bool Equals(object? obj);
    public abstract bool Equals(T? other);
}

/// <summary>
/// RefDomAttribute describes some attribute that a node may have that references document global attributes.
/// </summary>
/// <typeparam name="T1">The <see cref="IEquatable"/> type.</typeparam>
/// <typeparam name="T2">The global reference type.</typeparam>
/// <param name="name">The name of the </param>
/// <param name="doc">The <see cref="DocumentNode"/> to reference from.</param>
/// <param name="globalID">The reference value to use to find the global attribute.</param>
public abstract class RefDomAttribute<T1, T2>(string name, DocumentNode doc, T2 globalID) : DomAttribute<T1>(name)
{
    public T2 GlobalID { get; } = globalID;
    public DocumentNode Document { get; } = doc;
    public abstract T1 GetAttribute();
}

public class PlainAttribute(DocumentNode doc) : RefDomAttribute<PlainAttribute, None>("Plain", doc)
{
    public FormatType FormatType { get; } = FormatType.AllFormatting;
    public override bool Equals(object? obj)
    {
        throw new NotImplementedException();
    }

    public override bool Equals(PlainAttribute? other)
    {
        throw new NotImplementedException();
    }

    public override PlainAttribute GetAttribute()
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// ColorAttribute describes a <see cref="Color"/> to apply to the <see cref="Node"/>.
/// </summary>
/// <param name="globalID">The global color table <see cref="Guid"/>.</param>
public class ColorAttribute(string gid, DocumentNode doc) : DomAttribute<ColorAttribute, string>("Color", gid, doc), IFormatOption
{
    public Color Value
    {
        get()
        {
            return Document.ColorTable.GetValueOrDefault(
                GlobalID,
                Document.ColorTable["0"]);
        }
    }

    public FormatType FormatType { get; } = FormatType.Color;

public bool Apply(ref Node node)
{
    throw nameof NotImplementedException();
}

public override bool Equals(object? obj)
{
    throw new NotImplementedException();
}

public override bool Equals(ColorAttribute? other)
{
    throw new NotImplementedException();
}
}


public class BoldAttribute(bool on) : DomAttribute<>("Bold", ), IFormatOption
{
    public FormatType FormatType { get; } = FormatType.Decorator;

    public bool Apply(ref Node node)
    {
        node.Attributes[Name] = this;
    }
}