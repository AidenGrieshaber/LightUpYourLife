using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    [SerializeField]
    private LampManager lampManager;
    [SerializeField]
    protected List<Sprite> spriteSheet;

    //The tile this lamp is on
    public Tile tileOn = null;

    //The dimension of a tile for distance calculations
    public float TileSize = 1;

    //Whether or not the lamp is on, may be useful later for visuals
    public bool isLit = false;

    //Point for the lamp to follow while it is held, likely the mouse or touch input
    public Vector3 anchorpoint;

    protected LampState state;

    [SerializeField]//serialize for now until lampmanager implemented
    private GridManager gridManager;

    //position of this lamp on the hotbar
    public Vector3 HotbarPosition;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        HotbarPosition = transform.position;

        state = LampState.Hotbar;
        anchorpoint = Vector3.zero; //This shoule not still be zero by the time it is used
        gridManager = lampManager.GetGridManager();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (state == LampState.Held)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            anchorpoint = new Vector3(mouseWorld.x, mouseWorld.y, transform.position.z); //Don't change z pos
            gameObject.transform.position = anchorpoint;

            HighlightTiles();
        }
    }

    private void OnMouseDown()
    {
        if (state == LampState.Hotbar || state == LampState.None)
        {
            state = LampState.Held;
        }
    }

    private void OnMouseUp()
    {
        if (state == LampState.Held)
        {
            state = LampState.Placed;

            Tile nearest = FindNearestTile();
            if (nearest != null)
            {
                SnapToGrid(nearest);
                gridManager.UnHightlightTiles();
            }
            else
            {
                //return to hotbar
                state = LampState.Hotbar;
                transform.position = HotbarPosition;
            }

            LightTiles();
        }
    }

    protected Tile FindNearestTile()
    {
        //Find nearest tile
        Tile nearest = gridManager.TileArray[0, 0];
        float smallestSquareDistance = float.MaxValue; //using square magnitude for performance
        foreach (Tile t in gridManager.TileArray)
        {
            //account for x and y only
            float distance = (float)Math.Pow(t.transform.position.x - transform.position.x, 2) + (float)Math.Pow(t.transform.position.y - transform.position.y, 2);
            if (distance < smallestSquareDistance)
            {
                smallestSquareDistance = distance;
                nearest = t;
            }
        }

        //Check if nearest tile is near enough to be considered snapable
        float squareDiagonal = Mathf.Pow(TileSize / 2, 2) + Mathf.Pow(TileSize / 2, 2);
        if (smallestSquareDistance > squareDiagonal)// the 1+ is to account for difference in z pos
            return null;

        return nearest;
    }

    /// <summary>
    /// The method used to find unobstructed nearby tiles
    /// </summary>
    protected virtual List<Tile> CheckTiles() { return null; }

    /// <summary>
    /// Highlights tiles in range for being held
    /// </summary>
    public void HighlightTiles()
    {
        gridManager.UnHightlightTiles();
        List<Tile> nearTiles = CheckTiles();
        if (nearTiles != null) //tiles do not need to light if the lamp isn't on the board
        {
            foreach (Tile t in nearTiles)
            {
                if (!t.IsLit)
                {
                    //Change the lit tile visually
                    t.renderer.color = Color.green;
                }
            }
        }
    }

    /// <summary>
    /// Lights up tiles in range
    /// </summary>
    public void LightTiles()
    {
        List<Tile> nearTiles = CheckTiles();
        foreach (Tile t in nearTiles)
        {
            t.IsLit = true;
            //Change the lit tile visually
            t.renderer.color = Color.yellow;
            t.highlight.SetActive(false);
        }
    }

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
