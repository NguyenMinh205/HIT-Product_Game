using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : IItemAction
{
    public void Execute(Player player, Enemy target)
    {
        if(player != null)
        {
            player.Stats.ChangeCoin(1);
        }
        Debug.Log("Coin used by player: " + player.name);
    }
    public void Upgrade()
    {
        Debug.Log("Coin upgraded.");
    }
}
