using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textHp;
    [SerializeField] private Image imageHp;

    public void SetHp(int hp, int currentHP)
    {
        textHp.text = hp.ToString() + '/' + currentHP.ToString();
        imageHp.fillAmount = (float)hp / currentHP;
    }
}
