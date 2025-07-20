using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextEffect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void SetText(float value)
    {
        if (text != null)
        {
            if(value == -1)
            {
                text.enabled = false;
                return;
            }
            text.text = value.ToString();
            text.enabled = true;
        }
    }
}
