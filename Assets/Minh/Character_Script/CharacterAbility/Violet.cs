using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Violet : ICharacterAbility
{
    public void StartSetupEffect(Player player)
    {
        player.AddBuffEffect("poison_damage", 1, -1);
    }

    public void StartSetupStat()
    {
        
    }
}
