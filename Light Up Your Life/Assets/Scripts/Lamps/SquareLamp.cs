using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareLamp : Lamp //IGONORE THIS CLASS FOR NOW
{
    public override void CheckTiles()
    {
        if (tileOn == null)
            return;

        float scaledDistance = LightDistance * TileSize;
        
        //TODO: use tile coords from tile once that is implemented
        Vector2 tileCoord = Vector2.zero;

        for (float i = tileCoord.x - LightDistance; i < tileCoord.x + LightDistance; i++)
        {
            for (float j = tileCoord.y - LightDistance; j < tileCoord.y + LightDistance; j++)
            {
                //Tiles at each of these locations are lit if a line can be drawn to them
            }
        }
    }
}
