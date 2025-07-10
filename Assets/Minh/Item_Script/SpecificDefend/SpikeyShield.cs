using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeyShield : DefendWithBuff
{
    private int shield = 10;
    public int Shield { get { return shield; } set { shield = value; } }
    public override void Buff(Player player)
    {
        //player.AddBuffEffect("thorns_damage", 5, -1);
    }

    public override void Defend(Player player)
    {
        player._CharacterStatModifier.ChangeShield(shield);
    }

    public override void Execute(Player player, Enemy enemy)
    {
        if (player != null)
        {
            Defend(player);
            Buff(player);
        }
    }
}
