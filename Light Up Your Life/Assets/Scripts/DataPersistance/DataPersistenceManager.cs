using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    private GameData gameData;

    public static DataPersistenceManager Instance {get; private set;}

    private List<IDataPersistance> dataPersistanceObjects;

    private void Awake()
    {
        if (Instance == null)
        {
            Debug.LogError("More than one data persistance manager in scene");
        }
        Instance = this;
    }

    private void Start()
    {
        this.dataPersistanceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        //TODO: load data

        if (this.gameData == null)
        {
            Debug.Log("No data found. Using default data");
            NewGame();
        }

        //TODO: move loaded data
        foreach (IDataPersistance data in dataPersistanceObjects)
        {
            data.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        //TODO: pass data to be updated
        foreach( IDataPersistance data in dataPersistanceObjects) 
        { 
            data.SaveData(ref gameData);
        }

        Debug.Log("Saved data: LevelAt is = " + gameData.levelAt);
        //TODO: save to file
    }

    private void OnApplicationQuit()
    {
        //TODO: check if we really want this
        SaveGame();
    }

    private List<IDataPersistance> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistance>();

        return new List<IDataPersistance>(dataPersistanceObjects);
    }
}
