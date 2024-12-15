using System.Collections.Generic;
using System;
using UnityEngine;

public class GameCore : MonoBehaviour
{
    public static Color[] MyColors = {
        new Color(0.5f, 0.5f, 0.5f),  // Grey ~ FreeNode
        new Color(0.0f, 0.0f, 1.0f),  // Blue
        new Color(   1,    0,    0),  // Red
        new Color(   0,    1,    0),  // Green
        new Color(   0,    0,    0),  // Debug
    };
    public static int NumberOfColours = MyColors.Length;  // public fields are evil?
    public class Node
    {
        public enum NodeType
        {
            Default,
            Miner
        }
        
        private Vector2 _coordinate;
        // private Color _color; // TODO: change with texture
        private int _color;  // refers to MyColors
        private int _value;
        private int _grade;
        private NodeType _nodeType;

        public void SetCoordinate(Vector2 new_coordinate) => _coordinate = new_coordinate;
        
        public void SetColor(int new_color) => _color = new_color;

        public void SetValue(int new_value) => _value = new_value;

        public void ChangeValue(int delta) => _value += delta;

        public void SetGrade(int new_grade) => _grade = new_grade;

        public Vector2 GetCoordinate() => _coordinate;
        
        public int GetColor() => _color;
        
        public int GetValue() => _value;

        public int GetGrade() => _grade;
        
        public NodeType GetNodeType() => _nodeType;
        
        public NodeType GetDefaultType() => NodeType.Default;
        
        public NodeType GetMinerType() => NodeType.Miner;

        // tmp_cringe
        public void SetNodeState(Vector2 coord, int color, int val, int grade, NodeType nt)
        {
            _coordinate = coord;
            _color = color;
            _value = val;
            _grade = grade;
            _nodeType = nt;
        }
    }

    public struct Edge
    {
        public Node FirstNode;
        public Node SecondNode;
    }

    public struct Graph
    {
        public List<Node> nodes;
        public List<Edge> edges;
        public Graph(int lol = 0) {
            nodes = new ();
            edges = new ();
        }
    }

    public struct GameState
    {
        public Graph graph;
        public List<int> scores;
        public List<int> click_powers;
        public bool is_game_running;
        public void InitGameState(int players_cnt) {
            graph = new (0);
            scores = new ();
            click_powers = new ();
            for (int i = 0; i < players_cnt; ++i)
            {
                scores.Add(0); click_powers.Add(0);
            }
        }
    }

    // actual data
    private GameState state = new ();
    public int PlayerColor = 1;
    public ObjectsHolder ObjectHolder;
    // actual data
    public static float CellSize = 100f;

    private Node CheckCollision()
    {
        var mouseWorldPos = Input.mousePosition;

        foreach (var node in state.graph.nodes)
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

            LeftClickOnNode(clickedNode);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            // RMB
            
            var clickedNode = CheckCollision();
            if (clickedNode == null)
            {
                return;
            }

            RightClickOnNode(clickedNode);
        }
    }
    

















    // go away? ))))))
    private void LeftClickOnNode(Node node)
    {
        int power = state.click_powers[PlayerColor];
        if (PlayerColor == 0) {
            // player 0 does unique things
            if (node.GetValue() < power) node.SetColor(0);
            if (node.GetColor() == 0) { node.ChangeValue(power); }
            else { node.ChangeValue(-power); }
        } else if (node.GetColor() == PlayerColor) { node.ChangeValue(power); }
            else {
            int capture_constant = 100;
            bool capture_condition = node.GetColor() == 0;
            if (node.GetColor() != 0) {
                capture_condition = TryToPay(capture_constant * Math.Min(power, node.GetValue()));
            }
            if (capture_condition) {
                if (node.GetValue() < power) {
                    node.SetColor(PlayerColor);
                    node.ChangeValue(power - 2 * node.GetValue());
                } else { node.ChangeValue(-power); }
            }
        }
        UpdateNode(node);
        UpdateScore();
    }
    private void RightClickOnNode(Node node)
    {
        if (node.GetColor() != PlayerColor) { return; }
        if (TryToPay(2 << node.GetGrade())) node.SetGrade(node.GetGrade() + 1);
        UpdateNode(node);
    }
    public void Start()
    {
        state.InitGameState(NumberOfColours);
        state.is_game_running = true;

        // tmp_filling
        for (int i = 0; i < 4; ++i) state.graph.nodes.Add(new Node());
        state.graph.nodes[0].SetNodeState(new Vector2(200, 800), 0, 0, 0, Node.NodeType.Miner);
        state.graph.nodes[1].SetNodeState(new Vector2(400, 800), 0, 0, 0, Node.NodeType.Miner);
        state.graph.nodes[2].SetNodeState(new Vector2(600, 800), 0, 0, 0, Node.NodeType.Miner);
        state.graph.nodes[3].SetNodeState(new Vector2(800, 800), 0, 0, 0, Node.NodeType.Miner);

       ObjectHolder.GraphUIObject.DrawGraph(state.graph);
    }
    void CountGold()
    {
        foreach(Node node in state.graph.nodes)
        state.scores[node.GetColor()] += node.GetColor() * (node.GetGrade() + 1);

        UpdateScore();
    }
    private float GoldTimer = 0;
    public void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Tab)) { Application.Quit(); }
        if (!state.is_game_running) { return; }
        GoldTimer += Time.deltaTime;
        if (GoldTimer >= 1) {
            GoldTimer -= 1;
            CountGold();
        }
    }
    bool TryToPay(int cost)
    {
        if (state.scores[PlayerColor] >= cost)
        {
            ChangeCurrentScore(-cost);
            return true;
        }
        return false;
    }
    void ChangeCurrentScore(int delta)
    {
        state.scores[PlayerColor] += delta;
        UpdateScore();
    }



    // updating screen section
    void UpdateScore() {}
    void UpdateNode(Node node) {}
    
}