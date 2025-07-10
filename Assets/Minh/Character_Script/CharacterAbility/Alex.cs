using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alex : ICharacterAbility
{
    public void StartSetup(Player player)
    {
        player.Stats.ChangeMaxHP(10);
        player.Stats.ChangeCurHP(10);
        player.AddBuffEffect("buff_shield_start_turn", 5, -1);
    }
}
