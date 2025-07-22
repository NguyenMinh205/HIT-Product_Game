using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Antidote : BuffItem
{
    public override void Buff(Player player)
    {
        IBuffEffect effect = player.GetActiveEffect("poison_effect");
        if (effect != null)
        {
            player.RemoveBuffEffect(effect);
        }    
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
        
    }
}
