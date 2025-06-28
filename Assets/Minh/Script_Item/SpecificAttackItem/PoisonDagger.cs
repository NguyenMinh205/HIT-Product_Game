using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDagger : AttackWithEffect
{
    public override void AttackEnemy(Enemy enemy, float damage = 0)
    {
        if (enemy == null) return;
        enemy.ReceiverDamage((int)damage);
    }

    public override void Effect(Enemy enemy, float effectVal = 0)
    {
        //Gây effectVal stack độc lên kẻ địch nhân đòn đánh
    }

    public override void Execute(GameObject player = null, GameObject target = null, float value = 0)
    {
        Enemy enemy = target?.GetComponent<Enemy>();
        if (enemy != null)
        {
            AttackEnemy(enemy, value);
            Effect(enemy);
        }
    }

    public void Execute(GameObject player = null, GameObject target = null, float attackVal = 0, float effectVal = 0)
    {
        Enemy enemy = target?.GetComponent<Enemy>();
        if (enemy != null)
        {
            AttackEnemy(enemy, attackVal);
            Effect(enemy, effectVal);
        }
    }
}
