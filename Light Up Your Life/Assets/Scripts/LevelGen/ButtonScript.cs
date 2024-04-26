using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ButtonState
{
    Tile,
    Obstacle,
    Table
}
public class ButtonScript : MonoBehaviour
{
    [SerializeField] private Button gridButton;
    [SerializeField] public Image buttonImage;

    protected ButtonState state;
    void Start()
    {
        switch (state)
        {
            case ButtonState.Tile:
                buttonImage.color = Color.white;
                break;
            case ButtonState.Obstacle:
                buttonImage.color = Color.red;
                break;
            case ButtonState.Table:
                buttonImage.color = Color.blue;
                break;
            default:
                buttonImage.color = Color.white;
                break;
        }
    }

    public ButtonState ButtonStateGetSet
    {
        get { return state; }
        set { state = value; }
    }
    public void ButtonColorChange()
    {
        //Debug.Log("previous State:  " + state);
        switch (state)
        {
            case ButtonState.Tile:
                state = ButtonState.Obstacle;
                buttonImage.color = Color.red;
                break;
            case ButtonState.Obstacle:
                //state = ButtonState.Table;
                //renderer.color = Color.blue;
                state = ButtonState.Tile;
                buttonImage.color = Color.white;
                break;
            case ButtonState.Table:
               state = ButtonState.Tile;
                buttonImage.color = Color.white;
                break;
            default:
                buttonImage.color = Color.white;
                break;
        }
    }

    public void SetColor() 
    {
        
    }
}
