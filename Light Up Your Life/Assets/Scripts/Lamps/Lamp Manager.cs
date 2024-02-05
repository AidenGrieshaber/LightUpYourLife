using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampManager : MonoBehaviour
{
    [SerializeField]
    private List<Lamp> lamps;

    // Start is called before the first frame update
    void Start()
    {
        lamps = new List<Lamp>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AllLampsCheckTiles()
    {
        foreach (Lamp lamp in lamps)
            lamp.CheckTiles();
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
