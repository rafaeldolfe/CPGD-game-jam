using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Pathfinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    public static Pathfinding Instance { get; private set; }
    private MapGrid<GridContainer> grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;

    public Pathfinding(MapGrid<GridContainer> grid)
    {
        Debug.Log("Why?");
        Instance = this;
        this.grid = grid;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b) {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.z - b.z);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private List<PathNode> CalculatePath(PathNode endNode) {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.cameFromNode != null) {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList) {
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++) {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost) {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }

    public PathNode GetNode(int x, int y) {
        return grid.GetGridNode(x, y);
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode) {
        List<PathNode> neighbourList = new List<PathNode>();
        int level = Int32.Parse(currentNode.tag.Substring(1));

        if (currentNode.x - 1 >= 0) {
            // Left
            PathNode leftNode = GetNode(currentNode.x - 1, currentNode.z);
            int neighbourLevel = Int32.Parse(leftNode.tag.Substring(1));
            if (Math.Abs(neighbourLevel - level) < 2)
            {
                neighbourList.Add(leftNode);
            }
            // Left Down
            //if (currentNode.z - 1 >= 0) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.z - 1));
            // Left Up
            //if (currentNode.z + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.z + 1));
        }
        if (currentNode.x + 1 < grid.GetWidth()) {
            // Right
            PathNode rightNode = GetNode(currentNode.x + 1, currentNode.z);
            int neighbourLevel = Int32.Parse(rightNode.tag.Substring(1));
            if (Math.Abs(neighbourLevel - level) < 2)
            {
                neighbourList.Add(rightNode);
            }
            // Right Down
            //if (currentNode.z - 1 >= 0) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.z - 1));
            // Right Up
            //if (currentNode.z + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.z + 1));
        }
        // Down
        
        if (currentNode.z - 1 >= 0)
        {
            PathNode downNode = GetNode(currentNode.x, currentNode.z - 1);
            int neighbourLevel = Int32.Parse(downNode.tag.Substring(1));
            if (Math.Abs(neighbourLevel - level) < 2)
            {
                neighbourList.Add(downNode);
            }
        }   
        // Up
        if (currentNode.z + 1 < grid.GetHeight())
        {
            PathNode upNode = GetNode(currentNode.x, currentNode.z + 1);
            int neighbourLevel = Int32.Parse(upNode.tag.Substring(1));
            if (Math.Abs(neighbourLevel - level) < 2)
            {
                neighbourList.Add(upNode);
            }
        }
        

        return neighbourList;
    }

    public List<PathNode> FindPath(int startX, int startZ, int endX, int endZ)
    {
        Debug.Log("in FindPath...");
        Debug.Log(grid);
        PathNode startNode = grid.GetGridNode(startX, startZ);
        PathNode endNode = grid.GetGridNode(endX, endZ);

        if (startNode == null || endNode == null) {
            // Invalid Path
            return null;
        }

        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int z = 0; z < grid.GetHeight(); z++) {
                PathNode pathNode = grid.GetGridNode(x, z);
                pathNode.gCost = 99999999;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0) {
            PathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode) {
                // Reached final node
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode)) {
                if (closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.isWalkable) {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost) {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode)) {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }
        return null;
    }
}