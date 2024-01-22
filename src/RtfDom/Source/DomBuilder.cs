namespace RtfDom;

using System.ComponentModel;
using RtfModels;

public static class DomBuilder
{
    public static DocumentNode Build(Group docGroup)
    {
        DocumentNode dNode = new();
        Node root = dNode;
        Build(docGroup, ref root, ref dNode);
        return dNode;
    }

    private static void Build(
        Group group,
        ref Node current,
        ref DocumentNode doc)
    {
        FormatList options = [];
        Node prev = current;

        // For each token:
        //  if it's an IString, convert it and put it into the 
        foreach (IToken token in group)
        {
            Console.WriteLine($"Current token: {token}\n\tFormatting Options: {options.Count}");
            Node? n = null;

            if (token is IString str)
            {
                n = new(doc, current)
                {
                    InnerText = str.Convert(((FontAttribute)prev.GetAttribute("Font")).Value.Encoding)
                };

                Console.WriteLine($"IString Token, Value: {prev.InnerText}");
            }
            else if (token is IFormat fmtr)
            {
                Console.WriteLine("IFormat Token");
                options.Add(IFormatOption.FromFormatWord(fmtr));
            }
            else if (token is Group grp)
            {
                Console.WriteLine($"Group Token: {grp.GetType()}");
                if (grp.Type != GroupType.Default)
                {
                    FormatList fmtlst = ParseDestinationGroup(grp);
                    if (grp.Type == GroupType.Global)
                    {
                        Node docNode = doc;
                        fmtlst.Apply(ref docNode);
                    }
                    else
                    {
                        options.AddRange(fmtlst);
                    }
                }
                else
                {
                    n = new(doc, current);
                    options.Apply(ref n);
                    options.Clear();
                    Build(grp, ref n, ref doc);
                }
            }

            if (n != null)
            {
                current.Children.Add(n);
                prev = n;
            }
            Console.WriteLine($"Prev Node: {prev}");
        }
    }

    private static FormatList ParseDestinationGroup(Group group)
    {
        return [];
        throw new NotImplementedException();
    }

    private static FormatList BuildColorTable(Group group)
    {
        return [];
        throw new NotImplementedException();
    }

    private static FormatList BuildFontTable(Group group)
    {
        return [];
        throw new NotImplementedException();
    }
}