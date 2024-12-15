using UnityEngine;

public class NodeUI
{
    public void DrawNode(GameCore.Node node)
    {
        var newNode = new GameObject();
        newNode.transform.position = new Vector2(node.GetCoordinate().x, node.GetCoordinate().y);
    }
}
