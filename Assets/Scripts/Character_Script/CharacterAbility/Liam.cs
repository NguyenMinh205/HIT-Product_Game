using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liam : ICharacterAbility
{
    public void StartSetupEffect(Player player)
    {
        player.AddBuffEffect("magnet_claw_each_turns", 1, -1);
        UiPerksList.Instance.AddPerks(PerkIconManager.Instance.Liam);
    }

    public void StartSetupStat()
    {
        
    }
}
