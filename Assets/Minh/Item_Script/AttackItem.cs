using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackItem : IItemAction
{
    public abstract void AttackEnemy(Enemy enemy);

    public abstract void Execute(GameObject player, GameObject target);
}
