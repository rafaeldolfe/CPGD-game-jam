using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Village : MonoBehaviour
{
    public GameObject villager;
    public GameObject trees;
    public Pathfinding pf;
    public MetaInformation mi;

    public void Start()
    {
        this.mi = gameObject.GetComponent<MetaInformation>();
    }
    public void Update()
    {
        MetaInformation treeMI = trees.GetComponent<MetaInformation>();
        if (villager.GetComponent<State>().GetState() == Constants.IDLE)
        {
            if (villager.GetComponent<MetaInformation>().x == mi.x && villager.GetComponent<MetaInformation>().z == mi.z)
            {
                if (pf.FindPath(mi.x, mi.z, treeMI.x, treeMI.z) != null)
                {
                    villager.GetComponent<Highlight>().PlaceFlag(treeMI.x, mi.grid.gridArray[treeMI.x, treeMI.z].height, treeMI.z, mi.flagPrefab);
                }
            }
            else
            {
                if (pf.FindPath(villager.GetComponent<MetaInformation>().x, villager.GetComponent<MetaInformation>().z, mi.x, mi.z) != null)
                {
                    villager.GetComponent<Highlight>().PlaceFlag(mi.x, mi.grid.gridArray[mi.x, mi.z].height, mi.z, mi.flagPrefab);
                }
            }
        }
    }
}
