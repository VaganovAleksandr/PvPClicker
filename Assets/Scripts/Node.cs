using UnityEngine;

public class Node : ScriptableObject
{
    public enum Type
    {
        Default,
        Miner
    }

    [SerializeField] public GameObject nodeGObj;
    [SerializeField] public Vector2 position;
    [SerializeField] public Type nodeType;

    public void Draw()
    {
        nodeGObj.transform.position = position;
        nodeGObj.transform.localScale = Vector3.one;
        nodeGObj.GetComponent<SpriteRenderer>().color = (nodeType == Type.Default) ? Color.white : Color.yellow;

        // Will be uncommented when the textures are ready
        // Texture2D texture = Resources.Load<Texture2D>("DefaultNode");
        // Renderer renderer = NodeGObj.GetComponent<Renderer>();
        // renderer.material.mainTexture = texture;
    }
}