using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CircleLamp : Lamp
{
    private short frame = 0;
    private float counter = 0;
    private static float animationTimer = .12f;
    public LayerMask IgnoreLayer;

    protected override void Update()
    {
        //plays the lamp animation, runs through sprites until sprite 6, and then loops last 3
        if (state == LampState.Placed)
        {
            counter += Time.deltaTime;
            if (counter > animationTimer)
            {
                counter = 0;
                frame++;
                if (frame > 5)
                {
                    frame = 3;
                }
            }
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteSheet[frame];
        }
        base.Update();
    }

    protected override List<Tile> CheckTiles()
    {
        if (tileOn == null) //use tileOn if on a tile, calculate nearest otherwise
            return CheckLights(FindNearestTile(), LightDistance);
        else
        {
            Tile nearest = FindNearestTile();
            if (nearest != null)
                return CheckLights(tileOn, LightDistance);
            return null;
        }
    }

    private List<Tile> CheckLights(Tile currentTile, float count)
    {
        //break case
        if (count <= 0 || currentTile == null)
        {
            return new List<Tile>();
        }

        Tile tileUp = null;
        Tile tileDown = null;
        Tile tileLeft = null;
        Tile tileRight = null;

        try
        { tileUp = Physics2D.Raycast(currentTile.transform.position, -Vector2.up, Mathf.Infinity, ~IgnoreLayer).collider.GetComponent<Tile>(); }
        catch (Exception e) {  }

        try
        { tileDown = Physics2D.Raycast(currentTile.transform.position, Vector2.up, Mathf.Infinity, ~IgnoreLayer).collider.GetComponent<Tile>(); }
        catch (Exception e) {  }

        try
        { tileRight = Physics2D.Raycast(currentTile.transform.position, -Vector2.right, Mathf.Infinity, ~IgnoreLayer).collider.GetComponent<Tile>(); }
        catch (Exception e) {  }

        try
        { tileLeft = Physics2D.Raycast(currentTile.transform.position, Vector2.right, Mathf.Infinity, ~IgnoreLayer).collider.GetComponent<Tile>(); }
        catch (Exception e) {  }

        List<Tile> tiles = new List<Tile>();
        tiles.Add(currentTile);

        //Recursively check the surrounding tiles
        tiles.AddRange(CheckLights(tileUp, count - 1));
        tiles.AddRange(CheckLights(tileDown, count - 1));
        tiles.AddRange(CheckLights(tileLeft, count - 1));
        tiles.AddRange(CheckLights(tileRight, count - 1));

        return tiles; 
    }
}
