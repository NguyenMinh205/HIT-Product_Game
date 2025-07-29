using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDrink : BuffItem
{
    public override void Buff(Player player)
    {
        int curShield = (int)player.Stats.Shield;
        int damageExtra = curShield / 4;
        player.Stats.ChangeStrength(damageExtra);
    }

    public override void Execute(Player player, Enemy enemy)
    {
        if (player != null)
        {
            Buff(player);
        }
    }

    public override void Upgrade()
    {
        return;
    }
}
