using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTile : Tile
{

    protected TileType type;
    private Color tileColor;
    // Start is called before the first frame update
    void Start()
    {
        type = TileType.Obstacle;
        tileColor = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
