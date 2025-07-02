using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackWithBuffItem : IItemAction
{
    public abstract void AttackEnemy(Enemy enemy, float damage = 0);

    public abstract void Buff(Player player, float value = 0);

    public abstract void Execute(GameObject player, GameObject target, float value = 0);
}
