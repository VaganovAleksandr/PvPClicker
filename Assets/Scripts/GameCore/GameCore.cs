using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

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

        public List<int> edges;

        public GameObject unity_object;

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
            edges = new ();
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
        public List<Edge> edges;  // ?
        public Graph(int lol = 0) {
            nodes = new ();
            edges = new ();
        }
        public void AddEdge(int a, int b) {
            Node node_a = nodes[a];
            Node node_b = nodes[b];

            // var new_edge = new Edge(); edges.Add();
            node_a.edges.Add(b);
            node_b.edges.Add(a);

            nodes[a] = node_a;
            nodes[b] = node_b;
        }
    }

    public struct GameState
    {
        public Graph graph;
        public List<int> scores;
        public List<int> click_powers;
        public List<bool> running_state;
        public void InitGameState(int players_cnt) {
            graph = new (0);
            scores = new ();
            click_powers = new ();
            running_state = new();
            for (int i = 0; i < players_cnt; ++i)
            {
                scores.Add(0);
                click_powers.Add(1);
                running_state.Add(true);
            }
        }
    }

    // actual data
    public GameState state = new ();
    public int PlayerColor = 0;
    public ObjectsHolder Holder;
    // actual data












    // go away? ))))))
    private bool CheckAvalibility(int node_num) {
        Node node = state.graph.nodes[node_num];
        foreach (var i in node.edges) {
            if (state.graph.nodes[i].GetColor() == PlayerColor) {
                return true;
            }
        }
        Debug.Log("This node is not near");
        return false;
    }
    public void LeftClickOnNode(int node_num)
    {
        Node node = state.graph.nodes[node_num];

        int power = state.click_powers[PlayerColor];
        if (PlayerColor == 0) {
            // player 0 does unique things
            if (node.GetColor() == 0) { node.ChangeValue(power); }
            else {
                if (node.GetValue() < power) node.SetColor(0);
                node.ChangeValue(power - 2 * node.GetValue());
            }
        } else if (node.GetColor() == PlayerColor) { node.ChangeValue(power); }
            else if (CheckAvalibility(node_num)) {
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
        Holder.GameDrawerObject.UpdateNode(node);
        Holder.GameDrawerObject.UpdateScore();

        state.graph.nodes[node_num] = node;
    }
    public void RightClickOnNode(int node_num)
    {
        Node node = state.graph.nodes[node_num];

        if (node.GetColor() != PlayerColor) { return; }
        if (TryToPay(2 << node.GetGrade())) node.SetGrade(node.GetGrade() + 1);
        Holder.GameDrawerObject.UpdateNode(node);

        state.graph.nodes[node_num] = node;
    }
    public void Start()
    {
        // Holder.OnClick += OnPointerDown;
        state.InitGameState(NumberOfColours);

        // tmp_filling
        for (int i = 0; i < 10; ++i) state.graph.nodes.Add(new Node());
        state.graph.nodes[0].SetNodeState(new Vector2(200, 750), 0, 0, 0, Node.NodeType.Miner);
        state.graph.nodes[1].SetNodeState(new Vector2(511, 742), 0, 1, 0, Node.NodeType.Miner);
        state.graph.nodes[2].SetNodeState(new Vector2(1720, 120), 0, 2, 0, Node.NodeType.Miner);
        state.graph.nodes[3].SetNodeState(new Vector2(1430, 910), 0, 3, 0, Node.NodeType.Miner);
        state.graph.nodes[4].SetNodeState(new Vector2(1042, 671), 0, 4, 0, Node.NodeType.Miner);

        state.graph.nodes[5].SetNodeState(new Vector2(700, 940), 1, 5, 0, Node.NodeType.Miner);
        state.graph.nodes[6].SetNodeState(new Vector2(980, 200), 2, 6, 0, Node.NodeType.Miner);
        state.graph.nodes[7].SetNodeState(new Vector2(1700, 510), 3, 7, 0, Node.NodeType.Miner);

        state.graph.nodes[8].SetNodeState(new Vector2(432, 110), 0, 8, 0, Node.NodeType.Miner);
        state.graph.nodes[9].SetNodeState(new Vector2(524, 571), 0, 9, 0, Node.NodeType.Miner);



        state.graph.AddEdge(1, 5);
        state.graph.AddEdge(0, 1);
        state.graph.AddEdge(1, 9);
        state.graph.AddEdge(8, 9);
        state.graph.AddEdge(6, 9);
        state.graph.AddEdge(2, 4);
        state.graph.AddEdge(4, 6);
        state.graph.AddEdge(3, 4);
        state.graph.AddEdge(2, 7);
        // tmp_filling

       Holder.GameDrawerObject.DrawGame();
       ChangePlayerColor();
    }
    void CheckGameEnd() {
        foreach (var node in state.graph.nodes) {
            if (node.GetColor() == PlayerColor) {
                state.running_state[PlayerColor] = true;
                Holder.GameDrawerObject.UpdateEndGame();  // ?
                return;
            }
        }
        state.running_state[PlayerColor] = false;

        Holder.GameDrawerObject.UpdateEndGame();
    }
    void CountGold()
    {
        foreach(Node node in state.graph.nodes)
        state.scores[node.GetColor()] += node.GetValue() * (node.GetGrade() + 1);

        Holder.GameDrawerObject.UpdateScore();
    }
    private float GoldTimer = 0;
    private float CheckEndGameTimer = 0;
    public void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Tab)) { Application.Quit(); }
        // if (!state.is_game_running) { return; }
        GoldTimer += Time.deltaTime;
        CheckEndGameTimer += Time.deltaTime;
        if (GoldTimer >= 1) {
            GoldTimer -= 1;
            CountGold();
        }
        if (CheckEndGameTimer >= 1) {
            CheckEndGameTimer -= 1;
            CheckGameEnd();
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
        Holder.GameDrawerObject.UpdateScore();
    }
    public void ChangePlayerColor() {
        PlayerColor += 1; PlayerColor %= NumberOfColours;
        CheckGameEnd();
        Holder.GameDrawerObject.UpdateAllStats();
    }
    public void IncreasePowerClick() {
        if (TryToPay(1 << state.click_powers[PlayerColor])) {
            ++state.click_powers[PlayerColor];
            Holder.GameDrawerObject.UpdatePowerClick();
        }
    }

}