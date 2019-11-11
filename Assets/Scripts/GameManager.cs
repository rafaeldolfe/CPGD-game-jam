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
    public TextMeshProUGUI timerText;
    public GameObject titleScreen;
    public GameObject gameOverScreen;
    public Color titleBackgroundColor, gameBackgroundColor, bridgeBackgroundColor;
    public Pathfinding pf;

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
    private GameObject selectedBridgeTile; 
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

    //public float bridgeE1_y = .1875f;
    public float bridgeE1_y = -.0625f;
    public float bridgeE2_y = .4375f;
    public float bridgeE3_y = .9375f;

    //public vars
    public bool isGameActive;

    //private vars
    private int timeLeft = 50;
    private bool inBridgeMode = false; 

    // Start is called before the first frame update
    void Start()
    {
        grid = new MapGrid<GridContainer>(10, 10, 1, new Vector3(0,0,0), (MapGrid<GridContainer> g, int x, int y) => new GridContainer(g, x, y));
        pf = new Pathfinding(grid);
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        mainCamera.clearFlags = CameraClearFlags.SolidColor;
        mainCamera.backgroundColor = titleBackgroundColor;

        Debug.Log("time left: " + timeLeft);
    }

    // Update is called once per frame
    void Update()
    {
        MouseSelection();
        EnterBridgeMode(); 
        checkIfGameOver();
    }

    public void StartGame(int difficulty)
    {
        isGameActive = true;
        mainCamera.backgroundColor = gameBackgroundColor;

        timerText.gameObject.SetActive(true);
        UpdateBridges(0);

        float timeRate = 1.0f / difficulty;
        StartCoroutine(Example(timeRate)); //do you need to do something over time? it needs to be a coroutine

        titleScreen.gameObject.SetActive(false);

        GenerateGrid();
    }

    #region Update Game (example): gain 1 bridge every 1 or 1/2 or 1/3 sec (difficulty determines this), game over at 10 bridges

    private void checkIfGameOver()
    {
        if (timeLeft == 0) { GameOver(); }
    }

    void UpdateBridges(int timeChange)
    {
        if (isGameActive)
        {
            timeLeft -= timeChange;
            timerText.text = "Time: " + timeLeft;
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


    // void TestGenerateBridge() //implemented gridTest's grid generation into GameManager
    // {
    //     GameObject leftclone = UnityEngine.Object.Instantiate(bridgeE2_Left, bridgeE2_Left.transform.position + new Vector3(1, 0, 0), bridgeE2_Left.transform.rotation);
    //     grid.gridArray[1, 0].AddUnit(1, 0, leftclone);
    //     GameObject cornerclone = UnityEngine.Object.Instantiate(bridgeE2Corner_Down, bridgeE2Corner_Down.transform.position + new Vector3(0, 0, 0), bridgeE2Corner_Down.transform.rotation);
    //     grid.gridArray[0, 0].AddUnit(0, 0, cornerclone);
    //     GameObject rightclone = UnityEngine.Object.Instantiate(bridgeE2_Right, bridgeE2_Right.transform.position + new Vector3(0, 0, 1), bridgeE2_Right.transform.rotation);
    //     grid.gridArray[0, 1].AddUnit(0, 1, rightclone);
    // }

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
        GenerateLevelGrid();
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
        
        for (int x = 0; x < grid.gridArray.GetLength(0); x++)
        {
            for (int z = 0; z < grid.gridArray.GetLength(1); z++)
            {
                GameObject water = UnityEngine.Object.Instantiate(waterPrefab, waterPrefab.transform.position + new Vector3(x, -0.5f, z), Quaternion.identity);
                grid.gridArray[x,z].SetFloor(water, 0.0f);
                grid.pathNodes[x,z].isWalkable = false;
                grid.pathNodes[x,z].tag = "E0";
            }
        }

        for (int x = 0; x < grid.gridArray.GetLength(0); x++)
        {
            for (int z = 0; z < grid.gridArray.GetLength(1); z++)
            {
                //GameObject small = UnityEngine.Object.Instantiate(smallPrefab, smallPrefab.transform.position + new Vector3(x, -0.5f, z), Quaternion.identity);
                //grid.gridArray[x,z].SetFloor(small, 0.0f);
            }
        }
        Debug.Log("GenerateGrid");

        GameObject medForest = UnityEngine.Object.Instantiate(mediumForestPrefab,mediumForestPrefab.transform.position + new Vector3(4, 0, 8), Quaternion.identity);
        grid.gridArray[4,8].SetFloor(medForest, 0.75f);
        grid.pathNodes[4,8].isWalkable = true;
        grid.pathNodes[4,8].tag = mediumForestPrefab.tag;
        medForest = UnityEngine.Object.Instantiate(mediumForestPrefab,mediumForestPrefab.transform.position + new Vector3(2, 0, 5), Quaternion.identity);
        grid.gridArray[2,5].SetFloor(medForest, 0.75f);
        grid.pathNodes[2,5].isWalkable = true;
        grid.pathNodes[2,5].tag = mediumForestPrefab.tag;
        // GameObject medVillage = UnityEngine.Object.Instantiate(mediumVillagePrefab,mediumVillagePrefab.transform.position + new Vector3(8, 0, 5), Quaternion.identity);
        // grid.gridArray[8,5].SetFloor(medVillage, 0.25f + 0.5f);
        // grid.pathNodes[8,5].isWalkable = true;
        // grid.pathNodes[8,5].tag = mediumVillagePrefab.tag;
        GameObject medVillage = UnityEngine.Object.Instantiate(mediumVillagePrefab,mediumVillagePrefab.transform.position + new Vector3(5, 0, 3), Quaternion.identity);
        grid.gridArray[5,3].SetFloor(medVillage, 0.75f);
        grid.pathNodes[5,3].isWalkable = true;
        grid.pathNodes[5,3].tag = mediumVillagePrefab.tag;
        GameObject newUnit = UnityEngine.Object.Instantiate(unitPrefab, unitPrefab.transform.position + new Vector3(5, 0, 3), Quaternion.identity);
        grid.gridArray[5,3].AddUnit(5, 3, newUnit);
        units.Add(newUnit);

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
        GenerateFloorTile("large", 1,2);


        GenerateFloorTile("small", 8,5);
        GenerateFloorTile("small", 3,6);
        GenerateFloorTile("small", 4,6);
        GenerateFloorTile("small", 4,3);
        GenerateFloorTile("small", 4,2);
        GenerateFloorTile("small", 5,2);
        GenerateFloorTile("small", 7,3); 
        GenerateFloorTile("small", 7,2);
    }

    public void GenerateFloorTile(String size, int x, int z) {

        if(size == "small"){
            GameObject small = UnityEngine.Object.Instantiate(smallPrefab, smallPrefab.transform.position + new Vector3(x, 0, z), Quaternion.identity);
            grid.gridArray[x,z].SetFloor(small, -0.25f + 0.5f);
            grid.pathNodes[x,z].isWalkable = true;
            grid.pathNodes[x,z].tag = smallPrefab.tag;
        }else if(size == "medium"){
            GameObject medium = UnityEngine.Object.Instantiate(mediumPrefab, mediumPrefab.transform.position + new Vector3(x, 0, z), Quaternion.identity);
            grid.gridArray[x,z].SetFloor(medium, 0.25f + 0.5f);
            grid.pathNodes[x,z].isWalkable = true;
            grid.pathNodes[x,z].tag = mediumPrefab.tag;
        }else if(size == "large"){
            GameObject large = UnityEngine.Object.Instantiate(largePrefab, largePrefab.transform.position + new Vector3(x, 0, z), Quaternion.identity);
            grid.gridArray[x,z].SetFloor(large, 0.75f + 0.5f);
            grid.pathNodes[x,z].isWalkable = true;
            grid.pathNodes[x,z].tag = largePrefab.tag;
        }
    }

    
    public void MouseSelection(){
        if(inBridgeMode){
            BridgeSelectionCommands(); 
        }else{
            MouseGridCommands();
        }
    }

    public void BridgeSelectionCommands(){
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
            if (Physics.Raycast(ray, out hit)) 
            {
                GameObject objectHit = hit.transform.gameObject;
                
                SelectBridgeTile(objectHit);
            }
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
                    MetaInformation mi = selected.GetComponent<MetaInformation>();
                    Debug.Log(selected);
                    selected.GetComponent<MoveQueue>().Clear();
                    selected.GetComponent<Highlight>().RemoveFlag();
                    if (grid.pathNodes[x,z].isWalkable)
                    {
                        if (pf.FindPath(mi.x, mi.z, x, z) != null)
                        {
                            selected.GetComponent<Highlight>().PlaceFlag(x, grid.gridArray[x,z].height, z, flagPrefab);
                        }
                    }
                }
            }
        }

        foreach (var unitClone in units)
        {
            if(unitClone.GetComponent<Unit>() && unitClone.GetComponent<MoveQueue>())
            {
                MoveQueue mq = unitClone.GetComponent<MoveQueue>();
                if (unitClone.GetComponent<State>().GetState() == Constants.BUSY)
                {
                    return;
                }
                if (mq.q.Count > 0)
                {
                    return;
                }
                if (unitClone.GetComponent<Highlight>() == null)
                {
                    return;
                }
                if (unitClone.GetComponent<Highlight>().rallyPoint == null)
                {
                    return;
                }
                int x = unitClone.GetComponent<Highlight>().rallyPoint.x;
                int z = unitClone.GetComponent<Highlight>().rallyPoint.z;
                MetaInformation mi = unitClone.GetComponent<MetaInformation>();
                if (x == mi.x && z == mi.z)
                {
                    unitClone.GetComponent<Highlight>().RemoveFlag();
                }
                List<PathNode> path = pf.FindPath(mi.x, mi.z, x, z);

                if (path == null)
                {
                    Debug.Log("No route to the target!");
                }

                foreach (PathNode node in path)
                {
                    unitClone.GetComponent<MoveQueue>().AddMove(new Vector3(node.x, grid.gridArray[node.x, node.z].height, node.z));
                }

            }
        }
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

    public void SelectBridgeTile(GameObject tile){
        if(selectedBridgeTile == tile){
            return; 
        }
        if(selectedBridgeTile == null ){
            selectedBridgeTile = tile; 
            Debug.Log("First bridge tile");
        } else{ // there is a selected bridge tile different than this tile 
            Debug.Log("Second bridge tile");
            placeBridge(selectedBridgeTile, tile);
            selectedBridgeTile = null; 
            inBridgeMode = false; 
            mainCamera.backgroundColor = gameBackgroundColor;
        }
    }

    public void placeBridge(GameObject src, GameObject dst){
        int srcX, srcZ;
        int dstX, dstZ; 
        grid.GetXY(src.transform.position, out srcX, out srcZ); 
        grid.GetXY(dst.transform.position, out dstX, out dstZ); 
        Debug.Log(src.tag); 
        Debug.Log("source position: " + srcX + ", " + srcZ);
        if(src.tag != dst.tag || !(srcX==dstX || srcZ==dstZ)){
            Debug.Log("Can't place bridge"); 
            selectedBridgeTile = null; 
            return; 
        }

        int bridgeX, bridgeZ; 
        if(srcX == dstX && Math.Abs(srcZ-dstZ) == 2){
            bridgeX = srcX;
            bridgeZ = (srcZ + dstZ) / 2; 
        }
        else if (srcZ == dstZ && Math.Abs(srcX-dstX) == 2){
            bridgeZ = srcZ;
            bridgeX = (srcX + dstX) / 2; 
        }
        else {
            Debug.Log("Can't place bridge"); 
            selectedBridgeTile = null; 
            return; 
        }
        
        float bridgeHeight; 
        if(src.tag == "E1"){
           bridgeHeight = bridgeE1_y; 
           grid.gridArray[bridgeX, bridgeZ].height = 0.25f;
        } else if(src.tag == "E2"){
            bridgeHeight = bridgeE2_y; 
            grid.gridArray[bridgeX, bridgeZ].height = 0.75f;
        } else if(src.tag == "E3"){
            bridgeHeight = bridgeE3_y;
            grid.gridArray[bridgeX, bridgeZ].height = 1.25f;
        } else {
            Debug.Log("Can't place bridge"); 
            selectedBridgeTile = null; 
            return; 
        }
           GameObject bridge = UnityEngine.Object.Instantiate(bridgeE1_Left, new Vector3(bridgeX, bridgeHeight, bridgeZ), Quaternion.identity);
           grid.gridArray[bridgeX,bridgeZ].AddUnit(bridgeX, bridgeZ, bridge); 
           // Make node now walkable too, also the height will depend on the bridge type
           grid.pathNodes[bridgeX, bridgeZ].isWalkable = true;
           grid.pathNodes[bridgeX, bridgeZ].tag = src.tag;
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

    public void EnterBridgeMode(){
        if(Input.GetKeyDown("b")){
            if(inBridgeMode){
                selectedBridgeTile = null; 
                inBridgeMode = false; 
                 mainCamera.backgroundColor = gameBackgroundColor;
            } else {
                inBridgeMode = true; 
                 mainCamera.backgroundColor = bridgeBackgroundColor;
            }
        }
    }

}
