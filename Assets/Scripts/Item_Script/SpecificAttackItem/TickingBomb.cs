using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickingBomb : BuffItem
{
    private int buffVal = 3;
    public int BuffVal { get { return buffVal; } set { buffVal = value; } }
    public override void Buff(Player player)
    {
        player.AddBuffEffect("bomb_effect", 0, buffVal);
    }

    public override void Execute(Player player, Enemy target)
    {
        if (player == null) return;
        Buff(player);
    }

    public override void Upgrade()
    {
        buffVal = 2;
    }
}
