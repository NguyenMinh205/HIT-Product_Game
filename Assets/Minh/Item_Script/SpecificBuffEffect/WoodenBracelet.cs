using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenBracelet : BuffItem
{
    private int buffVal = 10;
    public int BuffVal { get { return buffVal; } set { buffVal = value; } }

    public override void Buff(Player player)
    {
        player.Stats.ChangeDamageExtra(buffVal * 0.1f);
        player.Stats.ChangeShield(buffVal);
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
        buffVal *= 2;
    }
}
