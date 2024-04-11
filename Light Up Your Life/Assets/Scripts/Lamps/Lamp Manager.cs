using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampManager : MonoBehaviour
{
    private List<Lamp> lamps;
    [SerializeField]
    private GridManager gridManager;
    [SerializeField]
    private GameObject attachment;


    // Start is called before the first frame update
    void Start()
    {
        lamps = new List<Lamp>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Lamp> LampsGet
    {
        get
        {
            return lamps;
        }
    }
    public GameObject AttachmentGet
    {
        get
        {
            return attachment;
        }
    }

    public GridManager GetGridManager
    {
        get
        {
            return gridManager;
        }
    }



    public void AllLampsLightTiles()
    {
        foreach (Lamp lamp in lamps)
            lamp.LightTiles();
    }

    public void AddLamp(Lamp lamp)
    {
        lamps.Add(lamp);
    }

    public void AddLamp(List<Lamp> newLamps)
    {
        foreach (Lamp lamp in newLamps)
            lamps.Add(lamp);
    }

    public void ClearLamps() => lamps.Clear();
}
