using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleLamp : Lamp
{
    public override void CheckTiles()
    {
        float scaledDistance = LightDistance * TileSize;
        //TODO: how the check works
    }
}
