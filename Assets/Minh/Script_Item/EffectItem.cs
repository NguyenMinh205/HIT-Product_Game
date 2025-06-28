using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectItem : IItemAction
{
    public abstract void Effect(Enemy enemy, float value = 0);

    public abstract void Execute(GameObject player, GameObject target, float value = 0);
}
