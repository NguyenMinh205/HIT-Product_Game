using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "New CharacterStat", menuName = "Character/CharacterStat")]
public class CharacterStatSO : ScriptableObject
{
    [SerializeField] private string characterId;
    private string CharacterId => characterId;
    [SerializeField] private float currentHP = 100;
    public float CurrentHP => currentHP;
    [SerializeField] private float maxHP = 100;
    public float MaxHP => maxHP;
    [SerializeField] private float coin = 0;
    public float Coin => coin;
    [SerializeField] private float damageIncrease = 0;
    public float DamageIncrease => damageIncrease;
    [SerializeField] private float criticalChance = 0.05f;
    public float CriticalChance => criticalChance;
    [SerializeField] private float criticalDamage = 1.8f;
    public float CriticalDamage => criticalDamage;
    [SerializeField] private float bloodsuckingRate;
    public float BloodsuckingRate => bloodsuckingRate;
    [SerializeField] private float shield;
    public float Shield => shield;
    [SerializeField] private float retainedBlock;
    public float RetainedBlock => retainedBlock;
    [SerializeField] private float magnetStrength = 1;
    public float MagnetStrength => magnetStrength;
    [SerializeField] private int fluffCount = 30;
    public int FluffCount => fluffCount;
    [SerializeField] private float maxItemAddPerTurn = 15;
    public float MaxItemAddPerTurn => maxItemAddPerTurn;
    [SerializeField] private float priceReduction;
    public float PriceReduction => priceReduction;
    [SerializeField] private int clawPerTurn = 2;
    public float ClawPerTurn => clawPerTurn;
    [SerializeField] private int clawInGrannyRoom = 1;
    public int ClawInGrannyRoom => clawInGrannyRoom;

    public CharacterStatSO Clone()
    {
        CharacterStatSO clone = ScriptableObject.CreateInstance<CharacterStatSO>();
        clone.characterId = characterId;
        clone.currentHP = currentHP;
        clone.maxHP = maxHP;
        clone.coin = coin;
        clone.damageIncrease = damageIncrease;
        clone.criticalChance = criticalChance;
        clone.criticalDamage = criticalDamage;
        clone.bloodsuckingRate = bloodsuckingRate;
        clone.shield = shield;
        clone.retainedBlock = retainedBlock;
        clone.magnetStrength = magnetStrength;
        clone.fluffCount = fluffCount;
        clone.maxItemAddPerTurn = maxItemAddPerTurn;
        clone.priceReduction = priceReduction;
        clone.clawPerTurn = clawPerTurn;
        clone.clawInGrannyRoom = clawInGrannyRoom;
        return clone;
    }

    public void ChangeCurHP(float value)
    {
        this.currentHP += value;
        if (this.currentHP > this.maxHP)
        {
            this.currentHP = this.maxHP;
        }
    }

    public void ChangeMaxHP(float value)
    {
        this.maxHP += value;
    }

    public void ChangeShield(float value)
    {
        if (this.shield + value <= 0)
        {
            this.shield = 0;
            return;
        }
        this.shield += value;
    }

    public void ChangeCoin(int value)
    {
        this.coin += value;
        GamePlayController.Instance.PlayerController.UpdateCoinText();
    }

    public void MultipleShield(int val)
    {
        this.shield *= val;
    }

    public void ChangeDamageExtra(float value)
    {
        this.damageIncrease += value;
    }

    public void MultipleDamageExtra(int value)
    {
        this.damageIncrease *= value;
    }

    public void ChangeCriticalChance(float value)
    {
        if (this.damageIncrease + value > 1)
        {
            this.damageIncrease = 1f;
        }
        this.damageIncrease += value;
    }

    public void ChangeCriticalDamage(float value)
    {
        this.damageIncrease += value;
    }

    public void ChangeBloodsuckingRate(float value)
    {
        this.bloodsuckingRate += value;
    }
}