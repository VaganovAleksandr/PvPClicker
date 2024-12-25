using System.Collections.Generic;
using System.Linq;
using DelaunatorSharp;
using Unity.VisualScripting;
using UnityEngine;


public class Graph {
    // Container for all nodes in graph
    public List<Node> nodes;
    // Adjacency matrix for all nodes in container nodes
    public List<Edge> edges;
    
    // Private function for generating nodes in this.nodes
    private void GenerateNodes(int num_nodes, Vector2 bounds, float gold_freq) {
        var random_coords = Randomizer.GetUniqueRandomVector2Array(bounds, num_nodes);

        for (int i = 0; i < num_nodes; ++i) {
            nodes.Add(new Node(random_coords[i].x, random_coords[i].y));
        }

        for (int i = 0; i <= Size() * gold_freq + 1; ++i) {
            nodes[i].SetNodeType(NodeType.Gold);
        }
    }

    // Private function for generating edges in graph. Uses Delone triangulation
    private void MakeEdges() {
        IPoint[] points = new IPoint[Size()];

        for (int i = 0; i < this.Size(); ++i) {
            points[i].X = nodes[i].coords.x;
            points[i].Y = nodes[i].coords.y;
        }

        Delaunator delaunator = new(points);
        var delaunay_edges = delaunator.GetEdges();

        foreach (var edge in delaunay_edges) {
            var s = edge.P;
            var t = edge.Q;
            
            edges.Add(new Edge(new Node((float)s.X, (float)s.Y), new Node((float)t.X, (float)t.Y)));
        }
    }

    // Function to generate graph
    void Generate(Vector2 bounds, int num_nodes=0, float gold_freq=0.1f) {
        /*
            bounds: Bounds of coordinates to generate points, must be two integers
            num_nodes: Number of nodes in generated graph
            gold_freq: Frequency of gold nodes in graph. Must be float value in [0, 1]
        */
        GenerateNodes(num_nodes, bounds, gold_freq);
        MakeEdges();
    }

    int Size() {
        return this.nodes.Count();
    }
}