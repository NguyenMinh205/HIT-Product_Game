using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingOfStrength : BuffItem
{
    private int buffVal = 2;
    public int BuffVal { get { return buffVal; } set { buffVal = value; } }
    public override void Buff(Player player)
    {
        player.Stats.ChangeDamageExtra(buffVal);
    }

    public override void Execute(Player player, Enemy enemy)
    {
        if (player != null)
        {
            Buff(player);
        }
    }
}
