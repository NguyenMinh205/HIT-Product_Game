using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paperclips : AttackItem
{
    private int damage = 2;
    public int Damage { get { return damage; } set { damage = value; } }
    private int strength = 0;
    private int curDamage = 0;
    public override void AttackEnemy(Enemy enemy)
    {
        if (strength == 0)
            enemy.ReceiverDamage(curDamage);
        enemy.ReceiverDamage(curDamage * strength);
    }

    public override void Execute(Player player, Enemy enemy)
    {
        if (player != null)
        {
            strength = (int)player.Stats.DamageIncrease;
        }

        if (enemy != null)
        {
            curDamage = CalculateDamageWithCrit(player, damage);
            AttackEnemy(enemy);
        }
    }
}
