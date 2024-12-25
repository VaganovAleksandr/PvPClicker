using UnityEngine;


// States of node in graph: free if no player controls it, occupied otherwise
public enum NodeState {
    Free, 
    Occupied
}

// Type of node in graph: pointers to resource that the node produces
public enum NodeType {
    Gold, 
    Mana
}

// Node class for graph
public class Node {
    // Current power of node
    public int value = 0;

    public Vector2 coords = new(0, 0);

    public NodeState state = NodeState.Free;
    public NodeType type = NodeType.Mana;

    public Node(float x, float y) {
        coords.x = x;
        coords.y = y;
    }

    // Increase value of node manually
    public void IncreaseValue(int value) {
        this.value += value;
    }

    // Set state of node manually
    public void SetState(NodeState state) {
        this.state = state;
    }

    // Get occupancy (donbass) state of node
    public NodeState GetState() {
        return this.state;
    }

    // Set type of node manually
    public void SetNodeType(NodeType type) {
        this.type = type;
    }

    // Get resource type of node
    public NodeType GetNodeType() {
        return this.type;
    }

    // Set coordinates of node manually
    public void SetCoords(Vector2 coords) {
        this.coords = coords;
    }

    // Get coords of node
    public Vector2 GetCoords() {
        return this.coords;
    }

    // Get current power of node
    public int Produce() {
        return value;
    }
}