using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridContainer : MonoBehaviour
{
    List<GameObject> units;
    private MapGrid<GridContainer> grid;
    private int x;
    private int z;
    public GridContainer(MapGrid<GridContainer> grid, int x, int z)
    {
        this.grid = grid;
        this.x = x;
        this.z = z;
        this.units = new List<GameObject>();
    }

    public override string ToString()
    {
        return x + "," + z;
    }

    public void AddUnit(GameObject unit)
    {
        units.Add(unit);
        SpawnUnit(unit);
    }

    public void RemoveUnit(GameObject unit)
    {
        units.Remove(unit);
        SpawnUnit(unit);
    }

    public void DespawnUnit(GameObject unit)
    {
        Destroy(unit);
    }

    public void SpawnUnit(GameObject unit)
    {
        UnityEngine.Object.Instantiate(unit, new Vector3(x, 0, z), Quaternion.identity);
    }
}
