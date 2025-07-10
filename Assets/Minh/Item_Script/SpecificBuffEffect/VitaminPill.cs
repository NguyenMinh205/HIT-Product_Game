using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitaminPill : BuffItem
{
    public override void Buff(Player player)
    {
        int curDamageExtra = (int)player.Stats.DamageIncrease;
        int shieldIncrease = curDamageExtra / 2;
        player.Stats.ChangeDamageExtra(shieldIncrease);
    }

    public override void Execute(Player player, Enemy enemy)
    {
        if (player != null)
        {
            Buff(player);
        }
    }
}
