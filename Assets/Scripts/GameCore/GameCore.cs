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
    public static GameObject NodeObjectSpawner;
    public static GameObject ButtonToChangeColor;
    public static GameObject ScoreTable;
    public static GameObject ButtonIncreasePower;
    public static GameObject ButtonChangeClickMode;
    public GameObject NodeObjectSpawner_tmp;
    public GameObject ButtonToChangeColor_tmp;
    public GameObject ScoreTable_tmp;
    public GameObject ButtonIncreasePower_tmp;
    public GameObject ButtonChangeClickMode_tmp;
    // buttons
    public static Color[] MyColors = {
        new Color(0.5f, 0.5f, 0.5f),  // Grey
        new Color(0.0f, 0.0f, 1.0f),  // Blue
        new Color(   1,    0,    0),  // Red
        new Color(   0,    1,    0),  // Green
        new Color(   0,    0,    0),  // Debug
    };
    public static int NumberOfColours = MyColors.Length;
    public int CurrentPlayerColor = 0;
    public static string[] ClickModes = {"UsualClick", "PowerUpNode"};
    public static int NumberOfClickModes = ClickModes.Length;
    public int CurrentClickMode = 0;
    public class MyColorClass {
        Color c;
        public static void ChangeIntencity(ref Color color, float mult) {
            color.r *= mult;
            color.g *= mult;
            color.b *= mult;
        }
        public void ChangeIntencity(float mult) {
            c.r *= mult;
            c.g *= mult;
            c.b *= mult;
        }
    }
    public static void ChangeColorOfAButton(ref Button obj, int num) {
        // here's a mess
        Color tmp = MyColors[num];
        Color new_color1 = new Color(tmp.r, tmp.g, tmp.b, tmp.a);
        Color new_color2 = new Color(tmp.r, tmp.g, tmp.b, tmp.a);
        MyColorClass.ChangeIntencity(ref new_color2, 0.8f);
        Color new_color3 = new Color(tmp.r, tmp.g, tmp.b, tmp.a);
        MyColorClass.ChangeIntencity(ref new_color3, 0.8f);

        var changed_color = obj.colors;

        changed_color.normalColor = new_color1;
        changed_color.selectedColor = new_color1;
        
        changed_color.highlightedColor = new_color1 * 1.0f;  // it's changing alpha I guess

        changed_color.pressedColor = new_color1 * 0.8f;
        changed_color.disabledColor = new_color1 * 0.8f;

        obj.colors = changed_color;
    }
    public void ChangePlayerColor() {
        CurrentPlayerColor = (CurrentPlayerColor + 1) % MyColors.Length;
        var button = ButtonToChangeColor.GetComponentInChildren<Button>();
        ChangeColorOfAButton(ref button, CurrentPlayerColor);
        UpdateScore();
        UpdateClickPower();
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
        public int grade;
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

            grade = 0;
        }
        public void ChangeColor(int num) {
            color = num;
            ChangeColorOfAButton(ref refs.button, num);
        }
        public void ChangeScore(int delta) {
            score += delta;
            refs.text.text = score.ToString();
        }
        public void ChangeGrade(int delta) {
            grade += delta;
            // update visuals?
        }
        public void DrawNode() {
            GameObject newobj = Instantiate(NodeObjectSpawner);
            newobj.name = number.ToString();
            refs.GenRefs(newobj);
            
            ChangeColor(color);
            ChangeScore(0);

            UnityEngine.Vector3 vec = new UnityEngine.Vector3(x, y, 0);
            refs.button.GetComponent<RectTransform>().anchoredPosition = vec;
        }
    }
    public struct GameState {
        public List<Node> nodes;
        public List<int> scores;
        public List<int> click_powers;
        public bool is_game_running;
        public GameState(int aboba) {
            nodes = new List<Node>();

            scores = new List<int>();
            click_powers = new List<int>();
            is_game_running = true;
            for (int i = 0; i < NumberOfColours; ++i) {
                scores.Add(0);
                click_powers.Add(1);
            }
        }
        public void RemoveDesk() {
            for (int i = 0; i < nodes.Count; ++i) {
                Node node = nodes[i];
                node.refs.ForgetRefs();
                nodes[i] = node;
            }

            for (int i = 0; i < NumberOfColours; ++i) { scores[i] = 0; }
            for (int i = 0; i < NumberOfColours; ++i) { click_powers[i] = 1; }
        }
        public void SetAnotherGameState(GameState another) {
            RemoveDesk();
            this = another;
            // DrawDesk();  // ?
        }
        public void DrawDesk() {
            RemoveDesk();
            for (int i = 0; i < nodes.Count; ++i) {
                Node node = nodes[i];
                node.DrawNode();
                nodes[i] = node;
            }
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
    //

    public void PressNode(GameObject NodeObject) {
        int num = int.Parse(NodeObject.name);
        Node node = state.nodes[num];
        int power = state.click_powers[CurrentPlayerColor];

        if (CurrentClickMode == 0) {
            if (CurrentPlayerColor == 0) {
                // player 0 does unique things
                if (node.score < power) node.ChangeColor(0);
                if (node.color == 0) { node.ChangeScore(power); }
                else { node.ChangeScore(-power); }
            } else if (node.color == CurrentPlayerColor) {
                node.ChangeScore(power);
            } else {
                int capture_constant = 100;
                bool capture_condition = node.color == 0;
                if (node.color != 0) {
                    capture_condition = TryToPay(capture_constant * Math.Min(power, node.score));
                }
                if (capture_condition) {
                    if (node.score < power) {
                        node.ChangeColor(CurrentPlayerColor);
                        node.ChangeScore(power - 2 * node.score);
                    } else {
                        node.ChangeScore(-power);
                    }
                }
            }
        } else if (CurrentClickMode == 1) {
            if (node.color == CurrentPlayerColor) {
                if (TryToPay(2 << node.grade)) node.ChangeGrade(1);
            }
        }

        state.nodes[num] = node;
    }
    public void IncreaseClickPower() {
        int cost = 100;  // a constant??
        if (state.scores[CurrentPlayerColor] >= cost) {
            state.scores[CurrentPlayerColor] -= cost;
            state.click_powers[CurrentPlayerColor] += 1;
        }
        UpdateClickPower();
        UpdateScore();
    }
    public void ChangeClickMode() {
        CurrentClickMode = (CurrentClickMode + 1) % NumberOfClickModes;
        ButtonChangeClickMode.GetComponentInChildren<TextMeshProUGUI>().text =
        ClickModes[CurrentClickMode];
        Debug.Log(CurrentClickMode);
    }

    public void Start() {
        NodeObjectSpawner = NodeObjectSpawner_tmp;  // lol wtf...
        ButtonToChangeColor = ButtonToChangeColor_tmp;
        ScoreTable = ScoreTable_tmp;
        ButtonIncreasePower = ButtonIncreasePower_tmp;
        ButtonChangeClickMode = ButtonChangeClickMode_tmp;
        ChangePlayerColor();
        ChangeClickMode(); ChangeClickMode();
    }

    public float GoldTimer = 0;
    void ChangeCurrentScore(int delta) {
        state.scores[CurrentPlayerColor] += delta;
        UpdateScore();
    }
    bool TryToPay(int cost) {
        if (state.scores[CurrentPlayerColor] >= cost) {
            ChangeCurrentScore(-cost);
            return true;
        }
        return false;
    }
    void UpdateScore() {
        ScoreTable.GetComponentInChildren<TextMeshProUGUI>().text =
        "Score: " + state.scores[CurrentPlayerColor].ToString();
    }
    void UpdateClickPower() {
        ButtonIncreasePower.GetComponentInChildren<TextMeshProUGUI>().text =
        "Click power: " + state.click_powers[CurrentPlayerColor].ToString();
    }
    void CountGold() {
        foreach(Node node in state.nodes) {
            state.scores[node.color] += node.score * (node.grade + 1);
        }
        UpdateScore();
    }
    public void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) { Application.Quit(); }
        if (!state.is_game_running) { return; }
        GoldTimer += Time.deltaTime;
        if (GoldTimer >= 1) {
            GoldTimer = 0;
            CountGold();
        }
    }
    public void DrawDesk() {
        state.DrawDesk();
    }
}







