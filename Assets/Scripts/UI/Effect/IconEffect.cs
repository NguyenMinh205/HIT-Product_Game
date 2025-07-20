using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconEffect : MonoBehaviour
{
    [SerializeField] private Image icon;

    public void SetIcon(Sprite sprite)
    {
        if (icon != null)
        {
            icon.sprite = sprite;
            icon.enabled = true;
        }
    }

}
