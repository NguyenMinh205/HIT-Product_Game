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

    [Header("data")]
    [SerializeField] public string perkName;
    [SerializeField] public string description;
    public Image Icon => icon;

    public void SetPerk(Sprite i = null, string pName = null, string des = null)
    {
        if (i != null) icon.sprite = i;
        if (pName != null) perkName = pName;
        if (des != null) description = des;
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
