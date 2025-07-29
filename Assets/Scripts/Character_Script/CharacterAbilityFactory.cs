using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterAbilityFactory
{
    public static ICharacterAbility CreateAbility(string characterID)
    {
        switch (characterID)
        {
            case "Cha01":
                return new Alex();
            case "Cha02":
                return new Liam();
            case "Cha03":
                return new Damian();
            case "Cha04":
                return new Coby();
            case "Cha05":
                return new Violet();
            default:
                Debug.LogWarning($"Không tìm thấy Ability cho: {characterID}");
                return null;
        }
    }
}
