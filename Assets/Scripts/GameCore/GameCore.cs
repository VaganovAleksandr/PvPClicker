using UnityEngine;

public class GameCore : MonoBehaviour
{
    public class Node
    {
        public enum NodeType
        {
            Default,
            Miner
        }
        
        private Vector2 _coordinate;
        private Color _color; // TODO: change with texture
        private int _value;
        private NodeType _nodeType;

        public void SetCoordinate(Vector2 new_coordinate) => _coordinate = new_coordinate;
        
        public void SetColor(Color new_color) => _color = new_color;

        public void SetValue(int new_value) => _value = new_value;

        public Vector2 GetCoordinate() => _coordinate;
        
        public Color GetColor() => _color;
        
        public int GetValue() => _value;
        
        public NodeType GetNodeType() => _nodeType;
        
        public NodeType GetDefaultType() => NodeType.Default;
        
        public NodeType GetMinerType() => NodeType.Miner;

        public void Update()
        {
            // TODO
        }

        public void PowerUpdate()
        {
            // TODO
        }
    }

    public struct Edge
    {
        public Node FirstNode;
        public Node SecondNode;
    }
    
    private Node[] _nodes;
    private Edge[] _edges;
    private const float CellSize = 20f;

    private Node CheckCollision()
    {
        var mouseWorldPos = Input.mousePosition;

        foreach (var node in _nodes)
        {
            if (Vector2.Distance(mouseWorldPos, node.GetCoordinate()) < CellSize)
            {
                return node;
            }
        }
        
        return null;
    }
    
    void OnGUI()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // LMB
            
            var clickedNode = CheckCollision();
            if (clickedNode == null)
            {
                return;
            }

            clickedNode.Update();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            // RMB
            
            var clickedNode = CheckCollision();
            if (clickedNode == null)
            {
                return;
            }

            clickedNode.PowerUpdate();
        }
    }
    
    public void Start()
    {
        //TODO
    }

    public void Update()
    {
        //TODO
    }
}