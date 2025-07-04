using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDagger : AttackWithEffect
{
    private int damage = 5;
    public int Damage { get { return damage; } set { damage = value; } }
    private int effectVal = 2;
    public int EffectVal => effectVal;
    public override void AttackEnemy(Enemy enemy)
    {
        if (enemy == null) return;
        enemy.ReceiverDamage((int)damage);
    }

    public override void Effect(Enemy enemy)
    {
        //Gây effectVal stack độc lên kẻ địch nhân đòn đánh
    }

    public override void Execute(GameObject player = null, GameObject target = null)
    {
        Enemy enemy = target?.GetComponent<Enemy>();
        if (enemy != null)
        {
            AttackEnemy(enemy);
            Effect(enemy);
        }
    }
}
