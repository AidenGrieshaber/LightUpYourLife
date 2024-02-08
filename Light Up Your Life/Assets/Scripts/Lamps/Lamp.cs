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

    //The tile this lamp is on
    public Tile tileOn = null;

    //The dimension of a tile for distance calculations
    public float TileSize = 1;

    //Whether or not the lamp is on, may be useful later for visuals
    public bool isLit = false;

    //Point for the lamp to follow while it is held, likely the mouse or touch input
    public Vector3 anchorpoint;

    private LampState state;

    [SerializeField]//serialize for now until lampmanager implemented
    private GridManager gridManager;

    //position of this lamp on the hotbar
    public Vector3 HotbarPosition;

    // Start is called before the first frame update
    void Start()
    {
        HotbarPosition = transform.position;

        state = LampState.Hotbar;
        anchorpoint = Vector3.zero; //This shoule not still be zero by the time it is used
        gridManager = lampManager.GetGridManager();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == LampState.Held)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            anchorpoint = new Vector3(mouseWorld.x, mouseWorld.y, transform.position.z); //Don't change z pos
            gameObject.transform.position = anchorpoint;
        }
    }

    private void OnMouseDown()
    {
        if (state == LampState.Hotbar)
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
                SnapToGrid(nearest);

            CheckTiles();
        }
    }

    private Tile FindNearestTile()
    {
        //Find nearest tile
        Tile nearest = gridManager.TileArray[0, 0];
        float smallestSquareDistance = float.MaxValue; //using square magnitude for performance
        foreach (Tile t in gridManager.TileArray)
        {
            float distance = (t.transform.position - transform.position).sqrMagnitude;
            if (distance < smallestSquareDistance)
            {
                smallestSquareDistance = distance;
                nearest = t;
            }
        }

        //Check if nearest tile is near enough to be considered snapable
        float squareDiagonal = Mathf.Pow(TileSize / 2, 2) + Mathf.Pow(TileSize / 2, 2);
        if (smallestSquareDistance > 1 + squareDiagonal)// the 1+ is to account for difference in z pos
        {
            //return to hotbar
            state = LampState.Hotbar;
            transform.position = HotbarPosition;
            Debug.Log(squareDiagonal + " vs " + smallestSquareDistance);
            return null;
        }

        return nearest;
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
