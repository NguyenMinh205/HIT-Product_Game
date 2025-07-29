using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[System.Serializable]
public class CharacterStat
{
    [SerializeField] private string characterId;
    private string CharacterId => characterId;
    [SerializeField] private float currentHP = 100;
    public float CurrentHP => currentHP;
    [SerializeField] private float maxHP = 100;
    public float MaxHP => maxHP;
    [SerializeField] private int coin = 0;
    public int Coin => coin;
    [SerializeField] private int strength;
    public int Strength => strength;
    [SerializeField] private float criticalChance = 0.05f;
    public float CriticalChance => criticalChance;
    [SerializeField] private float criticalDamage = 1.8f;
    public float CriticalDamage => criticalDamage;
    [SerializeField] private float bloodsuckingRate = 0;
    public float BloodsuckingRate => bloodsuckingRate;
    [SerializeField] private float shield;
    public float Shield => shield;
    [SerializeField] private float retainedBlock = 0;
    public float RetainedBlock => retainedBlock;
    [SerializeField] private float damageAbsorb = 0;
    public float DamageAbsorb => damageAbsorb;
    [SerializeField] private float magnetStrength = 1;
    public float MagnetStrength => magnetStrength;
    [SerializeField] private int fluffCount = 30;
    public int FluffCount => fluffCount;
    [SerializeField] private float maxItemAddPerTurn = 15;
    public float MaxItemAddPerTurn => maxItemAddPerTurn;
    [SerializeField] private float priceReduction = 0;
    public float PriceReduction => priceReduction;
    [SerializeField] private int clawPerTurn = 2;
    public int ClawPerTurn => clawPerTurn;
    [SerializeField] private int clawInGrannyRoom = 1;
    public int ClawInGrannyRoom => clawInGrannyRoom;
    [SerializeField] private int upgradeFreeTurn = 0;
    public int UpgradeFreeTurn => upgradeFreeTurn;
    [SerializeField] private int shredderFreeTurn = 0;
    public int ShredderFreeTurn => shredderFreeTurn;
    [SerializeField] private int rerollFreeTurn = 0;
    public int RerollFreeTurn => rerollFreeTurn;

    public CharacterStat Clone()
    {
        CharacterStat clone = new CharacterStat();
        clone.characterId = characterId;
        clone.currentHP = currentHP;
        clone.maxHP = maxHP;
        clone.coin = coin;
        clone.strength = strength;
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
        clone.upgradeFreeTurn = upgradeFreeTurn;
        clone.shredderFreeTurn = shredderFreeTurn;
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
        }
        else
        {
            this.shield += value;
        }
        GamePlayController.Instance.PlayerController.CurrentPlayer.UpdateArmorUI();
    }

    public void ChangeCoin(int value)
    {
        this.coin += value;
        GamePlayController.Instance.PlayerController.UpdateCoinText();
        ObserverManager<EventID>.PostEven(EventID.OnTakeCoin, value);
    }

    public void MultipleShield(int val)
    {
        this.shield *= val;
        GamePlayController.Instance.PlayerController.CurrentPlayer.UpdateArmorUI();
    }

    public void ChangeStrength(int value)
    {
        this.strength += value;
    }

    public void MultipleStrength(int value)
    {
        this.strength *= value;
    }

    public void ChangeCriticalChance(float value)
    {
        if (this.criticalChance + value > 1)
        {
            this.criticalChance = 1f;
        }
        this.criticalChance += value;
    }

    public void ChangeCriticalDamage(float value)
    {
        this.criticalDamage += value;
    }

    public void ChangeBloodsuckingRate(float value)
    {
        this.bloodsuckingRate += value;
    }

    public void ChangeRetainedBlock(float value)
    {
        this.retainedBlock += value;
    }

    public void ChangeDamageAbsorb(float value)
    {
        this.damageAbsorb += value;
    }

    public void ChangePriceReduction(float value)
    {
        this.priceReduction += value;
    }

    public void ChangeUpgradeFreeTurn(int value)
    {
        this.upgradeFreeTurn += value;
    }

    public void ChangeShredderFreeTurn(int value)
    {
        this.shredderFreeTurn += value;
    }

    public void ChangeRerollFreeTurn(int value)
    {
        this.rerollFreeTurn += value;
    }

    public void ResetStatAfterRound()
    {
        this.maxHP = GamePlayController.Instance.PlayerController.CurPlayerStat.MaxHP;
        if (this.currentHP > this.maxHP)
        {
            this.currentHP = this.maxHP;
        }
        this.shield = 0;
        this.strength = 0;
    }    
}