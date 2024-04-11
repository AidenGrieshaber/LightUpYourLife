using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Windows;
using Unity.VisualScripting;
using System.Collections.Generic;

/// <summary>
/// Created by Chris LoSardo
/// 2/3/2024
/// Manages the creation of the placement grid for each level
/// </summary>
public class GridManager : MonoBehaviour
{
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;

    //Reference to the tile prefab that makes up the grid
    [SerializeField] private Tile tileObject;

    [SerializeField] private Transform mainCamera;
    [SerializeField] private GameObject gridTiles;

    [SerializeField] public TMP_Text lightCoverage;
    [SerializeField] public TMP_Text finalScore;
    [SerializeField] private double lights;
    [SerializeField] private int stars = 0;
    [SerializeField] private int lampLit = 0;

    [SerializeField] public Image ProgressMask;

    [SerializeField] public GameObject B3Star1;
    [SerializeField] public GameObject B3Star2;
    [SerializeField] public GameObject B3Star3;
    [SerializeField] public GameObject W3Star1;
    [SerializeField] public GameObject W3Star2;
    [SerializeField] public GameObject W3Star3;

    [SerializeField] public GameObject B2Star1;
    [SerializeField] public GameObject B2Star2;
    [SerializeField] public GameObject W2Star1;
    [SerializeField] public GameObject W2Star2;

    [SerializeField] public GameObject B1Star;
    [SerializeField] public GameObject W1Star;

    [SerializeField] public GameObject EndBStar1;
    [SerializeField] public GameObject EndBStar2;
    [SerializeField] public GameObject EndBStar3;
    [SerializeField] public GameObject EndWStar1;
    [SerializeField] public GameObject EndWStar2;
    [SerializeField] public GameObject EndWStar3;

    [SerializeField] public List<Lamp> LampList;

    [SerializeField] public GameObject EndScreen;
    [SerializeField] public GameObject NextLevel;
    [SerializeField] public GameObject LevelComplete;
    [SerializeField] public GameObject LevelFail;

    private int currentLevel = 0;

    public double numTiles;

    private Tile[,] tileArray;

    private string filePath;

    public Tile[,] TileArray
    {
        get
        {
            return tileArray;
        }
    }

    void Start()
    {
        tileArray = new Tile[gridWidth, gridHeight];
        //GenerateDefaultGrid();


        filePath = Application.dataPath + "/Assets/LevelGen/TestLevel.txt";
        LoadLevel(Singleton.Instance.ID, filePath);


        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                if (tileArray[i, j].TileTypeGet == TileType.Tile)
                {
                    numTiles++;
                }
            }
        }
        //Debug.Log(tileArray.Length);
    }

    private void Update()
    {
        lights = 0;
        stars = 0;
        lampLit = 0;

        for (int i = 0; i < LampList.Count; i++)
        {
            if (LampList[i].state == LampState.Placed)
            {
                lampLit++;
            }
        }

        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                if (tileArray[i, j].IsLit == true)
                {
                    lights += 100 / numTiles;
                }
            }
        }

        ProgressMask.fillAmount = (float)(lights / 100);

        if(lights >= 50)
        {
            stars++;
            B1Star.SetActive(false);
            W1Star.SetActive(true);
        }

        if (lights >= 70)
        {
            stars++;
            B2Star1.SetActive(false);
            B2Star2.SetActive(false);
            W2Star1.SetActive(true);
            W2Star2.SetActive(true);
        }

        if (lights >= 99)
        {
            stars++;
            B3Star1.SetActive(false);
            B3Star2.SetActive(false);
            B3Star3.SetActive(false);
            W3Star1.SetActive(true);
            W3Star2.SetActive(true);
            W3Star3.SetActive(true);
        }

        int light = (int)lights;

        lightCoverage.text = light.ToString() + "%";

        if (lampLit == 6 || stars == 3) //TODO: we really need to change how this is done
        {
            EndScreen.SetActive(true);

            finalScore.text = "Light Coverage: " + light.ToString() + "%";

            if (stars > 0)
            {
                //Set progress data to the next level, and save the star count
                Singleton.Instance.SetStars(stars, currentLevel);
                if (Singleton.Instance.ID + 1 > Singleton.Instance.LevelAt)
                    Singleton.Instance.LevelAt = Singleton.Instance.ID + 1;
                DataPersistenceManager.Instance.SaveGame();
            }

            if (stars == 1)
            {
                EndBStar1.SetActive(false);
                EndWStar1.SetActive(true);
                return;
            }
            else if (stars == 2)
            {
                EndBStar1.SetActive(false);
                EndBStar2.SetActive(false);
                EndWStar1.SetActive(true);
                EndWStar2.SetActive(true);
                return;
            }
            else if (stars == 3)
            {
                EndBStar1.SetActive(false);
                EndBStar2.SetActive(false);
                EndBStar3.SetActive(false);
                EndWStar1.SetActive(true);
                EndWStar2.SetActive(true);
                EndWStar3.SetActive(true);
                return;
            }
            else
            {
                LevelComplete.SetActive(false);
                LevelFail.SetActive(true);
                NextLevel.SetActive(false);
                return;
            }
        }
    }

    //Instantiates the tile prefab to create the grid
    void GenerateDefaultGrid()
    {
        //Determines the width of the grid
        for (int i = 0; i < gridWidth; i++)
        {
            //Determines the height of the grid
            for (int j = 0; j < gridHeight; j++)
            {
                Tile newTile = Instantiate(tileObject, new Vector3(i, j), Quaternion.identity, gridTiles.transform);
                //Sets the name of each tile in the inspector
                newTile.name = "t" + i + " " + j;

                //TEMP DELETE THIS
                if ((i == 2 && j == 2) || (i == 5 && j == 5))
                {
                    newTile.SetTileType(newTile, 'o');
                }
                else
                {
                    newTile.SetTileType(newTile, '-');
                }


                //This bool determines whether or not this tile is an even or odd tile in order
                //To change its color
                bool isOffset = (i % 2 == 0 && j % 2 != 0) || (i % 2 != 0 && j % 2 == 0);
                newTile.ChangeColor(isOffset);


                tileArray[i, j] = newTile;

            }
        }
        //Sets the position of the camera to above the created grid
        mainCamera.transform.position = new Vector3((float)gridWidth / 2 - 0.5f, (float)gridHeight / 2 - 0.5f, -10);
    }

    public void UnHighlightTiles()
    {
        for (int i = 0; i < gridWidth; i++)
        {
            //Determines the height of the grid
            for (int j = 0; j < gridHeight; j++)
            {
                //This bool determines whether or not this tile is an even or odd tile in order
                //To change its color
                bool isOffset = (i + j) % 2 == 1; //my way of checking this is more epic than Chris's

                if (!tileArray[i, j].IsLit)
                    tileArray[i, j].ChangeColor(isOffset);
            }
        }
    }

    //Uses a specified level number to load a specific level layout from a file
    public void LoadLevel(int levelNum, string filePath)
    {
        //Debug.Log("LoadLevel GridManager: " + levelNum);
        currentLevel = levelNum; //used for setting stars later

        //Index of each level
        string[] levelIndex = System.IO.File.ReadAllLines(filePath);
        //Array of tiles that make up the specified level number
        string[] levelRows = levelIndex[levelNum - 1].Split(',');

        //Determines the width of the grid
        for (int i = 0; i < levelRows.Length; i++)
        {
            //Determines the height of the grid
            for (int j = 0; j < levelRows[0].Length; j++)
            {
                Tile newTile = Instantiate(tileObject, new Vector3(i, j), Quaternion.identity, gridTiles.transform);
                //Sets the type of the tile (Tile, Obstacle, etc.) based on the character stored in levelRows[i][j]
                newTile.SetTileType(newTile, levelRows[i][j]);
                //Sets the name of each tile in the inspector
                newTile.name = "t" + i + " " + j + " " + newTile.TileTypeGet;


                //This bool determines whether or not this tile is an even or odd tile in order
                //To change its color
                bool isOffset = (i % 2 == 0 && j % 2 != 0) || (i % 2 != 0 && j % 2 == 0);
                newTile.ChangeColor(isOffset);


                tileArray[i, j] = newTile;
            }
        }

        //Sets the position of the camera to above the created grid
        mainCamera.transform.position = new Vector3((float)gridWidth / 2 - 0.5f, (float)gridHeight / 2 - 0.5f, -10);

    }
    public void TriggerNextLevel()
    {
        if (Singleton.Instance.ID >= 15)
        {
            Singleton.Instance.ID = 1;
        }
        else
        {
            Singleton.Instance.ID ++;
        }
        LoadLevel(Singleton.Instance.ID, filePath);
    }
}
