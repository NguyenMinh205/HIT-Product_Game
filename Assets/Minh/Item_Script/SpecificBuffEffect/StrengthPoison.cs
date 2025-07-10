using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthPoison : BuffItem
{
    public override void Buff(Player player)
    {
        player.Stats.DoubleDamageExtra();
    }

    public override void Execute(Player player, Enemy enemy)
    {
        if (player != null)
        {
            Buff(player);
        }
    }
}
