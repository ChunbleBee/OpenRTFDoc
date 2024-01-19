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

    private static void Build(Group group, ref Node parent, ref DocumentNode doc)
    {
        foreach (IToken word in group.Children)
        {
            if (word is Group gWord)
            {

            }
        }
    }
}