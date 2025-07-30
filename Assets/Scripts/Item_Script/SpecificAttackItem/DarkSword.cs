using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkSword : AttackItem
{
    private int damage = 25;
    public int Damage { get { return damage; } set { damage = value; } }

    public override void AttackEnemy(Enemy enemy)
    {
        enemy.ReceiverDamage(damage);
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
        damage += 10;
    }
}