using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Lamp : MonoBehaviour //Lamp parent master
{
    [SerializeField] //Scale for the range of the light in tiles
    public float LightDistance = 1;

    //The tile this lamp is on
    public Tile tileOn = null;

    //The dimension of a tile for distance calculations
    public float TileSize = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// The method used to check nearby tiles
    /// </summary>
    public virtual void CheckTiles() { }

    /// <summary>
    /// Snap to a grid tile
    /// </summary>
    /// <param name="nearestTile">The tile to snap to</param>
    public void SnapToGrid(Tile nearestTile)
    {
        tileOn = nearestTile;

        Vector3 tilePos = nearestTile.gameObject.transform.position;
        Vector3 snapLocation = new Vector3(tilePos.x, tilePos.y, gameObject.transform.position.z); //Copy x and y of tile
        gameObject.transform.position = snapLocation;
    }
}
