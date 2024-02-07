using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleLamp : Lamp
{
    public override void CheckTiles()
    {
        if (tileOn == null)
            return;

        //float scaledDistance = LightDistance * TileSize;

        CheckLights(tileOn, LightDistance);
    }

    private void CheckLights(Tile currentTile, float count)
    {
        //break case
        if (count <= 0 || currentTile == null)
        {
            return;
        }

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
}
