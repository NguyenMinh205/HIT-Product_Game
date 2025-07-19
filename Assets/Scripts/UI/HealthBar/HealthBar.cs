using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    public void InitHealthBar(Object obj)
    {
        UpdateArmor(obj);
        UpdateHp(obj);
    }

    public void UnShowHealthBarEnemy()
    {
        gameObject.SetActive(false);
    }

    public void UpdateArmor(Object obj)
    {
        if (obj is Enemy enemy)
        {
            Debug.Log("Up Armor");
            if (enemy == null) return;

            if (enemy.Armor > 0)
            {
                armor.SetActive(true);
                textArmor.text = enemy.Armor.ToString();
            }
            else
            {
                armor.SetActive(false);
            }
        }
        else if(obj is Player player)
        {
            if(player.Stats.Shield > 0)
            {
                armor.SetActive(true);
                textArmor.gameObject.SetActive(true);
                textArmor.text = player.Stats.Shield.ToString();
            }
            else if(player.Stats.Shield <= 0)
            {
                armor.SetActive(false);
                textArmor.gameObject.SetActive(false);
            }
        }
    }

    public void UpdateHp(Object obj)
    {
        if (obj is Enemy enemy)
        {
            Debug.Log("Up Hp");
            if (enemy == null) return;

            textHp.text = enemy.HP + " / " + enemy.CurrentHp;
            imageHp.fillAmount = (float)enemy.HP / enemy.CurrentHp;
        }
        else if (obj is Player player)
        {
            Debug.Log("Update Hp Player");
            textHp.text = player.Stats.CurrentHP + "/" + player.Stats.MaxHP;
            imageHp.fillAmount = (float)player.Stats.CurrentHP / player.Stats.MaxHP;
        }
    }
}
