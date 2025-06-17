using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoManager : MonoBehaviour
{
    [Space]
    [Header("Object")]
    [SerializeField] private ObjectBase obj;

    [Space]
    [Header("Hp")]
    [SerializeField] private GameObject image;
    [SerializeField] private Image imageHp;
    [SerializeField] private TextMeshProUGUI textHp;

    [Space]
    [Header("Armor")]
    [SerializeField] private GameObject armor;
    [SerializeField] private TextMeshProUGUI textArmor;

    private void Awake()
    {
        ObserverManager<IDInfoObject>.AddDesgisterEvent(IDInfoObject.UpdateHp, UpdateHp);
        ObserverManager<IDInfoObject>.AddDesgisterEvent(IDInfoObject.UpdateArmor, UpdateArmor);
    }

    public void UpdateArmor(object _)
    {
        if (obj == null) return;

        if (obj.Armor > 0)
        {
            armor.SetActive(true);
            textArmor.text = obj.Armor.ToString();
        }
        else
        {
            armor.SetActive(false);
        }
    }

    public void UpdateHp(object _)
    {
        if (obj == null) return;

        textHp.text = obj.HP + " / " + obj.CurrentHp;
        imageHp.fillAmount = (float)obj.HP / obj.CurrentHp;
    }
}