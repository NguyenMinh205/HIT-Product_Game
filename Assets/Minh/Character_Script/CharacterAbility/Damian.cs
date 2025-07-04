using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damian : ICharacterAbility
{
    public void StartSetup(Player player)
    {
        player._CharacterStatModifier.ChangeMaxHP(10);
        player._CharacterStatModifier.ChangeCurHP(10);
        player.AddBuffEffect("lifesteal", 20, -1);
    }
}
