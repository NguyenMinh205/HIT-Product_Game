using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectItem : IItemAction
{
    public abstract void Effect(Enemy enemy);

    public abstract void Upgrade();

    public abstract void Execute(Player player, Enemy target);
}
