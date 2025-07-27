using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum UIInscreaseType
{
    SetText,
}
public class UIInscrease : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI text;

    private void Awake()
    {
        ObserverManager<UIInscreaseType>.AddDesgisterEvent(UIInscreaseType.SetText, SetText);
    }

    private void OnDisable()
    {
        ObserverManager<UIInscreaseType>.RemoveAddListener(UIInscreaseType.SetText, SetText);
    }

    public void SetText(object obj)
    {
        if(obj is int t)
        {
            if(t > 0 && !icon.gameObject.activeSelf)
            {
                icon.gameObject.SetActive(true);
                text.gameObject.SetActive(true);
            }

            text.text = t.ToString();
        }
    }
}
