namespace RtfDom;

using RtfModels;

/// <summary>
/// IFormatOption is an interface for any <see cref="DomAttribute"/> that can automatically apply their
/// </summary>
public interface IFormatOption
{
    public FormatType Type { get; }
    public abstract bool Apply(ref Node node);

    public static IFormatOption FromFormatWord(IFormat fmt)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// FormatList is a list of formatting options.
/// </summary>
public class FormatList : List<IFormatOption>
{
    public bool Apply(ref Node n)
    {
        bool appliedAll = true;

        foreach (IFormatOption fmt in this)
        {
            appliedAll &= fmt.Apply(ref n);
        }

        return appliedAll;
    }
}