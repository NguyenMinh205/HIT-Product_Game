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
        if(obj is Enemy enemy)
        {
            if(enemy.DamageIncreased > 0 && !icon.gameObject.activeSelf)
            {
                enemy.UIInscrease.icon.gameObject.SetActive(true);
                enemy.UIInscrease.text.gameObject.SetActive(true);
            }

            text.text = enemy.DamageIncreased.ToString();
        }
        else if(obj is Player player)
        {
        }
    }
}
