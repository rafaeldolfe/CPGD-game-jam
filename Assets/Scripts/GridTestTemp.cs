using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTestTemp : MonoBehaviour
{
    // private MapGrid<GridContainer> grid;
    // public GameObject smallPrefab;
    // public GameObject mediumPrefab;
    // public GameObject largePrefab;
    // public GameObject unit;
    // public GameObject flagPrefab;
    // public GameObject villager1Prefab;
    // public GameObject villager2Prefab;
    // public GameObject mediumVillage;
    // public GameObject mediumForest; 
    // public GameObject water; 
    // public List<GameObject> units = new List<GameObject>();
    // private GameObject selected;

//     #region GridTest
//     public static void GenerateGrid()
//     {

//         grid = new MapGrid<GridContainer>(10, 10, 1, new Vector3(0,0,0), (MapGrid<GridContainer> g, int x, int y) => new GridContainer(g, x, y));

//         for (int x = 0; x < grid.gridArray.GetLength(0); x++)
//         {
//             for (int z = 0; z < grid.gridArray.GetLength(1); z++)
//             {
//                 grid.gridArray[x,z].AddUnit(x, z, water);
//             }
//         }
        
//         grid.gridArray[4,8].AddUnit(4, 8, mediumForest); 
//         grid.gridArray[5,8].AddUnit(5, 8, medium);
//         grid.gridArray[5,7].AddUnit(5, 7, medium);
//         grid.gridArray[4,7].AddUnit(4, 7, medium);
//         grid.gridArray[7,5].AddUnit(7, 5, mediumVillage);
//         grid.gridArray[2,5].AddUnit(2, 5, mediumForest);
//         grid.gridArray[8,3].AddUnit(8, 3, medium);
//         grid.gridArray[3,2].AddUnit(3, 2, medium);
//         grid.gridArray[2,2].AddUnit(2, 2, medium);

//         grid.gridArray[6,7].AddUnit(6, 7, large);
//         grid.gridArray[1,5].AddUnit(1, 5, large);
//         grid.gridArray[1,4].AddUnit(1, 4, large);
//         grid.gridArray[3,3].AddUnit(3, 3, large);

//         grid.gridArray[3,6].AddUnit(3, 6, small);
//         grid.gridArray[4,6].AddUnit(4, 6, small);
//         grid.gridArray[4,3].AddUnit(4, 3, small);
//         grid.gridArray[4,2].AddUnit(4, 2, small);
//         grid.gridArray[5,2].AddUnit(5, 2, small);
//         grid.gridArray[7,3].AddUnit(7, 3, small);


//         for (int x = 0; x < grid.gridArray.GetLength(0); x++)
//         {
//             for (int y = 0; y < grid.gridArray.GetLength(1); y++)
//             {
//                 //randomized choice of tile prefab
//                 int var = Random.Range(0, 1000);
//                 if (var % 9 == 0)
//                 {
//                     GameObject large = UnityEngine.Object.Instantiate(largePrefab, largePrefab.transform.position + new Vector3(x, 0, z), Quaternion.identity);
//                     grid.gridArray[x,z].SetFloor(large, 1.5f);
//                 }
//                 else if (var % 3 == 0)
//                 {
//                     GameObject farmland = UnityEngine.Object.Instantiate(farmlandPrefab, farmlandPrefab.transform.position + new Vector3(x, 0, z), Quaternion.identity);
//                     grid.gridArray[x,z].SetFloor(farmland, 1.25f);
//                 }
//                 else
//                 {
//                     GameObject swamp = UnityEngine.Object.Instantiate(swampPrefab, swampPrefab.transform.position + new Vector3(x, 0, z), Quaternion.identity);
//                     grid.gridArray[x,z].SetFloor(swamp, 1.0f);
//                 }
//             }
//         }



//         GameObject newUnit = UnityEngine.Object.Instantiate(unit, unit.transform.position + new Vector3(1, 0, 1), Quaternion.identity);
//         newUnit.GetComponent<MetaInformation>().init(1, 1);
//         unit.GetComponent<MoveQueue>().AddMove(new Vector3(3, grid.gridArray[3,3].height, 3));
//         grid.gridArray[1,1].AddUnit(1, 1, newUnit);
//         units.Add(newUnit);

//         GameObject villager1 = UnityEngine.Object.Instantiate(villager1Prefab, villager1Prefab.transform.position + new Vector3(3, 0, 2), Quaternion.identity);
//         grid.gridArray[3,2].AddUnit(3, 2, villager1);

//         GameObject villager2 = UnityEngine.Object.Instantiate(villager2Prefab, villager2Prefab.transform.position + new Vector3(2, 0, 3), Quaternion.identity);
//         grid.gridArray[2,3].AddUnit(2, 3, villager2);
//     }

//     public void Update()
//     {
//         if (Input.GetMouseButtonDown(0))
//         {
//             RaycastHit hit;
//             Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
//             if (Physics.Raycast(ray, out hit)) 
//             {
//                 GameObject objectHit = hit.transform.gameObject;
                
//                 Select(objectHit);
//             }
//         }

//         if (Input.GetMouseButtonDown(1))
//         {
//             RaycastHit hit;
//             Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
//             if (Physics.Raycast(ray, out hit)) 
//             {
//                 int x;
//                 int z;
//                 grid.GetXY(hit.transform.position, out x, out z);
//                 // Only allow this flag setting to occur if the targeted game object is a tile
//                 if(selected.GetComponent<Unit>())
//                 {
//                     Debug.Log(selected);
//                     selected.GetComponent<Highlight>().PlaceFlag(x, z, flagPrefab);
//                 }
//             }
//         }

//         foreach (var unit in units)
//         {
//             if(unit.GetComponent<State>().GetState() == Constants.IDLE)
//             {
//                 MetaInformation mi = unit.GetComponent<MetaInformation>();
//                 int movex = Random.Range(0,2);
//                 int movez = Random.Range(0,2);
//                 if (movex == 1)
//                 {
//                     unit.GetComponent<MoveQueue>().AddMove(new Vector3(mi.x + movex, grid.gridArray[mi.x,mi.z].height, mi.z));
//                 }
//                 else if (movez == 1)
//                 {
//                     unit.GetComponent<MoveQueue>().AddMove(new Vector3(mi.x, grid.gridArray[mi.x,mi.z].height, mi.z + movez));
//                 }
//             }
//         }
//     }

//     public void Select(GameObject go)
//     {
//         if (selected == go)
//         {
//             selected.GetComponent<Highlight>().Deselect();
//             selected.GetComponent<Highlight>().HideFlag();
//             selected = null;
//             return;
//         }
//         else if (selected)
//         {
//             selected.GetComponent<Highlight>().Deselect();
//             selected.GetComponent<Highlight>().HideFlag();
//             selected = null;
//         }

//         if (go.GetComponent<Highlight>())
//         {
//             go.GetComponent<Highlight>().Select();
//             go.GetComponent<Highlight>().ShowFlag();
//             selected = go;
//         }
//     }

//     public static Vector3 GetMouseWorldPosition() 
//     {
//         Vector3 vec = GetMouseWorldPositionWithY(Input.mousePosition, Camera.main);
//         vec.y = 0f;
//         return vec;
//     }
//     public static Vector3 GetMouseWorldPositionWithY() {
//         return GetMouseWorldPositionWithY(Input.mousePosition, Camera.main);
//     }
//     public static Vector3 GetMouseWorldPositionWithY(Camera worldCamera) {
//         return GetMouseWorldPositionWithY(Input.mousePosition, worldCamera);
//     }
//     public static Vector3 GetMouseWorldPositionWithY(Vector3 screenPosition, Camera worldCamera) {
//         Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
//         return worldPosition;
//     }

//     #endregion
}
