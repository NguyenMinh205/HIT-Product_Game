using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallShield : DefendItem
{
    private int shield = 5;
    public int Shield { get { return shield; } set { shield = value; } }
    public override void Defend(Player player)
    {
        player.Stats.ChangeShield(shield);
        player.Health.UpdateArmor(player);
    }

    public override void Execute(Player player, Enemy enemy)
    {
        if (player != null)
        {
            Defend(player);
            ObserverManager<IDStateAnimationPlayer>.PostEven(IDStateAnimationPlayer.Buff, null);
        }
    }

    public override void Upgrade()
    {
        shield *= 2;
    }
}
