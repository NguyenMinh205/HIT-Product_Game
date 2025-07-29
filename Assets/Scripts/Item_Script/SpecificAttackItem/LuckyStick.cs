using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyStick : AttackItem
{
    private int minDamage = 1;
    public int MinDamage { get { return minDamage; } set { minDamage = value; } }
    private int maxDamage = 15;
    public int MaxDamage { get { return maxDamage; } set { maxDamage = value; } }
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
            curDamage = Random.Range(minDamage, maxDamage);
            curDamage = CalculateDamageWithCrit(player, curDamage);
            AttackEnemy(enemy);
        }
    }

    public override void Upgrade()
    {
        minDamage = 5;
        maxDamage *= 2;
    }
}
