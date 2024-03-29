using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField]
    private string fileName;

    [SerializeField]
    private GameData gameData;

    public static DataPersistenceManager Instance {get; private set;}

    private List<IDataPersistance> dataPersistanceObjects;

    private FileDataHandler dataHandler;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one data persistance manager in scene (not game breaking but do take note)");
        }
        Instance = this;
    }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistanceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame() //This can reset the game data
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load();

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

        dataHandler.Save(gameData);
    }

    public void ResetSaveData()
    {
        gameData.levelAt = 1;
        gameData.levelStars = new List<int>();
        dataHandler.Save(gameData);
        dataHandler.Load();
        LoadGame();
    }

    private List<IDataPersistance> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistance>();

        return new List<IDataPersistance>(dataPersistanceObjects);
    }
}
