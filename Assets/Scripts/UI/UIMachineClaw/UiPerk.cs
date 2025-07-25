using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiPerk : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private int quality = 0;
    [SerializeField] private PerkBase perk;
    public PerkBase Perk => perk;

    public void SetPerk(PerkBase perk)
    {
        this.perk = perk;
        icon.sprite = perk.icon;
        icon.gameObject.SetActive(true);
    }

    public void SetTextPerk()
    {
        quality++;
        if(quality > 1)
        {
            text.gameObject.SetActive(true);
            text.text = quality.ToString();
        }
    }    

}
