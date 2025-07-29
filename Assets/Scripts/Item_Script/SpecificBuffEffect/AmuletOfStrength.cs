using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmuletOfStrength : BuffItem
{
    private int buffVal = 5;
    public int BuffVal {  get { return buffVal; } set { buffVal = value; } }
    public override void Buff(Player player)
    {
        player.Stats.ChangeStrength(buffVal);
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