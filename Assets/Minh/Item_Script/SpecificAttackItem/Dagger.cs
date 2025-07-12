using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : AttackItem
{
    private int damage = 5;
    public int Damage { get { return damage; } set { damage = value; } }
    private int curDamage = 0;

    public override void AttackEnemy(Enemy enemy)
    {
        if (enemy == null) return;
        enemy.ReceiverDamage((int)curDamage);
    }

    public override void Execute(Player player, Enemy enemy)
    {
        if (enemy != null)
        {
            curDamage = CalculateDamageWithCrit(player, curDamage);
            AttackEnemy(enemy);
        }
    }

    public override void Upgrade()
    {
        damage *= 2;
    }
}
