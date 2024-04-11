using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Windows;
using Unity.VisualScripting;
using System.Collections.Generic;

public class Playfield : MonoBehaviour
{
    [SerializeField] public TMP_Text lightCoverage;
    private double lights;
    private int stars = 0;
    private int lampLit = 0;

    [SerializeField] public Image ProgressMask;

    [SerializeField] public GameObject B3Star1;
    [SerializeField] public GameObject B3Star2;
    [SerializeField] public GameObject B3Star3;
    [SerializeField] public GameObject W3Star1;
    [SerializeField] public GameObject W3Star2;
    [SerializeField] public GameObject W3Star3;

    [SerializeField] public GameObject B2Star1;
    [SerializeField] public GameObject B2Star2;
    [SerializeField] public GameObject W2Star1;
    [SerializeField] public GameObject W2Star2;

    [SerializeField] public GameObject B1Star;
    [SerializeField] public GameObject W1Star;

    [SerializeField] public GameObject EndBStar1;
    [SerializeField] public GameObject EndBStar2;
    [SerializeField] public GameObject EndBStar3;
    [SerializeField] public GameObject EndWStar1;
    [SerializeField] public GameObject EndWStar2;
    [SerializeField] public GameObject EndWStar3;

    [SerializeField] public List<Lamp> LampList;

    [SerializeField] public GameObject EndScreen;
    [SerializeField] public GameObject NextLevel;
    [SerializeField] public GameObject LevelComplete;
    [SerializeField] public GameObject LevelFail;

    private int currentLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lights = 0;
        stars = 0;
        lampLit = 0;

        for (int i = 0; i < LampList.Count; i++)
        {
            if (LampList[i].state == LampState.Placed)
            {
                lampLit++;
            }
        }

        //for (int i = 0; i < gridWidth; i++)
        //{
        //    for (int j = 0; j < gridHeight; j++)
        //    {
        //        if (tileArray[i, j].IsLit == true)
        //        {
        //            lights += 100 / numTiles;
        //        }
        //    }
        //}

        ProgressMask.fillAmount = (float)(lights / 100);

        if (lights >= 50)
        {
            stars++;
            B1Star.SetActive(false);
            W1Star.SetActive(true);
        }

        if (lights >= 70)
        {
            stars++;
            B2Star1.SetActive(false);
            B2Star2.SetActive(false);
            W2Star1.SetActive(true);
            W2Star2.SetActive(true);
        }

        if (lights >= 99)
        {
            stars++;
            B3Star1.SetActive(false);
            B3Star2.SetActive(false);
            B3Star3.SetActive(false);
            W3Star1.SetActive(true);
            W3Star2.SetActive(true);
            W3Star3.SetActive(true);
        }

        int light = (int)lights;

        lightCoverage.text = light.ToString() + "%";

        if (lampLit == 6 || stars == 3) //TODO: we really need to change how this is done
        {
            EndScreen.SetActive(true);
            if (stars > 0)
            {
                //Set progress data to the next level, and save the star count
                Singleton.Instance.SetStars(stars, currentLevel);
                if (Singleton.Instance.ID + 1 > Singleton.Instance.LevelAt)
                    Singleton.Instance.LevelAt = Singleton.Instance.ID + 1;
                DataPersistenceManager.Instance.SaveGame();
            }

            if (stars == 1)
            {
                EndBStar1.SetActive(false);
                EndWStar1.SetActive(true);
                return;
            }
            else if (stars == 2)
            {
                EndBStar1.SetActive(false);
                EndBStar2.SetActive(false);
                EndWStar1.SetActive(true);
                EndWStar2.SetActive(true);
                return;
            }
            else if (stars == 3)
            {
                EndBStar1.SetActive(false);
                EndBStar2.SetActive(false);
                EndBStar3.SetActive(false);
                EndWStar1.SetActive(true);
                EndWStar2.SetActive(true);
                EndWStar3.SetActive(true);
                return;
            }
            else
            {
                LevelComplete.SetActive(false);
                LevelFail.SetActive(true);
                NextLevel.SetActive(false);
                return;
            }
        }
    }
}
