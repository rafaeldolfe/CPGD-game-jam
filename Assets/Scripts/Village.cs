using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Village : MonoBehaviour
{
    public GameObject villager;
    public List<GameObject> trees;
    public Pathfinding pf;
    public GameManager gm;
    public MetaInformation mi;

    public bool holdingResource = false;

    public void Start()
    {
        this.mi = gameObject.GetComponent<MetaInformation>();
    }
    public void Update()
    {
        // List<List<PathNode>> paths;
        // foreach (GameObject tree in trees)
        // {
        //     MetaInformation treeMI = tree.GetComponent<MetaInformation>();

        //     paths.Add(pf.FindPath(villager.GetComponent<MetaInformation>().x, villager.GetComponent<MetaInformation>().z, treeMI.x, treeMI.z));
        // }

        // if (paths.Count == 0)
        // {
        //     return;
        // }

        // int count = int.MaxValue;
        // List<PathNode> minPath = null;


        // foreach (List<PathNode> path in paths)
        // {
        //     if (path.Count < count)
        //     {
        //         count = path.Count;
        //         minPath = path;
        //     }
        // }

        // if (villager.GetComponent<State>().GetState() == Constants.IDLE)
        // {
        //     if (villager.GetComponent<MetaInformation>().x == mi.x && villager.GetComponent<MetaInformation>().z == mi.z)
        //     {
        //         if (holdingResource)
        //         {
        //             gm.UpdateResources(1);
        //             holdingResource = false;
        //         }

        //         if (minPath != null)
        //         {
        //             villager.GetComponent<Highlight>().PlaceFlag(treeMI2.x, mi.grid.gridArray[treeMI2.x, treeMI2.z].height, treeMI2.z, mi.flagPrefab);
        //         }
        //     }
        // }




        MetaInformation treeMI2 = trees2.GetComponent<MetaInformation>();
        if (villager.GetComponent<State>().GetState() == Constants.IDLE)
        {
            if (villager.GetComponent<MetaInformation>().x == mi.x && villager.GetComponent<MetaInformation>().z == mi.z)
            {
                if (holdingResource)
                {
                    gm.UpdateResources(1);
                    holdingResource = false;
                }
                List<PathNode> paths1 = pf.FindPath(villager.GetComponent<MetaInformation>().x, villager.GetComponent<MetaInformation>().z, treeMI.x, treeMI.z);
                List<PathNode> paths2 = pf.FindPath(villager.GetComponent<MetaInformation>().x, villager.GetComponent<MetaInformation>().z, treeMI2.x, treeMI2.z);
                if (paths1 != null && paths2!= null)
                {
                    if (paths1.Count > paths2.Count)
                    {
                        villager.GetComponent<Highlight>().PlaceFlag(treeMI2.x, mi.grid.gridArray[treeMI2.x, treeMI2.z].height, treeMI2.z, mi.flagPrefab);
                    }
                    else
                    {
                        villager.GetComponent<Highlight>().PlaceFlag(treeMI.x, mi.grid.gridArray[treeMI.x, treeMI.z].height, treeMI.z, mi.flagPrefab);
                    }
                }
                else if (pf.FindPath(mi.x, mi.z, treeMI.x, treeMI.z) != null)
                {
                    villager.GetComponent<Highlight>().PlaceFlag(treeMI.x, mi.grid.gridArray[treeMI.x, treeMI.z].height, treeMI.z, mi.flagPrefab);
                }
                else if (pf.FindPath(mi.x, mi.z, treeMI2.x, treeMI2.z) != null)
                {
                    villager.GetComponent<Highlight>().PlaceFlag(treeMI2.x, mi.grid.gridArray[treeMI2.x, treeMI2.z].height, treeMI2.z, mi.flagPrefab);
                }
            }

            else if (villager.GetComponent<MetaInformation>().x == treeMI.x && villager.GetComponent<MetaInformation>().z == treeMI.z)
            {
                holdingResource = true;
                if (pf.FindPath(villager.GetComponent<MetaInformation>().x, villager.GetComponent<MetaInformation>().z, mi.x, mi.z) != null)
                {
                    villager.GetComponent<Highlight>().PlaceFlag(mi.x, mi.grid.gridArray[mi.x, mi.z].height, mi.z, mi.flagPrefab);
                }
            }

            else if (villager.GetComponent<MetaInformation>().x == treeMI2.x && villager.GetComponent<MetaInformation>().z == treeMI2.z)
            {
                holdingResource = true;
                if (pf.FindPath(villager.GetComponent<MetaInformation>().x, villager.GetComponent<MetaInformation>().z, mi.x, mi.z) != null)
                {
                    villager.GetComponent<Highlight>().PlaceFlag(mi.x, mi.grid.gridArray[mi.x, mi.z].height, mi.z, mi.flagPrefab);
                }
            }
        }
    }
}
