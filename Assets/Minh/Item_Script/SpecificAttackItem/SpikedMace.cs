using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikedMace : AttackItem
{
    private int damage = 0;
    public override void AttackEnemy(Enemy enemy)
    {
        enemy.ReceiverDamage((int)damage);
    }

    public override void Execute(Player player, Enemy enemy)
    {
        if (enemy != null)
        {
            AttackEnemy(enemy);
        }
    }
}
