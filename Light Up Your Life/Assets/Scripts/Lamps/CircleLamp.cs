using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CircleLamp : Lamp
{
    private short frame = 0;
    private float counter = 0;
    private static float animationTimer = .18f;

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
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteSheet[frame + 1];
        }
    }

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
        currentTile.renderer.color = Color.yellow;
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
