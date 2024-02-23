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

    protected TileType tileType;


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
        if (!IsLit)
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
    /*
    //TESTING ONLY
    private void OnMouseDown()
    {
        //Trigger the check
        CheckLights(this, 2);
    }*/
    private void CheckLights(Tile currentTile, float count)
    {
        //break case
        if (count <= 0 || currentTile == null || currentTile.tileType == TileType.Obstacle)
        {
            return;
        }
        Debug.Log("CheckLights");
        currentTile.IsLit = true;


        //Change the lit tile visually
        currentTile.renderer.color = Color.red;
        currentTile.highlight.SetActive(false);

        Tile tileUp = null;
        Tile tileDown = null;
        Tile tileLeft = null;
        Tile tileRight = null;

        try
        { tileUp = Physics2D.Raycast(currentTile.transform.position, Vector2.up).collider.GetComponent<Tile>(); }
        catch (Exception e) { }

        try
        { tileDown = Physics2D.Raycast(currentTile.transform.position, -Vector2.up).collider.GetComponent<Tile>(); }
        catch (Exception e) { }

        try
        { tileRight = Physics2D.Raycast(currentTile.transform.position, Vector2.right).collider.GetComponent<Tile>(); }
        catch (Exception e) { }

        try
        { tileLeft = Physics2D.Raycast(currentTile.transform.position, -Vector2.right).collider.GetComponent<Tile>(); }
        catch (Exception e) { }

        //Recursively check the surrounding tiles
        CheckLights(tileUp, count - 1);
        CheckLights(tileDown, count - 1);
        CheckLights(tileLeft, count - 1);
        CheckLights(tileRight, count - 1);
    }
    //TESTING ONLY
}
