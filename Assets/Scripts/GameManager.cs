using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
public class GameManager : MonoBehaviour
{
    //UI (menu) vars
    public TextMeshProUGUI bridgesText;
    public GameObject titleScreen;
    public GameObject gameOverScreen;
    //UI (gameplay) vars
    public MapGrid<GridContainer> grid;
    public GameObject small;
    public GameObject medium;
    public GameObject large;
    public GameObject mediumVillage;
    public GameObject mediumForest;
    public GameObject water;

    //public vars
    public bool isGameActive;

    //private vars
    private int bridges;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        checkIfGameOver();
    }

    public void StartGame(int difficulty)
    {
        isGameActive = true;

        bridgesText.gameObject.SetActive(true);
        bridges = 0;
        UpdateBridges(0);

        float spawnRate = 0.5f / difficulty;
        StartCoroutine(Example(spawnRate)); //do you need to do something over time? it needs to be a coroutine

        titleScreen.gameObject.SetActive(false);

        GenerateGrid();
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

    public void GameOver()
    {
        isGameActive = false;
        gameOverScreen.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
