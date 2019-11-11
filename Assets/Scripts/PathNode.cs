using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private PathNode[,] grid;
    public int x;
    public int z;
    public int gCost;
    public int hCost;
    public int fCost;
    public bool isWalkable;
    public string tag;
    public PathNode cameFromNode;

    public PathNode(PathNode[,] grid, int x, int z) {
        this.grid = grid;
        this.x = x;
        this.z = z;
        isWalkable = true;
    }

    public void CalculateFCost() {
        fCost = gCost + hCost;
    }

    public void SetIsWalkable(bool isWalkable) {
        this.isWalkable = isWalkable;
        //grid.TriggerGridObjectChanged(x, z);
    }

    public override string ToString() {
        return x + "," + z;
    }
}