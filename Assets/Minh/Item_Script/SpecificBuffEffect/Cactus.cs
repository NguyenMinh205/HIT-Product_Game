using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cactus : BuffItem
{
    private int buffVal = 10;
    public int BuffVal { get { return buffVal; } set { buffVal = value; } }
    public override void Buff(Player player)
    {
        player._CharacterStatModifier.ChangeCurHP(-buffVal * 0.2f);
        player._CharacterStatModifier.ChangeShield(buffVal);
    }

    public override void Execute(Player player, Enemy enemy)
    {
        if (player != null)
        {
            Buff(player);
        }
    }
}
