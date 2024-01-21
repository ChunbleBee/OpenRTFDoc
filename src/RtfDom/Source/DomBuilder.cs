namespace RtfDom;

using System.Collections.Generic;
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
        foreach (IToken token in group.Children)
        {
            if (token is IString str)
            {
                Node n = new(current)
                {
                    InnerText = str.Convert()
                };
            }
            else if (token is Group grp)
            {
                if (grp.Type != GroupType.Default)
                {
                    FormatOption fmt = ParseDestinationGroup(grp);
                    if (grp.Type == GroupType.Global)
                    {
                        fmt.Apply(doc);
                    }
                    else
                    {
                        options.Add(fmt);
                    }
                }
                else
                {
                    Node n = new()
                    {
                        Attributes = current.Attributes,
                        Parent = current,
                    };

                }
            }
        }
    }

    private static FormatOption ParseDestinationGroup(Group group)
    {
        throw new NotImplementedException();
    }
}