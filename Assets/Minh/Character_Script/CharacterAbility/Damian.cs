using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damian : ICharacterAbility
{
    public void StartSetup(Player player)
    {
        player.Stats.ChangeMaxHP(10);
        player.Stats.ChangeCurHP(10);
        player.AddBuffEffect("lifesteal", 1.2f, -1);
    }
}
