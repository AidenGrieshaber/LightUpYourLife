using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int levelAt;
    public List<int> stars;

    public GameData()
    {
        this.levelAt = 1;
        stars = new List<int>();
    }
}
