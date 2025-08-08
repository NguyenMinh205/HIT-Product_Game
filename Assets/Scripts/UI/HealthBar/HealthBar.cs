using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Space]
    [Header("Hp")]
    [SerializeField] private Image imageHp;
    [SerializeField] private TextMeshProUGUI textHp;

    [Space]
    [Header("Armor")]
    [SerializeField] private GameObject armor;
    [SerializeField] private TextMeshProUGUI textArmor;

    [Space]
    [Header("Strength")]
    [SerializeField] private GameObject strength;
    [SerializeField] private TextMeshProUGUI textStrength;

    public void InitHealthBar(Object obj)
    {
        UpdateArmor(obj);
        UpdateHp(obj);
        if (obj is Player player)
        {
            UpdateStrength(player);
        }
    }

    public void UnShowHealthBarEnemy()
    {
        gameObject.SetActive(false);
    }

    public void UpdateArmor(Object obj)
    {
        if (obj is Enemy enemy)
        {
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
            textHp.text = enemy.HP + " / " + enemy.CurrentHp;
            imageHp.fillAmount = (float)enemy.HP / enemy.CurrentHp;
        }
        else if (obj is Player player)
        {
            textHp.text = player.Stats.CurrentHP + "/" + player.Stats.MaxHP;
            imageHp.fillAmount = (float)player.Stats.CurrentHP / player.Stats.MaxHP;
        }
    }

    public void UpdateStrength(Player player)
    {
        if (player == null) return;
        if (player.Stats.Strength > 0)
        {
            strength.SetActive(true);
            textStrength.text = player.Stats.Strength.ToString();
        }
        else
        {
            strength.SetActive(false);
        }
    }
}
