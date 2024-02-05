using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Created by Chris LoSardo
/// 2/3/2024
/// Variables and logic for the tiles
/// </summary>
public class Tile : MonoBehaviour
{
    [SerializeField] private Color tileColor;
    [SerializeField] private Color tileColor2;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private GameObject highlight;


    public void ChangeColor(bool isOffset)
    {
        if (isOffset)
        {
            renderer.color = tileColor2;
        }
        else
        {
            renderer.color = tileColor;
        }
    }

    private void OnMouseEnter()
    {
        highlight.SetActive(true);
    }

    private void OnMouseExit()
    {
        highlight.SetActive(false);
    }
}
