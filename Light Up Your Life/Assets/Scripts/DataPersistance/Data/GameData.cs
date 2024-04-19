using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int levelAt;
    public List<int> levelStars;

    public GameData()
    {
        this.levelAt = 1;
        levelStars = new List<int>();
    }
}
