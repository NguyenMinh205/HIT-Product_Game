using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    /*[Space]
    [Header("Object")]
    [SerializeField] private ObjectBase obj;*/

    [Space]
    [Header("Hp")]
    //[SerializeField] private GameObject imageObject;
    [SerializeField] private Image imageHp;
    [SerializeField] private TextMeshProUGUI textHp;

    [Space]
    [Header("Armor")]
    [SerializeField] private GameObject armor;
    [SerializeField] private TextMeshProUGUI textArmor;

  /*  public ObjectBase Obj
    {
        get => this.obj;
        set => this.obj = value;
    }*/
    public void InitHealthBar(ObjectBase obj)
    {
        gameObject.SetActive(true);

        UpdateArmor(obj);
        UpdateHp(obj);
    }

    public void UnShowHealthBarEnemy()
    {
        gameObject.SetActive(false);
    }

    public void UpdateArmor(ObjectBase obj)
    {
        Debug.Log("Up Armor");
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

    public void UpdateHp(ObjectBase obj)
    {
        Debug.Log("Up Hp");
        if (obj == null) return;

        textHp.text = obj.HP + " / " + obj.CurrentHp;
        imageHp.fillAmount = (float)obj.HP / obj.CurrentHp;
    }
}
