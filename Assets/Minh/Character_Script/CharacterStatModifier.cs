using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatModifier : MonoBehaviour
{
    public Player CurPlayer { get; set; }
    public CharacterStatSO Stats { get; set; }

    public void ChangeCurHP(float value)
    {
        Stats.currentHP += value;
        if (Stats.currentHP > Stats.maxHP)
        {
            Stats.currentHP = Stats.maxHP;
        }    
    }
    
    public void ChangeMaxHP(float value)
    {
        Stats.maxHP += value;
    }

    public void ChangeShield(float value)
    {
        Stats.shield += value;
    }

    public void DoubleShield()
    {
        Stats.shield *= 2;
    }    

    public void ChangeDamageExtra(float value)
    {
        Stats.damageIncrease += value;
    }
    
    public void DoubleDamageExtra()
    {
        Stats.damageIncrease *= 2;
    }

    public void ChangeCriticalChance(float value)
    {
        Stats.damageIncrease += value;
    }

    public void ChangeCriticalDamage(float value)
    {
        Stats.damageIncrease += value;
    }

    public void ChangeBloodsuckingRate(float value)
    {
        Stats.bloodsuckingRate += value;
    }    
}
