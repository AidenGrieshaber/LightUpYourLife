using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
    void Start()
    {
        GenerateGrid();
    }

    //Instantiates the tile prefab to create the grid
    void GenerateGrid()
    {
        //Determines the width of the grid
        for (int i = 0; i< gridWidth; i++)
        {
            //Determines the height of the grid
            for (int j=0; j<gridHeight; j++) {
                Tile newTile = Instantiate(tileObject, new Vector3(i,j), Quaternion.identity);
                //Sets the name of each tile in the inspector
                newTile.name = "t" + i + " " + j;

                //This bool determines whether or not this tile is an even or odd tile in order
                //To change its color
                bool isOffset = (i % 2 == 0 && j % 2 != 0) || (i % 2 != 0 && j % 2 == 0);
                newTile.ChangeColor(isOffset);
            }
        }
        //Sets the position of the camera to above the created grid
        mainCamera.transform.position = new Vector3((float)gridWidth/2 -0.5f, (float)gridHeight/2 - 0.5f, -10);
    }
}
