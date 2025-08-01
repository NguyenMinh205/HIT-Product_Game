using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiPerk : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    [SerializeField] private Image icon;

    public void SetPerk(Sprite sprite)
    {
        icon.sprite = sprite;
    }
    
    public void SetPos(int index)
    {
        if(index % 2 == 1)
        {
            float y = (3-(index / 2 + 1)) * 60f;
            rect.anchoredPosition = new Vector2(-33.5f, y);
        }
        else
        {
            float y = (3-(index / 2)) * 60f;
            rect.anchoredPosition = new Vector2(33.5f, y);
        }
    }
}
