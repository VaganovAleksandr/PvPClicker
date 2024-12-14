using UnityEngine;

public class Graph
{
    public Node[] Nodes;
    public Edge[] Edges;

    public void Draw()
    {
        foreach (var node in Nodes)
        {
            node.Draw();
        }

        foreach (var edge in Edges)
        {
            edge.Draw();
        }
    }
}
