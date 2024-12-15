public class GraphUI
{
    public void DrawGraph(GameCore.Node[] nodes, GameCore.Edge[] edges)
    {
        var nodeDrawer = new NodeUI();
        var edgeDrawer = new EdgeUI();
        foreach (var node in nodes)
        {
            nodeDrawer.DrawNode(node);
        }

        foreach (var edge in edges)
        {
            edgeDrawer.DrawEdge(edge);
        }
    }
}
