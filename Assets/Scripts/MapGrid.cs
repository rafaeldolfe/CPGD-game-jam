using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class MapGrid<TGridObject>
{
    public const float DEFAULT_Z = -1f;

    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs
    {
        public int x;
        public int y;
    }

    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;

    public TGridObject[,] gridArray;

    public MapGrid(int width, int height, float cellSize, Vector3 originPosition, Func<MapGrid<TGridObject>, int, int, TGridObject> initObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridObject[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = initObject(this, x, y);
            }
        }

        // if (DebugStore.debugMode)
        // {
        //     for (int x = 0; x < gridArray.GetLength(0); x++)
        //     {
        //         for (int y = 0; y < gridArray.GetLength(1); y++)
        //         {
        //             Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
        //             Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
        //         }
        //     }
        //     Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        //     Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
        // }
    }
    private Vector3 GetWorldPosition(int x, int y)
    {
        Vector3 pos = new Vector3(x, y) * cellSize + originPosition;
        pos.z = DEFAULT_Z;
        return pos;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int z)
    {
        Vector3 newWorldPosition = worldPosition;
        newWorldPosition.y = 0;
        x = Mathf.FloorToInt((newWorldPosition - originPosition).x / cellSize);
        z = Mathf.FloorToInt((newWorldPosition - originPosition).z / cellSize);
    }

    public TGridObject GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(TGridObject);
        }
    }

    public TGridObject GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }
}
