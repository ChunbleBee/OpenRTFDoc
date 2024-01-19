namespace RtfDom;

public class DomBuilder
{
    public static DocumentNode Build(Group doc)
    {
        DocumentNode dNode = new();
        Build(doc, ref dNode, ref dNode);
        return dNode;
    }

    private static void Build(Group group, ref Node parent, ref DocumentNode doc)
    {
        foreach(ControlWord word in group.Children)
        {
            
        }
    }
}