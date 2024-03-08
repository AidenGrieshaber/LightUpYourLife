using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField]
    private GameObject attatchment = null;
    [SerializeField]
    private LayerMask tileLayer;

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
    protected GridManager gridManager;

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
            //SnapToGridCollider(FindNearestTile(),GetComponent<BoxCollider2D>());
        }
    }

    private void OnMouseDown()
    {
        if (state == LampState.Hotbar || state == LampState.None)
        {
            state = LampState.Held;

            if (attatchment != null)
                attatchment.SetActive(false);
        }
    }

    private void OnMouseUp()
    {
        if (state == LampState.Held)
        {
            state = LampState.Placed;
            
            Tile nearest = FindNearestTile();
            if (nearest != null && nearest.TileTypeGet != TileType.Obstacle)
            {
                SnapToGrid(nearest);
                gridManager.UnHighlightTiles();
            }
            else
            {
                //return to hotbar
                state = LampState.Hotbar;
                transform.position = HotbarPosition;
                gridManager.UnHighlightTiles();
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
            float distance = (float)Math.Pow(t.transform.position.x - transform.position.x, 2) +
                (float)Math.Pow(t.transform.position.y - transform.position.y, 2);

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
        gridManager.UnHighlightTiles();
        List<Tile> nearTiles = CheckTiles();


        if (nearTiles != null) //tiles do not need to light if the lamp isn't on the board
        {
            foreach (Tile t in nearTiles)
            {
                
                if(!t.IsLit && this.FindNearestTile().TileTypeGet == TileType.Obstacle)
                {
                    t.renderer.color = Color.red;
                }
                else if (!t.IsLit && t.TileTypeGet != TileType.Obstacle)
                {
                    //Change the lit tile visually
                    t.renderer.color = Color.green;
                }
            }
        }

        foreach (Tile t in nearTiles)
        {
            Debug.Log("Checking Tile: " + t);
            CheckShadows(t);
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
            if (t.TileTypeGet != TileType.Obstacle)
            {
                t.IsLit = true;
                //Change the lit tile visually
                t.renderer.color = Color.yellow;
                t.highlight.SetActive(false);
            }
           
        }

        foreach (Tile t in nearTiles)
        {
            Debug.Log("Checking Tile: " + t);
            CheckShadows(t);
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

    public void SnapToGridCollider(Tile nearestTile, BoxCollider2D collider)
    {
        tileOn = nearestTile;

        Vector3 tilePos = nearestTile.gameObject.transform.position;
        Vector3 snapLocation = new Vector3(tilePos.x, tilePos.y, gameObject.transform.position.z); //Copy x and y of tile
        collider.transform.position = snapLocation;
    }

    private void CheckShadows(Tile cTile)
    {
        Vector3 DirToLight;

        if(this.tileOn == null)
        {
            DirToLight = (FindNearestTile().transform.position - cTile.transform.position).normalized;
        }
        else
        {
            DirToLight = (this.GetComponent<BoxCollider2D>().transform.position - cTile.transform.position).normalized;
        }
        
        RaycastHit2D hit = Physics2D.Raycast(cTile.transform.position, DirToLight, LightDistance, ~tileLayer);
        Debug.DrawRay(this.GetComponent<BoxCollider2D>().transform.position, DirToLight, Color.cyan, 1000000, false);
        Debug.Log(hit.collider + " " + cTile + " " + DirToLight);
        if(hit.collider != null && hit.collider.tag != "Lamp")
        {
            cTile.IsLit = false;
            cTile.ChangeColor(true);
            cTile.highlight.SetActive(false);
        }

    }


}
