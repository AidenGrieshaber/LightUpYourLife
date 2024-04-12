using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallLamp : Lamp //IGNORE THIS CLASS FOR NOW
{
    private short frame = 0;
    private float counter = 0;
    private static float animationTimer = .12f;
    public LayerMask IgnoreLayer;
    private bool onWall;

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

        List<Tile> tiles = new List<Tile>();

        if (currentTile == null)
        {
            return tiles;
        }
        
        if (currentTile.TileTypeGet != TileType.Obstacle)
        {
            List<Tile> walls = new List<Tile>();
            foreach (Tile t in gridManager.TileArray)
            {
                float distance = 0;
                
                try
                {
                    distance = Vector2.Distance(t.transform.position, currentTile.transform.position);
                }
                catch (Exception e) { }
                if (Math.Ceiling(distance) == 1 && t.TileTypeGet != TileType.Obstacle)
                {
                    walls.Add(t);
                }
                
            }

            if(walls.Count < 4)
            {
                onWall = true;
            }
            else
            {
                onWall = false;
            }

            if(!onWall)
            {
                return tiles;
            }

            foreach (Tile t in gridManager.TileArray)
            {
                float distance = 0;
                try
                {
                    distance = Vector2.Distance(t.transform.position, currentTile.transform.position);
                }
                catch (Exception e) { }
                if (Math.Ceiling(distance) < LightDistance && distance != 0 && t.TileTypeGet != TileType.Obstacle)
                {
                    tiles.Add(t);
                }
                if (t == FindNearestTile())
                {
                    tiles.Add(t);
                }
            }
        }
        else if (currentTile == null)
        {
            tiles.Add(FindNearestTile());
        }
        return tiles;
    }

    private void OnMouseUp()
    {
        if (onWall)
        {
            base.OnMouseUp();
        }
        else
        {
            state = LampState.Hotbar;
            transform.position = HotbarPosition;
            gridManager.UnHighlightTiles();
        }
    }
}
