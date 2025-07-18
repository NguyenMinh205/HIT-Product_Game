using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum IDMap
{
    UpdateHpBar,
}
public class HealthBarMap : MonoBehaviour
{
    [SerializeField] private Image imageHp;
    [SerializeField] private TextMeshProUGUI textHp;

    private void Awake()
    {
        ObserverManager<IDMap>.AddDesgisterEvent(IDMap.UpdateHpBar, UpdateHpBarInMap);
    }

    /*    private void OnDisable()
        {
            ObserverManager<IDMap>.RemoveAddListener(IDMap.UpdateHpBar, UpdateHpBarInMap);
        }*/


    public void UpdateHpBarInMap(object obj)
    {
        textHp.text = GamePlayController.Instance.PlayerController.CurPlayerStat.CurrentHP + "/" + GamePlayController.Instance.PlayerController.CurPlayerStat.MaxHP;
        imageHp.fillAmount = (float)GamePlayController.Instance.PlayerController.CurPlayerStat.CurrentHP / GamePlayController.Instance.PlayerController.CurPlayerStat.MaxHP;
    }
}
