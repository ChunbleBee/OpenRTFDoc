namespace RtfDom;

public interface IType { }

/// <summary>
/// Attribute defines some attribute that a node may have.
/// </summary>
public abstract class DomAttribute(string name, IType value)
{
    public IType Value { get; set; } = value;
    public string Name { get; } = name;
}