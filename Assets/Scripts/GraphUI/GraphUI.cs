using UnityEngine;

public class GraphUI : MonoBehaviour
{
    [SerializeField] public NodeUI node_drawer;
    public void DrawGraph(GameCore.Graph graph)
    {
        foreach (var node in graph.nodes) node_drawer.DrawNode(node);
        foreach (var edge in graph.edges) EdgeUI.DrawEdge(edge);
    }
}
