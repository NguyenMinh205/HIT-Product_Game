using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Violet : ICharacterAbility
{
    public void StartSetupEffect(Player player)
    {
        player.AddBuffEffect("poison_damage", 1, -1);
        UiPerksList.Instance.AddPerks(PerkIconManager.Instance.Violet);
    }

    public void StartSetupStat()
    {
        
    }
}
