using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helmet : BuffItem
{
    public override void Buff(Player player)
    {
        player.Stats.DoubleShield();
    }

    public override void Execute(Player player, Enemy enemy)
    {
        if (player != null)
        {
            Buff(player);
        }
    }
}
