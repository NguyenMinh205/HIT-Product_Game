using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CharacterStat", menuName = "Character/CharacterStat")]
public class CharacterStatSO : ScriptableObject
{
    public string characterId;
    public float currentHP = 100;
    public float maxHP = 100;
    public float damageIncrease = 0;
    public float criticalChance = 0;
    public float criticalDamage = 1.8f;
    public float bloodsuckingRate;
    public float shield;
    public float retainedBlock;
    public float magnetStrength = 1;
    public int fluffCount = 30;
    public float maxItemAddPerTurn = 20;
    public float priceReduction;
    public int clawPerTurn = 2;
    public int clawInGrannyRoom = 1;

    public CharacterStatSO Clone()
    {
        CharacterStatSO clone = ScriptableObject.CreateInstance<CharacterStatSO>();
        clone.characterId = characterId;
        clone.currentHP = currentHP;
        clone.maxHP = maxHP;
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
}