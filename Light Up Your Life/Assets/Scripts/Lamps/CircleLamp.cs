using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleLamp : Lamp
{
    public override void CheckTiles()
    {
        if (tileOn == null)
            return;

        float scaledDistance = LightDistance * TileSize;
        //TODO: how the check works
    }
}
