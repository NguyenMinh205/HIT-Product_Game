using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Leech : BuffItem
{
    private int buffVal = 1;
    public int BuffVal { get { return buffVal; } set { buffVal = value; } }
    private int healAmount = 0;
    public override void Buff(Player player)
    {
        player.Stats.ChangeCurHP(healAmount);
    }

    public override void Execute(Player player, Enemy target)
    {
        if (player != null && target != null)
        {
            IBuffEffect poisonEffect = target.GetActiveEffect("poison_effect");
            if (poisonEffect != null)
            {
                healAmount = (int)poisonEffect.Duration * buffVal;
                target.RemoveBuffEffect(poisonEffect);
            }
        }
    }

    public override void Upgrade()
    {
        BuffVal = 2;
    }
}
