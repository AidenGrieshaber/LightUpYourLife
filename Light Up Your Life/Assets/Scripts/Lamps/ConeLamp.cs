using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ConeLamp : Lamp //IGNORE THIS CLASS FOR NOW
{
    private short frame = 0;
    private float counter = 0;
    private static float animationTimer = .12f;
    public LayerMask IgnoreLayer;

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
        //Vector2 direction = DirectionCheck(dir);
        List<Tile> tiles = new List<Tile>();
        List<Tile> inLine = new List<Tile>();

        if (currentTile == null)
        {
            return tiles;
        }
        if (currentTile.TileTypeGet != TileType.Obstacle)
        {

            foreach (Tile t in gridManager.TileArray)
            {
                float distance = 0;
                try
                {
                    distance = Vector2.Distance(t.transform.position, currentTile.transform.position);
                }
                catch (Exception e) { }
                if (Math.Ceiling(distance) <= LightDistance && distance != 0 && t.TileTypeGet != TileType.Obstacle)
                {
                    if(t.transform.position.x > currentTile.transform.position.x)
                    {
                        tiles.Add(t);
                    }

                }

                for(int i = 0; i < tiles.Count; i++)
                {
                    if (Math.Ceiling(tiles[i].transform.position.y) != Math.Ceiling(currentTile.transform.position.y))
                    {
                        tiles.Remove(tiles[i]);
                        i--;
                    }
                }

                

                if (t == FindNearestTile())
                {
                    tiles.Add(t);
                }
            }

            int tileCount = tiles.Count;
            for (int i = 0; i < tileCount; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    try
                    {
                        tiles.Add(gridManager.TileArray[(int)tiles[i].posX, (int)tiles[i].posY + j + 1]);

                    }
                    catch (Exception e) { }

                    try
                    {
                        tiles.Add(gridManager.TileArray[(int)tiles[i].posX, (int)tiles[i].posY - j - 1]);
                    }
                    catch (Exception e) { }
                    }
            }
        }
        else if (currentTile == null)
        {
            tiles.Add(FindNearestTile());
        }
        return tiles;
    }
}
