using System;
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

    private List<int> levelStars;

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
            Destroy(gameObject);
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

    public void SetStars(int count, int level)
    {
        if (levelStars.Count < level)//add a new entry if this level is not on the list yet (it is next new level)
        {
            levelStars.Add(count);
        }
        else //Replace old entry if better stars
        {
            if (levelStars[level-1] < count)
                levelStars[level-1] = count;
        }
    }

    public int GetStars(int level)//get stars for given level, default 0
    {
        if (levelStars.Count >= level)
        {
            Console.Out.WriteLine(level);
            return levelStars[level-1];
        }
        return 0;
    }

    public void LoadData(GameData data)
    {
        this.levelAt = data.levelAt;
        this.levelStars = data.levelStars;
    }

    public void SaveData(ref GameData data)
    {
        data.levelAt = this.levelAt;
        data.levelStars = this.levelStars;
    }
}
