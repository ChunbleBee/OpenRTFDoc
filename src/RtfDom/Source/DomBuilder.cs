namespace RtfDom;

using RtfModels;

public static class DomBuilder
{
    public static DocumentNode Build(Group doc)
    {
        DocumentNode dNode = new();
        Node root = dNode;
        List<FormatOption> opts = [];
        Build(doc, ref root, ref dNode, ref opts);
        return dNode;
    }

    private static Node? Build(
        Group group,
        ref Node current,
        ref DocumentNode doc,
        ref List<FormatOption> opts)
    {
        Node? n = null;

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

        }

        return null;
    }
}