using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    public Node start;
    public Node end;


    public Edge(Node s, Node e) {
        start = s;
        end = e;
    }
}
