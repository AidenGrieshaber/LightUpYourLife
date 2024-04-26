using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    private int levelWidth;
    private int levelHeight;

    //Lamp and Star Data
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


    //Used for getting the filepath without passing it through a method
    public string FilePathGet
    {
        get
        {
            return filePath;
        }
    }

    //Used for getting the current list of Map Buttons
    public List<Button> MapButtonsGet
    {
        get
        {
            return mapButtons;
        }
    }

    void Start()
    {
        //Creates the LevelData folder in the streaming assets directory
        Directory.CreateDirectory(Application.streamingAssetsPath + "/LevelData/");

        //Assigns the file path for the level data file so that it can be read from and written to
        filePath = Application.streamingAssetsPath + "/LevelData/LevelData.txt";

        ///PSA: These terms are used a lot, her is what they mean:
        ///Details:  The lamp and star information for the current level
        ///Rows:  The actual grid data for the current level
        ///Data (not currently pictured):  The details and rows combined into one file, details always first

        levelDetails = new List<string>();
        levelRows = new List<string>();

        //Loads the first level in the file
        LoadLevelFromFile();
    }


    public void LoadLevelFromFile()
    {
        #region GridGeneration

        //Creates an array (levelIndex) that contains all of the level data from the file
        string[] levelIndex = File.ReadAllLines(FilePathGet);

        //Uses the current page number to determine what the current level is
        //Then it splits the corresponding line of text and assigns the data to the levelData array
        levelData = levelIndex[int.Parse(currentPage.GetComponent<TextMeshProUGUI>().text) - 1].Split(',');

        //If there are buttons making up the grid, each one is destroy and
        //all related lists are cleared to make room for the new buttons
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

        //Splits the level details and rows into two seperate lists,
        //determined by the position of the data in the levelData array
        for (int i = 0; i < levelData.Length; i++)
        {
            //Details always take up the first two indexs of the array
            if (i <= 1)
            {
                levelDetails.Add(levelData[i]);
            }
            else
            {
                levelRows.Add(levelData[i]);
            }
        }

        //Determines default value of width and height, used for saving
        levelWidth = levelRows[0].Length;
        levelHeight = levelRows.Count;

        //Determines the height of the grid
        for (int i = 0; i < levelHeight; i++)
        {
            //Determines the width of the grid
            for (int j = 0; j < levelWidth; j++)
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


                //Positions the button on the mapGrid gameobject
                newButton.transform.position = new Vector2(posX + mapGrid.transform.position.x, posY + mapGrid.transform.position.y);

                mapButtons.Add(newButton);

                //Sets the state of the new button
                if (levelRows[i][j] == 'o')
                {
                    newButton.GetComponent<ButtonScript>().ButtonStateGetSet = ButtonState.Obstacle;
                    newButton.GetComponent<ButtonScript>().buttonImage.color = Color.red;
                }

            }
        }
        #endregion

        #region DataLoading

        //Loads lamp and Star data
        ParseLampData(levelData[0]);

        ParseStarData(levelData[1]);

        #endregion
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

            //Checks the first character of the lamp data (the one that determines lamp type)
            //Then it assigns the values accordingly
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
                    coneDir.GetComponent<TMP_InputField>().text = tempLampData[3].ToString();
                    break;
                default:
                    cCount.GetComponent<TMP_InputField>().text = 3.ToString();
                    cRad.GetComponent<TMP_InputField>().text = 3.ToString();
                    sCount.GetComponent<TMP_InputField>().text = 3.ToString();
                    sRad.GetComponent<TMP_InputField>().text = 1.ToString();
                    Debug.Log("Default LampParse LevelGen");
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

    //Allows the user to manually generate grids
    public void GenerateGrid()
    {
        //If there are buttons making up the grid, each one is destroy and
        //all related lists are cleared to make room for the new buttons
        if (mapButtons.Count != 0)
        {
            for (int k = 0; k < mapButtons.Count; k++)
            {
                Destroy(mapButtons[k].gameObject);
            }

            mapButtons.Clear();
            levelWidth = 0;
            levelHeight = 0;
        }

        //Assigns new values to the level width and height, used for saving
        levelHeight = int.Parse(dropdownY.options[dropdownY.value].text);
        levelWidth = int.Parse(dropdownX.options[dropdownX.value].text);


        //Determines the height of the grid
        for (int i = 0; i < int.Parse(dropdownY.options[dropdownY.value].text); i++)
        {
            //Determines the width of the grid
            for (int j = 0; j < int.Parse(dropdownX.options[dropdownX.value].text); j++)
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
    }

    //Overwrites the file with the current level data
    public void SaveLevel()
    {
        //Creates an array (levelIndex) that contains all of the level data from the file
        string[] levelIndex = File.ReadAllLines(FilePathGet);

        //creates a list containing the current buttons on the grid
        List<Button> currentButtons = MapButtonsGet;
        List<string> tempDetails = new List<string>();
        List<string> tempRows = new List<string>();

        levelData = levelIndex[int.Parse(currentPage.GetComponent<TextMeshProUGUI>().text) - 1].Split(',');

        for (int i = 0; i < levelData.Length; i++)
        {
            //Assigns the levelDetails the same way
            if (i <= 1)
            {
                tempDetails.Add(levelData[i]);
            }
        }
        //But fills tempRows with empty data depending on the height of the current grid
        for (int b = 0; b < levelHeight; b++)
        {
            tempRows.Add("");
        }

        //See bottom comment
        int stringJump = 0;

        Debug.Log(levelWidth);
        Debug.Log(levelHeight);

        //loops through the grid and creates the corresponding data line by line
        for (int j = 0; j < levelHeight; j++)
        {
            //Starts each new line with a blank string
            string currentLine = "";
            for (int k = 0; k < levelWidth; k++)
            {
               //Then adds the corresponding character to the current line depending on the state of the button
                if (currentButtons[k + stringJump].GetComponent<ButtonScript>().ButtonStateGetSet == ButtonState.Obstacle)
                {
                    currentLine += 'o';
                }
                else
                {
                    currentLine += '-';
                }
            }

            //Then assigns the current index of tempRows with the final line
            tempRows[j] = currentLine;

            //Increments stringJump by the amount of buttons in each row
            //This ensures that each line starts with the first button in each row
            stringJump += levelWidth;
        }

        //Assigns the lamp and star data to tempDetails
        tempDetails[0] = writeLampData();
        tempDetails[1] = writeStarData();
        //Then passes them into the filewriter
        WriteFile(tempDetails, tempRows, levelIndex);
    }

    //Reloads the current level, essentially erasing any unsaved changes
    public void ResetLevel()
    {
        #region GridGeneration

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
            }
            else
            {
                levelRows.Add(levelData[i]);
            }
        }

        for (int i = 0; i < levelRows.Count; i++)
        {
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

    //loads the next level when the next page button is pressed
    public void IncrementPage()
    {
        int pageNum = int.Parse(currentPage.GetComponent<TextMeshProUGUI>().text);

        if (pageNum != 15)
        {
            pageNum++;
            currentPage.GetComponent<TextMeshProUGUI>().text = pageNum.ToString();
            LoadLevelFromFile();
        }

    }

    //Loads the last level when the last page button is pressed
    public void DecrementPage()
    {
        int pageNum = int.Parse(currentPage.GetComponent<TextMeshProUGUI>().text);

        if (pageNum != 1)
        {
            pageNum--;
            currentPage.GetComponent<TextMeshProUGUI>().text = pageNum.ToString();
            LoadLevelFromFile();
        }
    }

    public string writeLampData()
    {
        string currentDetails = "";
        string tempString = "";

        if (int.Parse(cCount.GetComponent<TMP_InputField>().text) != 0)
        {
            tempString = "C-" + int.Parse(cCount.GetComponent<TMP_InputField>().text) + "-";

            tempString += int.Parse(cRad.GetComponent<TMP_InputField>().text);

            currentDetails = tempString;
        }


        if (int.Parse(sCount.GetComponent<TMP_InputField>().text) != 0)
        {
            if (currentDetails != "")
            {
                currentDetails += "|";
            }
            tempString = "S-" + int.Parse(sCount.GetComponent<TMP_InputField>().text) + "-";

            tempString += int.Parse(sRad.GetComponent<TMP_InputField>().text);

            currentDetails += tempString;
        }

        if (int.Parse(wCount.GetComponent<TMP_InputField>().text) != 0)
        {
            if (currentDetails != "")
            {
                currentDetails += "|";
            }
            tempString = "W-" + int.Parse(wCount.GetComponent<TMP_InputField>().text) + "-";

            tempString += int.Parse(wRad.GetComponent<TMP_InputField>().text);

            currentDetails += tempString;
        }

        if (int.Parse(coneCount.GetComponent<TMP_InputField>().text) != 0)
        {
            if (currentDetails != "")
            {
                currentDetails += "|";
            }
            tempString = "<-" + int.Parse(coneCount.GetComponent<TMP_InputField>().text) + "-";

            tempString += int.Parse(coneRad.GetComponent<TMP_InputField>().text);

            tempString += "-" + int.Parse(coneDir.GetComponent<TMP_InputField>().text);

            currentDetails += tempString;
        }
        return currentDetails;
    }

    public string writeStarData()
    {
        string currentStars = "";

        string tempString = oneStar.GetComponent<TMP_InputField>().text + "|" + twoStar.GetComponent<TMP_InputField>().text +
            "|" + threeStar.GetComponent<TMP_InputField>().text;


        currentStars = tempString;
        return currentStars;
    }

    //Overwrites the level file
    public void WriteFile(List<string> details, List<string> rows, string[] levelIndex)
    {
        #region FileWriting

        string completeString = "";

        //Recreates the text format for the level data using the passed in variables.
        for (int i = 0; i < details.Count; i++)
        {
            completeString += details[i] + ",";
        }

        for (int j = 0; j < rows.Count; j++)
        {
            completeString += rows[j];

            if (j != rows.Count - 1)
            {
                completeString += ",";
            }
        }

        //Reassigns the current page in the levelIndex with the new leve data
        levelIndex[int.Parse(currentPage.GetComponent<TextMeshProUGUI>().text) - 1] = completeString;
        
        //Gets file
        string fileLocation = FilePathGet;

        //Overwrites every level in the file.
        File.WriteAllText(fileLocation, "");

        //Loops through the level index and adds each line to the file, essentially recreating it
        //This time with the new level
        for (int a = 0; a < levelIndex.Length; a++)
        {
            File.AppendAllText(fileLocation, levelIndex[a] + "\n");
        }
        #endregion

        #region DataLoading

        ParseLampData(levelData[0]);

        ParseStarData(levelData[1]);

        #endregion
    }

}
