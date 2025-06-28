using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : AttackItem
{
    public override void AttackEnemy(Enemy enemy, float damage = 0)
    {
        if (enemy == null) return;
        enemy.ReceiverDamage((int)damage);
    }

    public override void Execute(GameObject player, GameObject target, float value = 0)
    {
        Enemy enemy = target?.GetComponent<Enemy>();
        if (enemy == null) return;
        AttackEnemy(enemy, value);
    }
}
