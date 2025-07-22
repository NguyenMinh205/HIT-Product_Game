using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMirror : BuffItem
{
    private int buffVal = 1;
    public int BuffValue { get => buffVal; set => buffVal = value; }
    public override void Buff(Player player)
    {
        player.AddBuffEffect("counter_attack", buffVal, -1);
    }

    public override void Execute(Player player, Enemy target)
    {
        if (player != null)
        {
            Buff(player);
        }    
    }

    public override void Upgrade()
    {
        buffVal++;
    }
}
