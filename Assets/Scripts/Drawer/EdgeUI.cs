using UnityEngine;

public class EdgeUI
{
    // Function of drawing edges
    public static void DrawEdge(GameCore.Edge edge)
    {
        // Creating edge
        var newEdge = new GameObject("edge");
        var renderer = newEdge.AddComponent<SpriteRenderer>();
        var texture = Texture2D.whiteTexture;
        var length = Vector2.Distance(edge.FirstNode.GetCoordinate(), edge.SecondNode.GetCoordinate());
        renderer.sprite = Sprite.Create(texture, new Rect(edge.FirstNode.GetCoordinate().x, edge.FirstNode.GetCoordinate().y, length, 10f),
            new Vector2(0.5f, 0.5f));
        
        // Counting angle
        var angleBetween = Mathf.Atan2(edge.SecondNode.GetCoordinate().y - edge.FirstNode.GetCoordinate().y,
            edge.SecondNode.GetCoordinate().x - edge.FirstNode.GetCoordinate().x) * Mathf.Rad2Deg;
        newEdge.transform.rotation = Quaternion.Euler(0, 0, angleBetween);
    }
}