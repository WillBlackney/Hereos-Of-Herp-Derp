using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public Point GridPosition { get; private set; }

    public TileScript TileRef { get; private set; }

    public Vector2 WorldPosition { get; set; }

    public Node Parent { get; private set; }

    public int G { get; set; }
    public int H { get; set; }
    public int F { get; set; }

    public Node(TileScript tileRef)
    {
        Debug.Log("Calling node constructor");
        this.TileRef = tileRef;
        this.GridPosition = tileRef.GridPosition;
        this.WorldPosition = tileRef.WorldPosition;
        
    }

    public void CalcValues(Node parent, Node goal, int gCost)
    {
        this.Parent = parent;
        this.G = parent.G + gCost;
        this.H = ((Mathf.Abs(GridPosition.X - goal.GridPosition.X)) + Mathf.Abs((goal.GridPosition.Y - GridPosition.Y))) * 10;
        this.F = G + H;
    }

}
