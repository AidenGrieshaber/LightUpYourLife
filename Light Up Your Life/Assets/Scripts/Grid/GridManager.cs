using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Windows;

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
    [SerializeField] private double lights;

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
        LoadLevel(1, filePath);

        //Debug.Log(tileArray.Length);
    }

    private void Update()
    {
        lights = 0;

        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                if (tileArray[i, j].IsLit == true)
                {
                    lights += 100.00 / (gridWidth * gridHeight);
                }
            }
        }

        lightCoverage.text = "Light Coverage: " + lights.ToString() + "%";
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
}
