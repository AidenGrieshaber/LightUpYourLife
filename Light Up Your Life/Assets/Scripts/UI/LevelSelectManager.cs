using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectManager : MonoBehaviour
{
    [SerializeField]
    private int totalLevels;
    [SerializeField]
    private GameObject LevelButton;
    [SerializeField]
    private Camera sceneCamera;
    // Start is called before the first frame update
    void Start()
    {
        GenerateLevelSelect();
    }

    private void GenerateLevelSelect()
    {
        float height = sceneCamera.orthographicSize * 2;
        float width = height * sceneCamera.aspect;
        for (int i = 0; i < totalLevels; i++)
        {
            GameObject current = Instantiate(LevelButton);
            float vertialAlign = (width / 6) * (i % 5 + 1);
            float horizontalAlign = (height / 6) * (i * 3 + 3); 
            current.transform.position = new Vector3(vertialAlign,horizontalAlign,0);
        }
    }
}
