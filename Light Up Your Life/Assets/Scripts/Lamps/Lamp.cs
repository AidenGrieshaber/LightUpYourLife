using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum LampState
{
    None,
    Hotbar,
    Held,
    Placed
}

public class Lamp : MonoBehaviour //Lamp parent master
{
    [SerializeField] //Scale for the range of the light in tiles
    public float LightDistance = 1;

    //The tile this lamp is on
    public Tile tileOn = null;

    //The dimension of a tile for distance calculations
    public float TileSize = 1;

    //Whether or not the lamp is on, may be useful later for visuals
    public bool isLit = false;

    //Point for the lamp to follow while it is held, likely the mouse or touch input
    public Vector3 anchorpoint;

    private LampState state;

    // Start is called before the first frame update
    void Start()
    {
        state = LampState.Hotbar;
        anchorpoint = Vector3.zero; //This shoule not still be zero by the time it is used
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case LampState.None:
                break;
            case LampState.Hotbar:
                break;
            case LampState.Held:
                //anchorpoint = ...
                gameObject.transform.position = anchorpoint;
                break;
            case LampState.Placed:
                break;
        }
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

    public void PickUp()
    {
        if (state != LampState.Placed) //deny picking up placed lamps for now
            state = LampState.Held;
    }

    public void PutDown()
    {
        //if placed back in hotbar
        //state = LampState.Hotbar

        state = LampState.Placed;
        //Search for nearest tile
        //SnapToGrid(nearestTile);
    }
}
