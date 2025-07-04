using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyStick : AttackItem
{
    private int minDamage = 1;
    public int MinDamage { get { return minDamage; } set { minDamage = value; } }
    private int maxDamage = 15;
    public int MaxDamage { get { return maxDamage; } set { maxDamage = value; } }
    public override void AttackEnemy(Enemy enemy)
    {
        if (enemy == null) return;
        enemy.ReceiverDamage(Random.Range(minDamage, maxDamage));
    }

    public override void Execute(GameObject player, GameObject target)
    {
        if (target.TryGetComponent<Enemy>(out var enemy))
        {
            AttackEnemy(enemy);
        }
    }
}
