using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DefendItem : IItemAction
{
    public abstract void Defend(Player player, float shield = 0);

    public abstract void Execute(GameObject player, GameObject target, float value = 0);
}
