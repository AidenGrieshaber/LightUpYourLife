using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{

    //UI
    [SerializeField] TMP_Dropdown dropdownX;
    [SerializeField] TMP_Dropdown dropdownY;
    [SerializeField] private GameObject currentPage;
    [SerializeField] private Button lArrow;
    [SerializeField] private Button rArrow;


    //LevelLoading
    private string filePath;
    private string[] levelData;
    private List<string> levelDetails;
    private List<string> levelRows;
    private List<string> levelLamps;

    [SerializeField] Button tileButtonLarge;
    [SerializeField] Button tileButtonSmall;
    [SerializeField] Button generateGridButton;
    [SerializeField] private GameObject mapGrid;
    private List<Button> mapButtons = new List<Button>();
    private float buttonSize = 40;

    //Lamp and Star Data
    [SerializeField] private LampManager lampManager;

    [SerializeField] private GameObject cCount;
    [SerializeField] private GameObject cRad;
    [SerializeField] private GameObject sCount;
    [SerializeField] private GameObject sRad;
    [SerializeField] private GameObject wCount;
    [SerializeField] private GameObject wRad;
    [SerializeField] private GameObject coneCount;
    [SerializeField] private GameObject coneRad;
    [SerializeField] private GameObject coneDir;


    [SerializeField] private GameObject oneStar;
    [SerializeField] private GameObject twoStar;
    [SerializeField] private GameObject threeStar;



    public string FilePathGet
    {
        get
        {
            return filePath;
        }
    }

    public List<Button> MapButtonsGet
    {
        get
        {
            return mapButtons;
        }
    }

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
                //Debug.Log(mapButtons[k].name);
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
        string[] levelIndex = System.IO.File.ReadAllLines(FilePathGet);

        List<Button> currentButtons = MapButtonsGet;
        List<string> tempDetails = new List<string>();
        List<string> tempRows = new List<string>();

        levelData = levelIndex[int.Parse(currentPage.GetComponent<TextMeshProUGUI>().text) - 1].Split(',');

        //for (int i = 0; i < levelData.Length; i++)
        //{
        //    Debug.Log(levelData[i]);
        //}
        //for (int j = 0; j < currentButtons.Count; j++)
        //{
        //    Debug.Log(currentButtons[j].GetComponent<ButtonScript>().ButtonStateGetSet);
        //}



        for (int i = 0; i < levelData.Length; i++)
        {
            string currentDetails = "";
            if (i <= 1)
            {
                tempDetails.Add(levelData[i]);

                for (int j = 0; j < tempDetails[j].Length; i++)
                {
                    Debug.Log(tempDetails[j]);
                }

                //Debug.Log("LevelDetails " + i + ": " + levelDetails[i]);
            }
            else
            {
                tempRows.Add(levelData[i]);
                //Debug.Log("levelRows " + (i - 2) + ": " + levelRows[i - 2]);

            }
        }


        int stringJump = 0;

        for (int j = 0; j < levelRows.Count; j++)
        {
            string currentLine = "";
            for (int k = 0; k < levelRows[0].Length; k++)
            {
                //Debug.Log(currentButtons[k + stringJump].GetComponent<ButtonScript>().ButtonStateGetSet);

                if (currentButtons[k + stringJump].GetComponent<ButtonScript>().ButtonStateGetSet == ButtonState.Obstacle)
                {
                    currentLine += 'o';
                }
                else 
                {
                    currentLine += '-';
                }
            }
            levelRows[j] = currentLine;
            stringJump += levelData[2].Length;
        }

    }

    public void PrintFile()
    {

    }

    public void LoadLevelFromFile(string filePath)
    {
        #region GridGeneration
        string[] levelIndex = System.IO.File.ReadAllLines(filePath);

        //Debug.Log("CurrentPage:  " + currentPage.GetComponent<TextMeshProUGUI>().text);
        levelData = levelIndex[int.Parse(currentPage.GetComponent<TextMeshProUGUI>().text) - 1].Split(',');

        if (mapButtons.Count != 0)
        {


            for (int k = 0; k < mapButtons.Count; k++)
            {
                Destroy(mapButtons[k].gameObject);
            }

            mapButtons.Clear();
            levelDetails.Clear();
            levelRows.Clear();
        }

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
                    newButton.GetComponent<ButtonScript>().ButtonStateGetSet = ButtonState.Obstacle;
                    newButton.GetComponent<ButtonScript>().buttonImage.color = Color.red;
                }

            }
        }
        #endregion

        #region DataLoading

        ParseLampData(levelData[0]);

        ParseStarData(levelData[1]);

        #endregion
    }


    public void IncrementPage()
    {
        int pageNum = int.Parse(currentPage.GetComponent<TextMeshProUGUI>().text);

        if (pageNum != 15)
        {
            pageNum++;
            currentPage.GetComponent<TextMeshProUGUI>().text = pageNum.ToString();
            LoadLevelFromFile(FilePathGet);
        }

    }

    public void DecrementPage()
    {
        int pageNum = int.Parse(currentPage.GetComponent<TextMeshProUGUI>().text);

        if (pageNum != 1)
        {
            pageNum--;
            currentPage.GetComponent<TextMeshProUGUI>().text = pageNum.ToString();
            LoadLevelFromFile(FilePathGet);
        }
    }


    public void ParseLampData(string levelData0)
    {

        levelLamps = new List<string>();

        string[] tempArray = levelData0.Split('|');

        for (int i = 0; i < tempArray.Length; i++)
        {
            levelLamps.Add(tempArray[i]);
        }

        for (int j = 0; j < levelLamps.Count; j++)
        {
            string[] tempLampData = levelLamps[j].Split('-');


            switch (tempLampData[0][0])
            {
                case 'C':
                    cCount.GetComponent<TMP_InputField>().text = tempLampData[1].ToString();
                    cRad.GetComponent<TMP_InputField>().text = tempLampData[2].ToString();
                    break;
                case 'S':
                    sCount.GetComponent<TMP_InputField>().text = tempLampData[1].ToString();
                    sRad.GetComponent<TMP_InputField>().text = tempLampData[2].ToString();
                    break;
                case 'W':
                    wCount.GetComponent<TMP_InputField>().text = tempLampData[1].ToString();
                    wCount.GetComponent<TMP_InputField>().text = tempLampData[2].ToString();
                    break;
                case '<':
                    coneCount.GetComponent<TMP_InputField>().text = tempLampData[1].ToString();
                    coneRad.GetComponent<TMP_InputField>().text = tempLampData[2].ToString();
                    coneDir.GetComponent<TMP_InputField>().text += tempLampData[3].ToString();
                    break;
                default:
                    cCount.GetComponent<TMP_InputField>().text = 3.ToString();
                    cRad.GetComponent<TMP_InputField>().text = 3.ToString();
                    sCount.GetComponent<TMP_InputField>().text = 3.ToString();
                    sRad.GetComponent<TMP_InputField>().text = 1.ToString();
                    Debug.Log("Defualt LampParse LevelGen");
                    break;
            }

        }




    }

    public void ParseStarData(string levelData1)
    {
        string[] tempArray = levelData1.Split('|');

        oneStar.GetComponent<TMP_InputField>().text = tempArray[0];
        twoStar.GetComponent<TMP_InputField>().text = tempArray[1];
        threeStar.GetComponent<TMP_InputField>().text = tempArray[2];
    }


}
