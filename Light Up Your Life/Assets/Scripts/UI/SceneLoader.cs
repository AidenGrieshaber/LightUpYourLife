using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Scene mainMenu;
    public Scene game;
    public Scene End;

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene(1);
    }

    public void EndScrene()
    {
        SceneManager.LoadScene(2);
    }

    public void ResetGame()//Resets save data, used by the button on main menu
    {
        DataPersistenceManager.Instance.ResetSaveData();
    }
}
