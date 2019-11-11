using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridContainer
{
    public List<GameObject> gos;
    public GameObject floor;
    private MapGrid<GridContainer> grid;
    private int x;
    private int z;

    public float height = 1.0f;
    public GridContainer(MapGrid<GridContainer> grid, int x, int z)
    {
        this.grid = grid;
        this.x = x;
        this.z = z;
        this.gos = new List<GameObject>(10);
    }

    public void SetFloor(GameObject unit, float height)
    {
        this.floor = unit;
        this.height = height;
    }

    public void AddUnit(int x, int z, GameObject unit)
    {
        Debug.Log(unit.GetComponent<MetaInformation>());
        //unit.GetComponent<MetaInformation>().x = x;
        //unit.GetComponent<MetaInformation>().z = z;
        gos.Add(unit);
    }

    public void RemoveUnit(GameObject unit)
    {
        gos.Remove(unit);
    }

    public bool RemoveBridge(){
        //this assumes only one bridge per tile
        for(int k = 0; k < gos.Count; k++ ){
            GameObject go = gos[k]; 
            if(go.tag == "Bridge"){
                gos.RemoveAt(k);
                Debug.Log("Bridge removed?? + " + (go == null));
                go.SetActiveRecursively(false); 
                return true; 
            }
        }
        return false; 
    }

    public void DespawnUnit(GameObject unit)
    {
        GameObject.Destroy(unit);
    }

    public void SpawnUnit(GameObject unit)
    {
        UnityEngine.Object.Instantiate(unit, unit.transform.position + new Vector3(x, 0, z), Quaternion.identity);
    }

    public override string ToString()
    {
        return x + "," + z;
    }
}
