namespace RtfDom;

/// <summary>
/// FormatOptions.
/// </summary>
public abstract class FormatOption
{
    public abstract bool Apply(Node other);
}

public class FormatList : List<FormatOption>
{
    public bool Apply(Node n)
    {
        bool appliedAll = true;

        foreach (FormatOption fmt in this)
        {
            fmt.Apply(n);
        }

        return appliedAll;
    }
}