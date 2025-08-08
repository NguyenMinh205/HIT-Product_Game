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
    }
    public void Upgrade()
    {

    }
}
