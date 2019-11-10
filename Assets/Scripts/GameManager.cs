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

    //UI (gameplay) vars
    public MapGrid<GridContainer> grid;
    public GameObject small;
    public GameObject medium;
    public GameObject large;
    public GameObject mediumVillage;
    public GameObject mediumForest;
    public GameObject water;
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

    void GenerateGrid() //implemented gridTest's grid generation into GameManager
    {
        grid = new MapGrid<GridContainer>(10, 10, 1, new Vector3(0, 0, 0), (MapGrid<GridContainer> g, int x, int y) => new GridContainer(g, x, y));

        for (int x = 0; x < grid.gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < grid.gridArray.GetLength(1); y++)
            {
                grid.gridArray[x, y].AddUnit(water);
            }
        }

        grid.gridArray[4, 8].AddUnit(mediumForest);
        grid.gridArray[5, 8].AddUnit(medium);
        grid.gridArray[5, 7].AddUnit(medium);
        grid.gridArray[4, 7].AddUnit(medium);
        grid.gridArray[7, 5].AddUnit(mediumVillage);
        grid.gridArray[2, 5].AddUnit(mediumForest);
        grid.gridArray[8, 3].AddUnit(medium);
        grid.gridArray[3, 2].AddUnit(medium);
        grid.gridArray[2, 2].AddUnit(medium);

        grid.gridArray[6, 7].AddUnit(large);
        grid.gridArray[1, 5].AddUnit(large);
        grid.gridArray[1, 4].AddUnit(large);
        grid.gridArray[3, 3].AddUnit(large);

        grid.gridArray[3, 6].AddUnit(small);
        grid.gridArray[4, 6].AddUnit(small);
        grid.gridArray[4, 3].AddUnit(small);
        grid.gridArray[4, 2].AddUnit(small);
        grid.gridArray[5, 2].AddUnit(small);
        grid.gridArray[7, 3].AddUnit(small);


        /*for (int x = 0; x < grid.gridArray.GetLength(0); x++)
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
        }*/
    }

    void TestGenerateBridge() //implemented gridTest's grid generation into GameManager
    {
        grid = new MapGrid<GridContainer>(10, 10, 1, new Vector3(0, 0, 0), (MapGrid<GridContainer> g, int x, int y) => new GridContainer(g, x, y));

        for (int x = 0; x < grid.gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < grid.gridArray.GetLength(1); y++)
            {
                grid.gridArray[x, y].AddUnit(water);
            }
        }
        grid.gridArray[6, 5].AddUnit(bridgeE2_Left);
        grid.gridArray[5, 5].AddUnit(bridgeE2Corner_Down);
        grid.gridArray[5, 6].AddUnit(bridgeE2_Right);
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
}
