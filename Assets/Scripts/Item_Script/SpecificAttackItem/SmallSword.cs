using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallSword: AttackItem
{
    private int damage = 10;
    public int Damage { get { return damage; } set { damage = value; } }
    public override void AttackEnemy(Enemy enemy)
    {
        int index = Random.Range(0, 3);
        if (index == 0)
            enemy.ReceiverDamage((int)damage);
        else
            enemy.ReceiverDamage((int)damage * 2);
    }

    public override void Execute(Player player, Enemy enemy)
    {
        if (enemy != null)
        {
            AttackEnemy(enemy);
        }
    }

    public override void Upgrade()
    {
        damage *= 2;
    }
}
