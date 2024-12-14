using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Graph {
    // Container for all nodes in graph
    public List<Node> nodes;
    // Adjacency matrix for all nodes in container nodes
    public List<Edge> edges;
    
    // Private function for generating nodes in this.nodes
    private void GenerateNodes(int num_nodes) {
        for (int i = 0; i < num_nodes; ++i) {
            nodes.Add(new Node());
        }
    }

    // Private function for generating edges in graph. Uses Delone triangulation
    private void MakeEdges() {
        // skip
    }

    // Function to generate graph
    void Generate(Vector2 bounds, int num_nodes=0, float gold_freq=0.1f) {
        /*
            bounds: Bounds of coordinates to generate points, 
            num_nodes: Number of nodes in generated graph
            gold_freq: Frequency of gold nodes in graph. Must be float value in [0, 1]
        */
        GenerateNodes(num_nodes);
        MakeEdges();
    }

    int Size() {
        return this.nodes.Count();
    }
}