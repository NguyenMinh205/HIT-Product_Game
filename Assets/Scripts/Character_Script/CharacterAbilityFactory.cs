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
            case "Cha03":
                return new Damian();
            case "Cha05":
                return new Violet();
            default:
                Debug.LogWarning($"Không tìm thấy Ability cho: {characterID}");
                return null;
        }
    }
}
