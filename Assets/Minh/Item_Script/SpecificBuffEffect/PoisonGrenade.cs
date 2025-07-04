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

    public override void Execute(GameObject player, GameObject target)
    {
        if (target.TryGetComponent<Enemy>(out var enemy))
        {
            Effect(enemy);
        }
    }

    public void Execute(GameObject player, List<GameObject> targets)
    {
        foreach (GameObject target in targets)
        {
            if (target != null)
            {
                Execute(player, target);
            }
        }

    }
}
