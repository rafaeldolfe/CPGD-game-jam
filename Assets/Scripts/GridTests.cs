using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTests : MonoBehaviour
{
    public MapGrid<GridContainer> grid;
    public GameObject swamp;
    public GameObject farmland;
    public GameObject hill;
    void Start()
    {
        grid = new MapGrid<GridContainer>(10, 10, 1, new Vector3(0,0,0), (MapGrid<GridContainer> g, int x, int y) => new GridContainer(g, x, y));
        
        for (int x = 0; x < grid.gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < grid.gridArray.GetLength(1); y++)
            {
                //randomized choice of tile prefab
                int var = Random.Range(0, 1000);
                if (var % 9 == 0)
                {
                    grid.gridArray[x,y].AddUnit(hill);
                }
                else if (var % 3 == 0)
                {
                    grid.gridArray[x,y].AddUnit(farmland);
                }
                else
                {
                    grid.gridArray[x,y].AddUnit(swamp);
                }
            }
        }
    }
}
