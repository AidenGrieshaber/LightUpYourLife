using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{

    [SerializeField] TMP_Dropdown dropdownX;
    [SerializeField] TMP_Dropdown dropdownY;

    [SerializeField] Button tileButtonLarge;
    [SerializeField] Button tileButtonSmall;
    [SerializeField] Button generateGridButton;

    [SerializeField] private GameObject mapGrid;

    [SerializeField] private Transform mainCamera;

    [SerializeField] private GameObject currentPage;


    private string filePath;
    private string[] levelData;
    private List<string> levelDetails;
    private List<string> levelRows;
    //private List<string> levelLamps;
    //private List<int> LevelStars;


    [SerializeField] private Button lArrow;
    [SerializeField] private Button rArrow;


    private List<Button> mapButtons = new List<Button>();
    private float buttonSize = 40;


    // Start is called before the first frame update
    void Start()
    {
        filePath = Application.dataPath + "/Assets/LevelGen/TestLevel.txt";
        levelDetails = new List<string>();
        levelRows = new List<string>();

        //lampList = new List<Lamp>();

        LoadLevelFromFile(filePath);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateGrid()
    {
        if (mapButtons.Count != 0)
        {


            for (int k = 0; k < mapButtons.Count; k++)
            {
                Debug.Log(mapButtons[k].name);
                Destroy(mapButtons[k].gameObject);
            }

            mapButtons.Clear();
        }


        //Debug.Log(dropdownX.options[dropdownX.value].text + ", " + dropdownY.options[dropdownY.value].text);

        //Debug.Log(dropdownX.value);
        //Determines the width of the grid
        for (int i = 0; i < int.Parse(dropdownX.options[dropdownX.value].text); i++)
        {
            //Determines the height of the grid
            for (int j = 0; j < int.Parse(dropdownY.options[dropdownY.value].text); j++)
            {
                float posX;
                float posY;

                Button newButton;

                buttonSize = 50;
                posX = j * buttonSize;
                posY = i * -buttonSize;
                newButton = Instantiate(tileButtonLarge, new Vector3(posX, posY), Quaternion.identity, mapGrid.transform);

                //if (int.Parse(dropdownX.options[dropdownX.value].text) >= 12 || int.Parse(dropdownY.options[dropdownY.value].text) >=14)
                //{
                //    buttonSize = 30;
                //    posX = j * buttonSize;
                //    posY = i * -buttonSize;
                //    newButton = Instantiate(tileButtonSmall, new Vector3(posX, posY), Quaternion.identity, gridButtons.transform);
                //}
                //else
                //{
                //    buttonSize = 40;
                //    posX = j * buttonSize;
                //    posY = i * -buttonSize;
                //    newButton = Instantiate(tileButtonLarge, new Vector3(posX, posY), Quaternion.identity, gridButtons.transform);
                //}

                //Sets the name of each tile in the inspector
                newButton.name = "t" + i + " " + j;



                newButton.transform.position = new Vector2(posX + mapGrid.transform.position.x, posY + mapGrid.transform.position.y);

                mapButtons.Add(newButton);

            }
        }

        Debug.Log(mapButtons.Count);
        //generateGridButton.enabled = false;
    }

    public void SaveLevel()
    { 
    
    }

    public void PrintFile()
    { 
    
    }

    public void LoadLevelFromFile(string filePath)
    {
        string[] levelIndex = System.IO.File.ReadAllLines(filePath);

        //Debug.Log(int.Parse(currentPage.GetComponent<TMP_InputField>().text));

        levelData = levelIndex[int.Parse(currentPage.GetComponent<TMP_InputField>().text) - 1].Split(',');

        for (int i = 0; i < levelData.Length; i++)
        {
            if (i <= 1)
            {
                levelDetails.Add(levelData[i]);

                //Debug.Log("LevelDetails " + i + ": " + levelDetails[i]);
            }
            else
            {
                levelRows.Add(levelData[i]);
                //Debug.Log("levelRows " + (i - 2) + ": " + levelRows[i - 2]);

            }
        }



        if (mapButtons.Count != 0)
        {


            for (int k = 0; k < mapButtons.Count; k++)
            {
                //Debug.Log(mapButtons[k].name);
                Destroy(mapButtons[k].gameObject);
            }

            mapButtons.Clear();
        }

        for (int i = 0; i < levelRows.Count; i++)
        {
            //Determines the height of the grid
            for (int j = 0; j < levelRows[0].Length; j++)
            {
                float posX;
                float posY;

                Button newButton;

                buttonSize = 50;
                posX = j * buttonSize;
                posY = i * -buttonSize;
                newButton = Instantiate(tileButtonLarge, new Vector3(posX, posY), Quaternion.identity, mapGrid.transform);

                //Sets the name of each tile in the inspector
                newButton.name = "t" + i + " " + j;



                newButton.transform.position = new Vector2(posX + mapGrid.transform.position.x, posY + mapGrid.transform.position.y);

                mapButtons.Add(newButton);

                //Debug.Log(levelRows[i][j]);
                if (levelRows[i][j] == 'o')
                {
                    Debug.Log(newButton.GetComponent<ButtonScript>().ButtonStateGetSet);
                    newButton.GetComponent<ButtonScript>().ButtonStateGetSet = ButtonState.Obstacle;
                    Debug.Log(newButton.GetComponent<ButtonScript>().ButtonStateGetSet);

                    newButton.GetComponent<ButtonScript>().buttonImage.color = Color.red;
                    newButton.GetComponent<ButtonScript>().SetColor();
                }

            }
        }
    }
}
