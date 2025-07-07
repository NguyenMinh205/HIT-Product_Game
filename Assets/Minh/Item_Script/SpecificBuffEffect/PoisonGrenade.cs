using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonGrenade : EffectItem
{
    private int effectVal = 3;
    public int EffectVal { get { return effectVal; } set { effectVal = value; } }
    public override void Effect(Enemy enemy)
    {
        //Set hiệu ứng độc
    }

    public override void Execute(Player player, Enemy enemy)
    {
        if (enemy != null)
        {
            Effect(enemy);
        }
    }

    public void Execute(Player player, List<Enemy> targets)
    {
        foreach (Enemy target in targets)
        {
            if (target != null)
            {
                Execute(player, target);
            }
        }

    }
}
