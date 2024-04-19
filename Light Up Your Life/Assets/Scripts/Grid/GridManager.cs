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

    //Reference to the various lamp prefabs
    [SerializeField] private CircleLamp circleLampObject;
    [SerializeField] private ConeLamp coneLampObject;
    [SerializeField] private SquareLamp squareLampObject;
    [SerializeField] private WallLamp wallLampObject;


    [SerializeField] private Transform mainCamera;
    [SerializeField] private GameObject gridTiles;
    [SerializeField] private GameObject lampDockBackground;

    [SerializeField] private TMP_Text lightCoverage;
    [SerializeField] private double lights;
    [SerializeField] private int stars = 0;
    [SerializeField] private int lampLit = 0;

    [SerializeField] private Image ProgressMask;

    [SerializeField] private GameObject B3Star1;
    [SerializeField] private GameObject B3Star2;
    [SerializeField] private GameObject B3Star3;
    [SerializeField] private GameObject W3Star1;
    [SerializeField] private GameObject W3Star2;
    [SerializeField] private GameObject W3Star3;

    [SerializeField] private GameObject B2Star1;
    [SerializeField] private GameObject B2Star2;
    [SerializeField] private GameObject W2Star1;
    [SerializeField] private GameObject W2Star2;

    [SerializeField] private GameObject B1Star;
    [SerializeField] private GameObject W1Star;

    [SerializeField] private GameObject EndBStar1;
    [SerializeField] private GameObject EndBStar2;
    [SerializeField] private GameObject EndBStar3;
    [SerializeField] private GameObject EndWStar1;
    [SerializeField] private GameObject EndWStar2;
    [SerializeField] private GameObject EndWStar3;

    private List<Lamp> lampList;

    [SerializeField] private GameObject EndScreen;
    [SerializeField] private GameObject NextLevel;
    [SerializeField] private GameObject LevelComplete;
    [SerializeField] private GameObject LevelFail;

    [SerializeField]
    private LampManager lampManager;

    private int currentLevel = 0;

    public double numTiles;

    private Tile[,] tileArray;

    private string filePath;

    private string[] levelData;
    private List<string> levelDetails;
    private List<string> levelRows;
    private List<string> levelLamps;
    private List<int> LevelStars;

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

        int levelNum = Singleton.Instance.ID;

        levelDetails = new List<string>();
        levelRows = new List<string>();

        lampList = new List<Lamp>();

        filePath = Application.dataPath + "/Assets/LevelGen/TestLevel.txt";

        //Index of each level
        string[] levelIndex = System.IO.File.ReadAllLines(filePath);
        currentLevel = levelNum;
        //Array of tiles that make up the specified level number


        levelData = levelIndex[levelNum - 1].Split(',');

        for (int i = 0; i < levelData.Length; i++)
        {
            if (i <= 1)
            {
                levelDetails.Add(levelData[i]);

                Debug.Log("LevelDetails " + i + ": " + levelDetails[i]);
            }
            else
            {
                levelRows.Add(levelData[i]);
                Debug.Log("levelRows " + (i - 2) + ": " + levelRows[i - 2]);

            }
        }

        ParseLampData(levelData[0]);
        lampList = lampManager.LampsGet;
        Debug.Log("LampsList: " + lampList.Count);
        ParseStarData(levelData[1]);
        LoadLevel(levelRows);


        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                if (tileArray[i, j].TileTypeGet == TileType.Tile)
                {
                    tileArray[i, j].posX = i;
                    tileArray[i, j].posY = j;
                    numTiles++;
                }
            }
        }
    }

    private void Update()
    {
        lights = 0;
        stars = 0;
        lampLit = 0;

        for (int i = 0; i < lampList.Count; i++)
        {
            if (lampList[i].state == LampState.Placed)
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

        if (lights >= LevelStars[0])
        {
            stars++;
            B1Star.SetActive(false);
            W1Star.SetActive(true);
        }

        if (lights >= LevelStars[1])
        {
            stars++;
            B2Star1.SetActive(false);
            B2Star2.SetActive(false);
            W2Star1.SetActive(true);
            W2Star2.SetActive(true);
        }

        if (lights >= LevelStars[2])
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

        if (lampLit == lampList.Count || stars == 3) 
        {
            EndScreen.SetActive(true);
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


                Debug.Log(tileArray);

                if (!tileArray[i, j].IsLit)
                    tileArray[i, j].ChangeColor(isOffset);
            }
        }
    }

    //Uses a specified level number to load a specific level layout from a file
    public void LoadLevel(List<string> levelRows)
    {
        ////Debug.Log("LoadLevel GridManager: " + levelNum);
        //currentLevel = levelNum; //used for setting stars later

        string[] levelTiles = new string[levelRows.Count];


        //Array of tiles that make up the specified level number

        for (int l = 0; l < levelRows.Count; l++)
        {
            levelTiles[l] = levelRows[l];
        }



        //Debug.Log("LevelRows[0]: " + levelTiles[0]);

        //Parse Lamp Data

        //Parse Star Data

        //Determines the width of the grid
        for (int i = 0; i < levelTiles.Length; i++)
        {

            //Determines the height of the grid
            for (int j = 0; j < levelTiles[0].Length; j++)
            {



                Tile newTile = Instantiate(tileObject, new Vector3(i, j), Quaternion.identity, gridTiles.transform);
                //Sets the type of the tile (Tile, Obstacle, etc.) based on the character stored in levelRows[i][j]
                newTile.SetTileType(newTile, levelTiles[i][j]);
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
            Singleton.Instance.ID++;
        }
        //LoadLevel(levelRows);
    }


    public void ParseLampData(string levelData0)
    {
        lampManager.ClearLamps();

        levelLamps = new List<string>();

        string[] tempArray = levelData0.Split('|');

        float lampHeight = 6.7f;

        for (int i = 0; i < tempArray.Length; i++)
        {
            levelLamps.Add(tempArray[i]);
        }

        for (int j = 0; j < levelLamps.Count; j++)
        {
            string[] tempLampData = levelLamps[j].Split('-');


            switch (tempLampData[0][0])
            {
                case 'C':
                    for (int a = 0; a < int.Parse(tempLampData[1]); a++)
                    {
                        CircleLamp newLampC = Instantiate(circleLampObject, new Vector3(-3.17f, lampHeight - a), Quaternion.identity); ;

                        newLampC.gridManager = this;
                        newLampC.lightDistance = int.Parse(tempLampData[2]);

                        lampManager.AddLamp(newLampC);
                    }
                    lampHeight -= int.Parse(tempLampData[1]);
                    break;
                case 'S':
                    for (int b = 0; b < int.Parse(tempLampData[1]); b++)
                    {
                        SquareLamp newLampS = Instantiate(squareLampObject, new Vector3(-3.17f, lampHeight - b), Quaternion.identity); ;

                        newLampS.gridManager = this;
                        newLampS.lightDistance = int.Parse(tempLampData[2]);

                        lampManager.AddLamp(newLampS);
                    }
                    lampHeight -= int.Parse(tempLampData[1]);
                    break;
                case 'W':
                    for (int c = 0; c < int.Parse(tempLampData[1]); c++)
                    {
                        WallLamp newLampW = Instantiate(wallLampObject, new Vector3(-3.17f, lampHeight - c), Quaternion.identity); ;

                        newLampW.gridManager = this;
                        newLampW.lightDistance = int.Parse(tempLampData[2]);

                        lampManager.AddLamp(newLampW);
                    }
                    lampHeight -= int.Parse(tempLampData[1]);
                    break;
                case '<':
                    for (int d = 0; d < int.Parse(tempLampData[1]); d++)
                    {
                        ConeLamp newLampCone = Instantiate(coneLampObject, new Vector3(-3.17f, lampHeight - d), Quaternion.identity); ;

                        newLampCone.gridManager = this;
                        newLampCone.lightDistance = int.Parse(tempLampData[2]);

                        lampManager.AddLamp(newLampCone);
                    }
                    lampHeight -= int.Parse(tempLampData[1]);
                    break;
                default:
                    for (int a = 0; a < int.Parse(tempLampData[1]); a++)
                    {
                        CircleLamp newLamp = Instantiate(circleLampObject, new Vector3(-3.17f, lampHeight - a), Quaternion.identity); ;

                        newLamp.gridManager = this;
                        newLamp.lightDistance = int.Parse(tempLampData[2]);

                        lampManager.AddLamp(newLamp);
                    }
                    lampHeight -= int.Parse(tempLampData[1]);
                    break;
            }

        }




    }

    public void ParseStarData(string levelData1)
    {
        LevelStars = new List<int>();

        string[] tempArray = levelData1.Split('|');

        for (int i = 0; i < tempArray.Length; i++)
        {
            LevelStars.Add(int.Parse(tempArray[i]));
        }
    }












    ////Instantiates the tile prefab to create the grid
    //void GenerateDefaultGrid()
    //{
    //    //Determines the width of the grid
    //    for (int i = 0; i < gridWidth; i++)
    //    {
    //        //Determines the height of the grid
    //        for (int j = 0; j < gridHeight; j++)
    //        {
    //            Tile newTile = Instantiate(tileObject, new Vector3(i, j), Quaternion.identity, gridTiles.transform);
    //            //Sets the name of each tile in the inspector
    //            newTile.name = "t" + i + " " + j;

    //            //TEMP DELETE THIS
    //            if ((i == 2 && j == 2) || (i == 5 && j == 5))
    //            {
    //                newTile.SetTileType(newTile, 'o');
    //            }
    //            else
    //            {
    //                newTile.SetTileType(newTile, '-');
    //            }


    //            //This bool determines whether or not this tile is an even or odd tile in order
    //            //To change its color
    //            bool isOffset = (i % 2 == 0 && j % 2 != 0) || (i % 2 != 0 && j % 2 == 0);
    //            newTile.ChangeColor(isOffset);


    //            tileArray[i, j] = newTile;

    //        }
    //    }
    //    //Sets the position of the camera to above the created grid
    //    mainCamera.transform.position = new Vector3((float)gridWidth / 2 - 0.5f, (float)gridHeight / 2 - 0.5f, -10);
    //}
}
