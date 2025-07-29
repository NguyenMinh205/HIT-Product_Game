using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasticKnife : AttackItem
{
    private int damage = 15;
    public int Damage { get { return damage; } set { damage = value; } }
    private int minDamage = 3;
    public int MinDamage { get { return minDamage; } set { minDamage = value; } }
    private int curDamage = 0;
    public override void AttackEnemy(Enemy enemy)
    {
        if (enemy == null) return;
        enemy.ReceiverDamage(curDamage);
        if (damage > minDamage)
        {
            damage -= minDamage;
        }    
    }

    public override void Execute(Player player, Enemy enemy)
    {
        if (enemy != null)
        {
            curDamage = CalculateDamageWithCrit(player, damage);
            AttackEnemy(enemy);
        }
    }

    public override void Upgrade()
    {
        damage += 10;
        minDamage = 5;
    }
}
