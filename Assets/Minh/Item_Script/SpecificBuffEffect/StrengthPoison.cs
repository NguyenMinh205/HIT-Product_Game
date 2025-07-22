using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthPoison : BuffItem
{
    private int buffVal = 2;
    public int BuffVal { get; set; }
    public override void Buff(Player player)
    {
        player.Stats.MultipleStrength(buffVal);
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
        buffVal = 3;
    }
}
