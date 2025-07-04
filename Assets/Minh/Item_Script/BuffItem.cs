using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffItem : IItemAction
{
    public abstract void Buff(Player player);

    public abstract void Execute(GameObject player, GameObject target);
}
