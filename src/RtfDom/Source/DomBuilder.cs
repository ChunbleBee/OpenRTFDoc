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
        /*
@"
{
    \rtf1
    \pard
    This is a paragraph of text.\par
    # Should provide a formatting 
    {\pntext\f0 1.\tab}{\*\pn\pnlvlblt\pnf0\pnindent0{\pntxtb .}}This is a paragraph with a numbered list item.\par
        {\pntext\f0 1.\tab}{\pntext\f0 1.\tab}{\*\pn\pnlvlbody\pnf0\pnindent0\pnstart1{\pntxta .}}This is a second level list item.\par
        {\pntext\f0 1.\tab}This is another second level list item.\par
            {\pntext\f0 1.\tab}{\*\pn\pnlvlblt\pnf0\pnindent0{\pntxtb .}}This is another paragraph with a numbered list item.\par
}"
        */
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
        return new FormatList();
        throw new NotImplementedException();
    }

    private static FormatList BuildColorTable(Group group)
    {
        return new FormatList();
        throw new NotImplementedException();
    }

    private static FormatList BuildFontTable(Group group)
    {
        return new FormatList();
        throw new NotImplementedException();
    }
}