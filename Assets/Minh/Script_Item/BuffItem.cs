using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffItem : IItemAction
{
    public abstract void Buff(Player player, float value = 0);

    public abstract void Execute(GameObject player, GameObject target, float value = 0);
}
