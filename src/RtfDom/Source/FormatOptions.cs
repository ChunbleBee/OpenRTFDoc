namespace RtfDom;

using RtfModels;

/// <summary>
/// IFormatOption is an interface for any <see cref="DomAttribute"/> that can automatically apply their
/// </summary>
public interface IFormatOption
{
    /// <summary>
    /// Gets the formatting type that the node will apply.
    /// </summary>
    public FormatType Type { get; }

    /// <summary>
    /// Applies the given attribute to the given node.
    /// </summary>
    /// <param name="node">The <see cref="Node"/> to apply the formatting to.</param>
    /// <returns>True if the formatting was applied, false otherwise.</returns>
    public abstract bool Apply(ref Node node);

    /// <summary>
    /// FromFormatWord 
    /// </summary>
    /// <param name="fmt">The <see cref="ControlWord"/> to build the formatting from.</param>
    /// <returns>A new <see cref="DomAttribute"/> built from the formatting word.</returns>
    public static IFormatOption FromFormatWord(IFormat fmt)
    {
        switch (fmt)
        {
            case VerticalTypesetWord word:
                {
                    if (word.State == VerticalTypesetType.None)
                    {
                        return new VerticalTypesettingAttribute(VerticalTypesetType.None);
                    }
                    else if (word.State == VerticalTypesetType.None)
                    {
                        return new VerticalTypesettingAttribute(VerticalTypesetType.None);
                    }
                    else if (word.State == VerticalTypesetType.None)
                    {
                        return new VerticalTypesettingAttribute(VerticalTypesetType.None);
                    }
                    else
                    {
                        throw new ArgumentException($"unknown vertical typesetting value: {word.State}");
                    }
                }
            case BoldWord word: { return new BoldAttribute(word.Param == null || word.Param != "0"); }
            case ItalicsWord word: { return new ItalicsAttribute(word.Param == null || word.Param != "0"); }
            case UnderlinedWord word: { return new UnderlinedAttribute(word.Param == null || word.Param != "0"); }
            case StrikeThroughWord word: { return new StrikeThroughAttribute(word.Param == null || word.Param != "0"); }
            default: throw new NotImplementedException($"unknown formatting word: {fmt.GetType()}");
        }
    }
}

/// <summary>
/// FormatList is a list of formatting options.
/// </summary>
public class FormatList : List<IFormatOption>
{
    /// <summary>
    /// Apply applies all <see cref="IFormatOption"/> to the given node.
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
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