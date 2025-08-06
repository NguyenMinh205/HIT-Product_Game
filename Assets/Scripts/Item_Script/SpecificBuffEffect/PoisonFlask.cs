using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PoisonFlask : EffectItem
{
    private int effectVal = 2;
    public override void Effect(Enemy enemy)
    {
        IBuffEffect poisonEffect = enemy.GetActiveEffect("poison_effect");
        if (poisonEffect != null)
        {
            poisonEffect.Value *= effectVal;
            enemy.EffectUICheck();
        }
    }

    public override void Execute(Player player, Enemy target)
    {
        if (target != null)
        {
            Effect(target);
        }
    }

    public override void Upgrade()
    {
        effectVal = 3;
    }
}
