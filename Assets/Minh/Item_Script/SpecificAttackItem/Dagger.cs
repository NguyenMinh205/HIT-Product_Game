using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : AttackItem
{
    private int damage = 5;
    public int Damage { get { return damage; } set { damage = value; } }
    public override void AttackEnemy(Enemy enemy)
    {
        if (enemy == null) return;
        enemy.ReceiverDamage((int)damage);
    }

    public override void Execute(Player player, Enemy enemy)
    {
        if (enemy == null) return;
        AttackEnemy(enemy);
    }
}
