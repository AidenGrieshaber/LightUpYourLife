using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeLamp : Lamp //IGNORE THIS CLASS FOR NOW
{
    //The angle of the light cone
    [SerializeField]
    float angle = 0;

    //The range in both directions around the angle in degrees that is lit
    [SerializeField]
    float range = 30;

    public override void CheckTiles()
    {
        if (tileOn == null)
            return;

        float scaledDistance = LightDistance * TileSize;
        //Use circle check, but only accept if checked points are within a certain range of the angle
    }
}
