using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiggyBank : BuffItem
{
    private int minCoin = 2;
    public int MinCoin { get; set; }
    private int maxCoin = 5;
    public int MaxCoin { get; set; }

    public override void Buff(Player player)
    {
        int coinToAdd = Random.Range(minCoin, maxCoin + 1);
        player.Stats.ChangeCoin(coinToAdd);
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
        minCoin = 5;
        maxCoin *= 2;
    }
}
