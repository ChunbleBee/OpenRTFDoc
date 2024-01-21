namespace RtfDom;

using RtfModels;

public static class DomBuilder
{
    public static DocumentNode Build(Group doc)
    {
        DocumentNode dNode = new();
        Node root = dNode;
        Build(doc, ref root, ref dNode);
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
            Node? n = null;

            if (token is IString str)
            {
                prev.InnerText = str.Convert(((FontAttribute)prev.GetAttribute("Font")).Value.Encoding);
            }
            else if (token is IFormat fmtr)
            {
                n = new(doc, current);
                options.Add(IFormatOption.FromFormatWord(fmtr));
                options.Apply(ref n);
                options.Clear();
            }
            else if (token is Group grp)
            {
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


            prev = n ?? prev;
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