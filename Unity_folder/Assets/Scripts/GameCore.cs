using UnityEngine;
using System.Numerics;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;

public class GameCore : MonoBehaviour
{
    // buttons
    public GameObject NodeObjectSpawner;
    public GameObject ButtonToChangeColor;
    public GameObject ScoreTable;
    public GameObject IncreasePower;
    // buttons
    public static Color[] MyColors = {
        new Color(0.5f, 0.5f, 0.5f),  // Grey
        new Color(0.0f, 0.0f, 1.0f),  // Blue
        new Color(   1,    0,    0),  // Red
        new Color(   0,    1,    0),  // Green
        new Color(   0,    0,    0),  // Debug
    };
    public static int NumberOfColours = MyColors.Length;
    public int CurrentPlayerColor = 1;
    public static void ChangeIntencity(ref Color color, float mult) {
        color.r *= mult;
        color.g *= mult;
        color.b *= mult;
    }
    public static void ChangeColorOfAButton(ref Button obj, int num) {
        // here's a mess
        Color tmp = MyColors[num];
        Color new_color1 = new Color(tmp.r, tmp.g, tmp.b, tmp.a);
        Color new_color2 = new Color(tmp.r, tmp.g, tmp.b, tmp.a);
        ChangeIntencity(ref new_color2, 0.8f);
        Color new_color3 = new Color(tmp.r, tmp.g, tmp.b, tmp.a);
        ChangeIntencity(ref new_color3, 0.8f);

        var changed_color = obj.colors;

        changed_color.normalColor = new_color1;
        changed_color.selectedColor = new_color1;
        
        changed_color.highlightedColor = new_color1 * 1.0f;  // it's changing alpha I guess

        changed_color.pressedColor = new_color1 * 0.8f;
        changed_color.disabledColor = new_color1 * 0.8f;

        obj.colors = changed_color;
    }

    public struct NodeObjectRefs {
        public GameObject node;
        public Canvas canvas;
        public Button button;
        public TextMeshProUGUI text;
        public void GenRefs(GameObject node_i) {
            node = node_i;
            canvas = node.GetComponentInChildren<Canvas>();
            button = canvas.GetComponentInChildren<Button>();
            text = button.GetComponentInChildren<TextMeshProUGUI>();
        }
        public void ForgetRefs() {
            if (node != null) {
                Destroy(node);
                node = null;
                canvas = null;
                button = null;
                text = null;
            }
        }
    }
    public struct Node {
        public int number;
        public int x;
        public int y;
        public int color;
        public int score;
        public List<int> edges;
        public NodeObjectRefs refs;
        // public GameObject ref_to_node;
        public Node(int n_i, int x_i, int y_i, int c_i, int s_i, List<int> e_i) {
            number = n_i;
            x = x_i;
            y = y_i;
            color = c_i;
            score = s_i;
            edges = e_i;
            refs = new NodeObjectRefs();
        }

        
        public void ChangeColor(int num) {
            color = num;
            ChangeColorOfAButton(ref refs.button, num);
        }
        public void ChangeScore(int delta) {
            score += delta;
            refs.text.text = score.ToString();
        }
    }
    public struct GameState {
        int Field_x_size;
        int Field_y_size;
        public List<Node> nodes;
        public GameState(int aboba) {
            Field_x_size = 1920;
            Field_y_size = 1080;
            nodes = new List<Node>();
        }
        public string StateToString() {
        string ans = "GameState:\n";
        ans += "Size: " + Field_x_size + "x" + Field_y_size + "\n";
        // for network translation? ...
        return ans;
    }
    }
    public class GameGenerator {
        public static GameState GenSimpleGamestate() {
            GameState ans = new GameState(0);
            //                     n     x     y  c  s
            ans.nodes.Add(new Node(0, 0100, 0100, 0, 2, new List<int>()));
            ans.nodes.Add(new Node(1, 0500, -100, 1, 1, new List<int>()));
            ans.nodes.Add(new Node(2, -200, 0000, 0, 5, new List<int>()));
            ans.nodes.Add(new Node(3, -200, -470, 0, 8, new List<int>()));
            ans.nodes.Add(new Node(4, -610, 0230, 0, 0, new List<int>()));
            ans.nodes.Add(new Node(5, -740, -280, 0, 3, new List<int>()));
            ans.nodes.Add(new Node(6, 0050, -210, 3, 4, new List<int>()));
            ans.nodes.Add(new Node(7, 0650, 0280, 2, 9, new List<int>()));
            return ans;
        }
        public static GameState GenRandomGamestate() {
            GameState ans = new GameState(0);
            // ??
            return ans;
        }
        public static GameState GenDefaultGamestate() {
            return GenSimpleGamestate();
            // return GenRandomGameState();
        }
    }

    //  block with actual game info
    public GameState state = GameGenerator.GenDefaultGamestate();
    public List<int> scores = new List<int>();
    public List<int> click_powers = new List<int>();
    public bool is_game_running = true;
    //

    public void SetAnotherGameState(GameState another) {
        RemoveDesk();
        state = another;
        DrawDesk();  // ?
    }
    public void DrawNode(ref Node node) {
        GameObject newobj = Instantiate(NodeObjectSpawner, transform.position, transform.rotation);
        newobj.name = node.number.ToString();
        node.refs.GenRefs(newobj);
        
        node.ChangeColor(node.color);
        node.ChangeScore(0);


        UnityEngine.Vector3 vec = new UnityEngine.Vector3(node.x, node.y, 0);
        node.refs.button.GetComponent<RectTransform>().anchoredPosition = vec;
    }
    public void RemoveDesk() {
        for (int i = 0; i < state.nodes.Count; ++i) {
            Node node = state.nodes[i];
            node.refs.ForgetRefs();
            state.nodes[i] = node;
        }

        for (int i = 0; i < NumberOfColours; ++i) { scores[i] = 0; }
        for (int i = 0; i < NumberOfColours; ++i) { click_powers[i] = 1; }
    }
    public void DrawDesk() {
        RemoveDesk();
        for (int i = 0; i < state.nodes.Count; ++i) {
            Node node = state.nodes[i];
            DrawNode(ref node);
            state.nodes[i] = node;
        }
    }
    public void PressNode(GameObject NodeObject) {
        int num = int.Parse(NodeObject.name);
        Node node = state.nodes[num];
        int power = click_powers[CurrentPlayerColor];

        if (CurrentPlayerColor == 0) {
            if (node.score < power) {
                node.ChangeColor(0);
            }
            if (node.color == 0) { node.ChangeScore(power); }
            else { node.ChangeScore(-power); }
        } else if (node.score < power) {
            node.ChangeColor(CurrentPlayerColor);
            node.ChangeScore(power);
        } else if (node.color == 0) {
            node.ChangeScore(-power);
        } else if (node.color == CurrentPlayerColor) {
            node.ChangeScore(power);
        }

        state.nodes[num] = node;
    }
    public void ChangePlayerColor() {
        CurrentPlayerColor = (CurrentPlayerColor + 1) % MyColors.Length;
        var button = ButtonToChangeColor.GetComponentInChildren<Button>();
        ChangeColorOfAButton(ref button, CurrentPlayerColor);
    }
    public void IncreaseClickPower() {
        int cost = 100;  // a constant??
        if (scores[CurrentPlayerColor] >= cost) {
            scores[CurrentPlayerColor] -= cost;
            click_powers[CurrentPlayerColor] += 1;
        }
        IncreasePower.GetComponentInChildren<TextMeshProUGUI>().text =
        "Click power: " + click_powers[CurrentPlayerColor].ToString();
    }

    [ContextMenu("DoAFunc")]
    public void DoAFunc() {

    }
    public void Start() {
        for (int i = 0; i < NumberOfColours; ++i) {
            scores.Add(0);
            click_powers.Add(1);
        }
    }

    public float GoldTimer = 0;
    void CountGold() {
        foreach(Node node in state.nodes) {
            scores[node.color] += node.score;
        }
        ScoreTable.GetComponentInChildren<TextMeshProUGUI>().text =
        "Score: " + scores[CurrentPlayerColor].ToString();
    }
    public void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) { Application.Quit(); }
        if (!is_game_running) { return; }
        GoldTimer += Time.deltaTime;
        if (GoldTimer >= 1) {
            GoldTimer = 0;
            CountGold();
        }
    }
}







