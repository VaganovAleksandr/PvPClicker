using UnityEngine;

public class NodeUI : MonoBehaviour
{
    [SerializeField] public GameObject defaultPrefab;
    [SerializeField] public GameObject minerPrefab;

    public void DrawNode(GameCore.Node node)
    {
        GameObject newNode = (node.GetNodeType() == node.GetDefaultType())
            ? Instantiate(defaultPrefab)
            : Instantiate(minerPrefab);
        newNode.transform.position = new Vector2(node.GetCoordinate().x, node.GetCoordinate().y);
    }
}