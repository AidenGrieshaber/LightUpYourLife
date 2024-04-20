using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public enum Direction
{
    Up,
    Down,
    Left,
    Right
}
public class ConeLamp : Lamp //IGNORE THIS CLASS FOR NOW
{
    private short frame = 0;
    private float counter = 0;
    private static float animationTimer = .12f;
    public LayerMask IgnoreLayer;
    public Direction dir;
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
                    switch (dir)
                    {
                        case Direction.Right:
                            if (t.transform.position.x > currentTile.transform.position.x)
                            {
                                tiles.Add(t);
                            }
                            break;

                        case Direction.Left:
                            if (t.transform.position.x < currentTile.transform.position.x)
                            {
                                tiles.Add(t);
                            }
                            break;

                        case Direction.Up:
                            if (t.transform.position.y > currentTile.transform.position.y)
                            {
                                tiles.Add(t);
                            }
                            break;

                        case Direction.Down:
                            if (t.transform.position.y < currentTile.transform.position.y)
                            {
                                tiles.Add(t);
                            }
                            break;
                        default:
                            break;
                    }
                    

                }

                for(int i = 0; i < tiles.Count; i++)
                {
                    if(dir == Direction.Right || dir == Direction.Left)
                    {
                        if (Math.Ceiling(tiles[i].transform.position.y) != Math.Ceiling(currentTile.transform.position.y))
                        {
                            tiles.Remove(tiles[i]);
                            i--;
                        }
                    }

                    else
                    {
                        if (Math.Ceiling(tiles[i].transform.position.x) != Math.Ceiling(currentTile.transform.position.x))
                        {
                            tiles.Remove(tiles[i]);
                            i--;
                        }
                    }
                    
                }

                

                if (t == FindNearestTile())
                {
                    tiles.Add(t);
                }
            }

            int tileCount = tiles.Count;

                switch (dir)
                {
                    case Direction.Right:
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
                        break;

                    case Direction.Left:
                    for (int i = 0; i < tileCount; i++)
                    {
                        for (int j = tileCount; j > i; j--)
                        {
                            try
                            {
                                tiles.Add(gridManager.TileArray[(int)tiles[i].posX, (int)tiles[i].posY + j - i - 1]);
                            }
                            catch (Exception e) { }

                            try
                            {
                                tiles.Add(gridManager.TileArray[(int)tiles[i].posX, (int)tiles[i].posY - j + i + 1]);
                            }
                            catch (Exception e) { }
                        }
                    }
                        break;

                    case Direction.Up:
                    for (int i = 0; i < tileCount; i++)
                    {
                        for (int j = 0; j < i; j++)
                        {
                            try
                            {
                                tiles.Add(gridManager.TileArray[(int)tiles[i].posX + j + 1, (int)tiles[i].posY]);
                            }
                            catch (Exception e) { }

                            try
                            {
                                tiles.Add(gridManager.TileArray[(int)tiles[i].posX - j - 1, (int)tiles[i].posY]);
                            }
                            catch (Exception e) { }
                        }
                    }
                        break;

                    case Direction.Down:
                    for (int i = 0; i < tileCount; i++)
                    {
                        for (int j = tileCount; j > i; j--)
                        {
                            try
                            {
                                tiles.Add(gridManager.TileArray[(int)tiles[i].posX + j - i - 1, (int)tiles[i].posY]);
                            }
                            catch (Exception e) { }

                            try
                            {
                                tiles.Add(gridManager.TileArray[(int)tiles[i].posX - j + i + 1, (int)tiles[i].posY]);
                            }
                            catch (Exception e) { }
                        }
                    }
                        break;

                    default:
                        break;           
                }
        }
        else if (currentTile == null)
        {
            tiles.Add(FindNearestTile());
        }
        return tiles;
    }
}
