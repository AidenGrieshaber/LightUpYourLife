using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created by Chris LoSardo
/// 2/3/2024
/// Variables and logic for the tiles
/// </summary>
/// 

public enum TileType
{
    Tile,
    Obstacle,
    Table
}
public class Tile : MonoBehaviour
{
    [SerializeField] private Color tileColor;
    [SerializeField] private Color tileColor2;
    [SerializeField] public SpriteRenderer renderer;
    [SerializeField] public GameObject highlight;
    [SerializeField] public bool IsLit;
    public bool permanentLit;

    protected TileType tileType;

    public void Start()
    {
        if (TileTypeGet == TileType.Obstacle)
        {
            int LayermaskInt = LayerMask.NameToLayer("Default");
            gameObject.layer = LayermaskInt;
        }
        
    }

    public TileType TileTypeGet
    {
        get
        {
            return tileType;
        }
    }
    public void ChangeColor(bool isOffset)
    {
        if (tileType == TileType.Tile)
        {
            //Debug.Log("TileType.Tile");
            if (isOffset)
            {
                renderer.color = tileColor2;
            }
            else
            {
                renderer.color = tileColor;
            }
        }
        else if (tileType == TileType.Obstacle)
        {
            renderer.color = Color.black;
        }
        
    }

    private void OnMouseEnter()
    {
        //Only highlight if the tile is not currently lit
        if (!IsLit && TileTypeGet != TileType.Obstacle)
        {
            highlight.SetActive(true);
        }
    }

    public void SetTileType(Tile currentTile, char identifier)
    {
        switch (identifier)
        {
            case '-':
                currentTile.tileType = TileType.Tile;
                break;
            case 'o':
                currentTile.tileType = TileType.Obstacle;
                break;
            default:
                currentTile.tileType = TileType.Tile;
                break;
        }
    }

    private void OnMouseExit()
    {
        highlight.SetActive(false);
    }
}
