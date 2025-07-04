using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackWithEffect : IItemAction
{
    public abstract void AttackEnemy(Enemy enemy);

    public abstract void Effect(Enemy enemy);

    public abstract void Execute(GameObject player, GameObject target);
}
