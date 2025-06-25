using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackItem : IAttackItem
{
    public virtual void AttackEnemy(Enemy enemy, float damage = 0)
    {
        if (enemy == null) return;
        enemy.ReceiverDamage((int)damage);
    }
}
