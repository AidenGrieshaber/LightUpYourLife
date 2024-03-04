using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private int stars;
    [SerializeField] private SpriteRenderer star1;
    [SerializeField] private SpriteRenderer star2;
    [SerializeField] private SpriteRenderer star3;
    [SerializeField] private TextMeshProUGUI number;
    [SerializeField] private int levelID;

    private List<SpriteRenderer> starList;

    public int LevelID
    {
        get { return levelID; }
        set { levelID = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        starList = new List<SpriteRenderer>() { star1, star2, star3};
        LoadLevelSelect();
    }

    public void LoadLevelSelect()
    {
        number.text = "" + levelID;
        for (int i = 0; i < 3; i++)
        {
            if (i < stars)
            {
                starList[i].color = Color.white;
            }
            else
            {
                starList[i].color = new Color32(0x66, 0x66, 0x66, 0xFF);
            }
        }
    }

    public void LoadGivenScene()
    {
        Debug.Log("clicked");
        Singleton.Instance.SetID(levelID);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
