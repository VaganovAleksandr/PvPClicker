using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameDrawer : MonoBehaviour
{
    public ObjectsHolder Holder;
    public GameObject minerNodePrefab;
    public GameObject defaultNodePrefab; // ?
    private GameObject GraphObject;
    public GameObject EdgePrefab;

    private GameCore gc;
    void Start() { gc = Holder.gameCore; }  // bad?
    public void DrawEdge(int a, int b) {
        Vector2 pos1 = gc.state.graph.nodes[a].GetCoordinate();
        Vector2 pos2 = gc.state.graph.nodes[b].GetCoordinate();

        GameObject NewEdge = Instantiate(EdgePrefab);
        NewEdge.transform.SetParent(GraphObject.transform);
        NewEdge.SetActive(true);
        var rect = NewEdge.GetComponent<RectTransform>();

        rect.position = (pos1 + pos2) / 2;
        rect.localScale = new Vector3(150, Vector2.Distance(pos1, pos2), 1);

        var radians = Math.Atan2(pos2.x - pos1.x, pos2.y - pos1.y);
        rect.Rotate(0, 0, -(float)(radians * (180/Math.PI)));
    }
    public void DrawNode(int node_num)
    {
        GameCore.Node node = Holder.gameCore.state.graph.nodes[node_num];
        GameObject newNode = (node.GetNodeType() == node.GetDefaultType())
            ? Instantiate(defaultNodePrefab)
            : Instantiate(minerNodePrefab);

        newNode.SetActive(true);
        newNode.GetComponent<Text>().text = node_num.ToString();
        newNode.transform.SetParent(GraphObject.transform);
        newNode.transform.position = new Vector2(node.GetCoordinate().x, node.GetCoordinate().y);

        node.unity_object = newNode;
        UpdateNode(node);
    }
    public void DrawGraph() {
        for (int i = 0; i < gc.state.graph.nodes.Count; ++i) {
            foreach (var j in gc.state.graph.nodes[i].edges) {
                if (i < j) DrawEdge(i, j);
            }
        }
        for (int i = 0; i < Holder.gameCore.state.graph.nodes.Count; ++i) DrawNode(i);
    }
    private void EraseScreen() {
        if (GraphObject != null) {
            Destroy(GraphObject);
            GraphObject = null;
        }
    }
    public void DrawGame() {
        EraseScreen();
        GraphObject = new GameObject();  // graph prefab?
        GraphObject.transform.name = "GraphObject";
        GraphObject.transform.SetParent(Holder.MainCanvas.transform);
        DrawGraph();

        UpdateAllStats();
    }



    // updating screen section
    public void UpdateScoreColor() {
        Holder.Score.GetComponent<Image>().color = GameCore.MyColors[gc.PlayerColor];
    }
    public void UpdateScore() {
        Holder.Score.GetComponentInChildren<TextMeshProUGUI>().text
        = "Score: " + gc.state.scores[gc.PlayerColor].ToString();
    }
    public void UpdatePowerClick() {
        Holder.ClickPowerButton.GetComponentInChildren<TextMeshProUGUI>().text
        = "Click power: " + gc.state.click_powers[gc.PlayerColor].ToString();
    }
    public void UpdateNode(GameCore.Node node) {
        node.unity_object.transform.Find("Circle").GetComponent<Image>().color = GameCore.MyColors[node.GetColor()];
        node.unity_object.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = node.GetValue().ToString();
        node.unity_object.transform.Find("Grade").GetComponent<TextMeshProUGUI>().text = node.GetGrade().ToString();
    }
    public void UpdateEndGame() {
        Holder.EndGameObject.SetActive(!gc.state.running_state[gc.PlayerColor]);
    }
    public void UpdateAllStats() {
        UpdateScore();
        UpdateScoreColor();
        UpdatePowerClick();
        UpdateEndGame();
    }

}
