using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
public class GameManager : MonoBehaviour
{
    public Camera mainCamera;

    //UI (menu) vars
    public TextMeshProUGUI bridgesText;
    public GameObject titleScreen;
    public GameObject gameOverScreen;
    public Color titleBackgroundColor, gameBackgroundColor;

    #region GridTestVars
    public MapGrid<GridContainer> grid;
    public GameObject smallPrefab;
    public GameObject mediumPrefab;
    public GameObject largePrefab;
    public GameObject unitPrefab;
    public GameObject flagPrefab;
    public GameObject villager1Prefab;
    public GameObject villager2Prefab;
    public GameObject mediumVillagePrefab;
    public GameObject mediumForestPrefab; 
    public GameObject waterPrefab; 
    public List<GameObject> units = new List<GameObject>();
    private GameObject selected;
    #endregion

    //UI (gameplay) vars
    public GameObject bridgeE1_Left;
    public GameObject bridgeE1_Right;
    public GameObject bridgeE1Corner_Down;
    public GameObject bridgeE1Corner_Left;
    public GameObject bridgeE1Corner_Up;
    public GameObject bridgeE1Corner_Right;
    public GameObject bridgeE2_Left;
    public GameObject bridgeE2_Right;
    public GameObject bridgeE2Corner_Down;
    public GameObject bridgeE2Corner_Left;
    public GameObject bridgeE2Corner_Up;
    public GameObject bridgeE2Corner_Right;

    public float bridgeE1_y = .1875f;
    public float bridgeE2_y = .4375f;

    //public vars
    public bool isGameActive;

    //private vars
    private int bridges;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        mainCamera.clearFlags = CameraClearFlags.SolidColor;
        mainCamera.backgroundColor = titleBackgroundColor;
    }

    // Update is called once per frame
    void Update()
    {
        
        MouseGridCommands();
        checkIfGameOver();
    }

    public void StartGame(int difficulty)
    {
        isGameActive = true;
        mainCamera.backgroundColor = gameBackgroundColor;

        bridgesText.gameObject.SetActive(true);
        bridges = 0;
        UpdateBridges(0);

        float spawnRate = 0.5f / difficulty;
        StartCoroutine(Example(spawnRate)); //do you need to do something over time? it needs to be a coroutine

        titleScreen.gameObject.SetActive(false);

        GenerateGrid();
        TestGenerateBridge();
    }

    #region Update Game (example): gain 1 bridge every 1 or 1/2 or 1/3 sec (difficulty determines this), game over at 10 bridges

    private void checkIfGameOver()
    {
        if (bridges == 50) { GameOver(); }
    }

    void UpdateBridges(int bridgesChange)
    {
        if (isGameActive)
        {
            bridges += bridgesChange;
            bridgesText.text = "Bridges: " + bridges;
        }
    }

    IEnumerator Example(float spawnRate)
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            UpdateBridges(1);
            if (!isGameActive) { break; }
        }
    }

    #endregion


    void TestGenerateBridge() //implemented gridTest's grid generation into GameManager
    {

        grid.gridArray[1, 0].AddUnit(1, 0, bridgeE2_Left);
        grid.gridArray[0, 0].AddUnit(0, 0, bridgeE2Corner_Down);
        grid.gridArray[0, 1].AddUnit(0, 1, bridgeE2_Right);
    }

    public void GameOver()
    {
        mainCamera.backgroundColor = titleBackgroundColor;
        isGameActive = false;
        gameOverScreen.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    #region GridTest
    public void GenerateGrid()
    {
        Debug.Log("GenerateGrid");
        grid = new MapGrid<GridContainer>(10, 10, 1, new Vector3(0,0,0), (MapGrid<GridContainer> g, int x, int y) => new GridContainer(g, x, y));

        //GenerateRandomGrid();
        GenerateLevelGrid();

        GameObject newUnit = UnityEngine.Object.Instantiate(unitPrefab, unitPrefab.transform.position + new Vector3(1, 0, 1), Quaternion.identity);
        newUnit.GetComponent<MetaInformation>().init(1, 1);
        unitPrefab.GetComponent<MoveQueue>().AddMove(new Vector3(3, grid.gridArray[3,3].height, 3));
        grid.gridArray[1,1].AddUnit(1, 1, newUnit);
        units.Add(newUnit);

        GameObject villager1 = UnityEngine.Object.Instantiate(villager1Prefab, villager1Prefab.transform.position + new Vector3(3, 0, 2), Quaternion.identity);
        grid.gridArray[3,2].AddUnit(3, 2, villager1);

        GameObject villager2 = UnityEngine.Object.Instantiate(villager2Prefab, villager2Prefab.transform.position + new Vector3(2, 0, 3), Quaternion.identity);
        grid.gridArray[2,3].AddUnit(2, 3, villager2);

    }

    public void GenerateRandomGrid (){
    for (int x = 0; x < grid.gridArray.GetLength(0); x++)
        {
            for (int z = 0; z < grid.gridArray.GetLength(1); z++)
            {
                //randomized choice of tile prefab
                int var = UnityEngine.Random.Range(0, 1000);
                if (var % 9 == 0)
                {
                    GenerateFloorTile("large", x, z);
                }
                else if (var % 3 == 0)
                {
                    GenerateFloorTile("medium", x, z);
                }
                else
                {
                    GenerateFloorTile("small", x, z);
                }
            }
        }
    }

    public void GenerateLevelGrid(){
        
        Debug.Log("GeneratingWater");
        for (int x = 0; x < grid.gridArray.GetLength(0); x++)
        {
            for (int z = 0; z < grid.gridArray.GetLength(1); z++)
            {
                GameObject water = UnityEngine.Object.Instantiate(waterPrefab, waterPrefab.transform.position + new Vector3(x, -0.5f, z), Quaternion.identity);
                grid.gridArray[x,z].SetFloor(water, 0.0f);

            }
        }
        Debug.Log("GenerateGrid");
        
        /*grid.gridArray[4,8].AddUnit(4, 8, mediumForestPrefab); 
        grid.gridArray[5,8].AddUnit(5, 8, mediumPrefab);
        grid.gridArray[5,7].AddUnit(5, 7, mediumPrefab);
        grid.gridArray[4,7].AddUnit(4, 7, mediumPrefab);
        grid.gridArray[7,5].AddUnit(7, 5, mediumVillagePrefab);
        grid.gridArray[2,5].AddUnit(2, 5, mediumForestPrefab);
        grid.gridArray[8,3].AddUnit(8, 3, mediumPrefab);
        grid.gridArray[3,2].AddUnit(3, 2, mediumPrefab);
        grid.gridArray[2,2].AddUnit(2, 2, mediumPrefab);

        grid.gridArray[6,7].AddUnit(6, 7, largePrefab);
        grid.gridArray[1,5].AddUnit(1, 5, largePrefab);
        grid.gridArray[1,4].AddUnit(1, 4, largePrefab);
        grid.gridArray[3,3].AddUnit(3, 3, largePrefab);

        grid.gridArray[3,6].AddUnit(3, 6, smallPrefab);
        grid.gridArray[4,6].AddUnit(4, 6, smallPrefab);
        grid.gridArray[4,3].AddUnit(4, 3, smallPrefab);
        grid.gridArray[4,2].AddUnit(4, 2, smallPrefab);
        grid.gridArray[5,2].AddUnit(5, 2, smallPrefab);
        grid.gridArray[7,3].AddUnit(7, 3, smallPrefab);*/

        GameObject medForest = UnityEngine.Object.Instantiate(mediumForestPrefab,mediumForestPrefab.transform.position + new Vector3(4, 0, 8), Quaternion.identity);
        grid.gridArray[4,8].SetFloor(medForest, 1.25f);
        medForest = UnityEngine.Object.Instantiate(mediumForestPrefab,mediumForestPrefab.transform.position + new Vector3(2, 0, 5), Quaternion.identity);
        grid.gridArray[2,5].SetFloor(medForest, 1.25f);
        GameObject medVillage = UnityEngine.Object.Instantiate(mediumVillagePrefab,mediumVillagePrefab.transform.position + new Vector3(7, 0, 5), Quaternion.identity);
        grid.gridArray[7,5].SetFloor(medForest, 1.25f);

        GenerateFloorTile("medium", 5,8); 
        GenerateFloorTile("medium", 5,7); 
        GenerateFloorTile("medium", 4,7);  
        GenerateFloorTile("medium", 8,3);
        GenerateFloorTile("medium", 3,2);
        GenerateFloorTile("medium", 2,2);

        GenerateFloorTile("large", 6,7); 
        GenerateFloorTile("large", 1,5); 
        GenerateFloorTile("large", 1,4); 
        GenerateFloorTile("large", 3,3);

        GenerateFloorTile("small", 3,6);
        GenerateFloorTile("small", 4,6);
        GenerateFloorTile("small", 4,3);
        GenerateFloorTile("small", 4,2);
        GenerateFloorTile("small", 5,2);
        GenerateFloorTile("small", 7,3); 


    }

    public void GenerateFloorTile(String size, int x, int z) {

        if(size == "small"){
            GameObject small = UnityEngine.Object.Instantiate(smallPrefab, smallPrefab.transform.position + new Vector3(x, 0, z), Quaternion.identity);
            grid.gridArray[x,z].SetFloor(small, 1.0f);
        }else if(size == "medium"){
            GameObject medium = UnityEngine.Object.Instantiate(mediumPrefab, mediumPrefab.transform.position + new Vector3(x, 0, z), Quaternion.identity);
            grid.gridArray[x,z].SetFloor(medium, 1.25f);
        }else if(size == "large"){
            GameObject large = UnityEngine.Object.Instantiate(largePrefab, largePrefab.transform.position + new Vector3(x, 0, z), Quaternion.identity);
            grid.gridArray[x,z].SetFloor(large, 1.5f);
        }
    }

    public void MouseGridCommands()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
            if (Physics.Raycast(ray, out hit)) 
            {
                GameObject objectHit = hit.transform.gameObject;
                
                Select(objectHit);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
            if (Physics.Raycast(ray, out hit)) 
            {
                int x;
                int z;
                grid.GetXY(hit.transform.position, out x, out z);
                // Only allow this flag setting to occur if the targeted game object is a tile
                if(selected.GetComponent<Unit>())
                {
                    Debug.Log(selected);
                    selected.GetComponent<Highlight>().PlaceFlag(x, z, flagPrefab);
                }
            }
        }

        // foreach (var unitPrefab in units)
        // {
        //     if(unitPrefab.GetComponent<State>().GetState() == Constants.IDLE)
        //     {
        //         MetaInformation mi = unitPrefab.GetComponent<MetaInformation>();
        //         int movex = Random.Range(0,2);
        //         int movez = Random.Range(0,2);
        //         if (movex == 1)
        //         {
        //             unitPrefab.GetComponent<MoveQueue>().AddMove(new Vector3(mi.x + movex, grid.gridArray[mi.x,mi.z].height, mi.z));
        //         }
        //         else if (movez == 1)
        //         {
        //             unitPrefab.GetComponent<MoveQueue>().AddMove(new Vector3(mi.x, grid.gridArray[mi.x,mi.z].height, mi.z + movez));
        //         }
        //     }
        // }
    }

    public void Select(GameObject go)
    {
        if (selected == go)
        {
            selected.GetComponent<Highlight>().Deselect();
            selected.GetComponent<Highlight>().HideFlag();
            selected = null;
            return;
        }
        else if (selected)
        {
            selected.GetComponent<Highlight>().Deselect();
            selected.GetComponent<Highlight>().HideFlag();
            selected = null;
        }

        if (go.GetComponent<Highlight>())
        {
            go.GetComponent<Highlight>().Select();
            go.GetComponent<Highlight>().ShowFlag();
            selected = go;
        }
    }

    public static Vector3 GetMouseWorldPosition() 
    {
        Vector3 vec = GetMouseWorldPositionWithY(Input.mousePosition, Camera.main);
        vec.y = 0f;
        return vec;
    }
    public static Vector3 GetMouseWorldPositionWithY() {
        return GetMouseWorldPositionWithY(Input.mousePosition, Camera.main);
    }
    public static Vector3 GetMouseWorldPositionWithY(Camera worldCamera) {
        return GetMouseWorldPositionWithY(Input.mousePosition, worldCamera);
    }
    public static Vector3 GetMouseWorldPositionWithY(Vector3 screenPosition, Camera worldCamera) {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    #endregion

}
