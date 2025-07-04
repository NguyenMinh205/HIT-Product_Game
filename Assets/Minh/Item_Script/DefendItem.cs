using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DefendItem : IItemAction
{
    public abstract void Defend(Player player);

    public abstract void Execute(GameObject player, GameObject target);
}
