using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sickle : AttackItem
{
    private int damage = 15;
    public int Damage { get { return damage; } set { damage = value; } }
    private int curDamage = 0;
    public override void AttackEnemy(Enemy enemy)
    {
        if (enemy == null) return;
        enemy.ReceiverDamage(curDamage);
    }

    public override void Execute(Player player, Enemy enemy)
    {
        if (enemy != null)
        {
            float criticalChance = player.Stats.CriticalChance;
            float criticalDamage = player.Stats.CriticalDamage;

            if (Random.value <= criticalChance + 0.5f)
            {
                curDamage = (int)(damage * criticalDamage);
            }
            curDamage = damage;
            AttackEnemy(enemy);
        }
    }

    public override void Upgrade()
    {
        damage += 10;
    }
}
