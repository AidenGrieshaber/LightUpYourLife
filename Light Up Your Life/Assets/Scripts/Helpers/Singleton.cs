using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Singleton : MonoBehaviour, IDataPersistance
{
    public static Singleton Instance { get; private set; }

    //[HideInInspector]
    private int id;
    [SerializeField]
    private int levelAt;

    public int ID
    {
        get { return id; }
        set { id = value; }
    }

    public int LevelAt
    {
        get { return levelAt; }
        set { levelAt = value; }
    }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void SetID(int idT)
    {
        //Debug.Log("SetID Before: " + id);
        //Debug.Log("the id: " + idT);
        this.id = idT;
        //Debug.Log("SetID After: " + id);
    }

    public void LoadData(GameData data)
    {
        this.levelAt = data.levelAt;
    }

    public void SaveData(ref GameData data)
    {
        data.levelAt = this.levelAt;
    }
}
