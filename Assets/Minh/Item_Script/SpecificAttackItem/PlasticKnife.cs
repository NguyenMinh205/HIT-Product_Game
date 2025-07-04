using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasticKnife : AttackItem
{
    private int damage = 15;
    public int Damage { get { return damage; } set { damage = value; } }
    private int minDamage = 15;
    public int MinDamage { get { return minDamage; } set { minDamage = value; } }
    public override void AttackEnemy(Enemy enemy)
    {
        if (enemy == null) return;
        enemy.ReceiverDamage((int)damage);
        if (damage != minDamage)
        {
            damage -= minDamage;
        }    
    }

    public override void Execute(GameObject player, GameObject target)
    {
        if (target.TryGetComponent<Enemy>(out var enemy))
        {
            AttackEnemy(enemy);
        }
    }
}
