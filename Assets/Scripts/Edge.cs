using UnityEngine;

public class Edge
{
    public Node FirstNode;
    public Node SecondNode;

    public void Draw()
    {
        double length = Vector2.Distance(FirstNode.position, SecondNode.position);
        
        var edge = new GameObject("Edge");
        var renderer = edge.AddComponent<SpriteRenderer>();
        var texture = Texture2D.whiteTexture; // TODO: edge texture
        renderer.sprite = Sprite.Create(texture, new Rect(FirstNode.position.x, FirstNode.position.y, (float )length, 20f), new Vector2(0.5f, 0.5f));

        var angleBetween = Mathf.Atan2(SecondNode.position.y - FirstNode.position.y, SecondNode.position.x - FirstNode.position.x) * Mathf.Rad2Deg;

        edge.transform.rotation = Quaternion.Euler(0, 0, angleBetween);
    }
}
