using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private int stars;
    [SerializeField] private GameObject litStarPrefab;
    [SerializeField] private GameObject unlitLightPrefab;

    public string connectedScene;

    // Start is called before the first frame update
    void Start()
    {
        //Generate the stars
        for (int i = 0; i < 3; i++)
        {
            if (i < stars)
            {
                GameObject star = Instantiate(litStarPrefab);
                star.transform.position = transform.position + new Vector3(.5f * (i-1),-.5f,0);
            }
            else
            {
                GameObject star = Instantiate(unlitLightPrefab);
                star.transform.position = transform.position + new Vector3(.5f * (i - 1), -.5f, 0);
            }
        }
    }

    private void LoadGivenScene()
    {
        SceneManager.LoadScene(connectedScene);
    }
}
